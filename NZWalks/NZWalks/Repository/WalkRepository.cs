using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Domain;

namespace NZWalks.Repository
{
    public class WalkRepository : IWalkRepository
    {

        public readonly NZWalksDbContext dbContext;

        public WalkRepository(NZWalksDbContext _dbContext)
        {
            dbContext = _dbContext;
        }


        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }


        public async Task<List<Walk>> GetAllAsync(string? region = null,
             string? difficulity = null,
            double? minLength = null,
           double? maxLength = null,
            string? name = null,
            bool? isAsyindingLength = null,
            int pageNumber = 1,
            int pageSize = 3)
        {

           var queryWalks =  dbContext.walks.Include(x=>x.Region).Include(x => x.Difficulty).AsQueryable();
            //Filtering
            if (!string.IsNullOrWhiteSpace(region))
                queryWalks = queryWalks.Where(x => x.Region.Name.ToLower().Contains(region.ToLower()));
            if (!string.IsNullOrWhiteSpace(difficulity))
                queryWalks = queryWalks.Where(x => x.Difficulty.Name.ToLower().Contains(difficulity.ToLower()));
            if (!string.IsNullOrWhiteSpace(name))
                queryWalks = queryWalks.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            if (!(minLength == null))
                queryWalks = queryWalks.Where(x => x.LengthKm >= minLength);
            if (!(maxLength == null))
                queryWalks = queryWalks.Where(x => x.LengthKm <= maxLength);
            //sorting
            if (!(isAsyindingLength == null))
                queryWalks = isAsyindingLength.Value ? queryWalks.OrderBy(x => x.LengthKm) : queryWalks.OrderByDescending(x => x.LengthKm);
            // total number befor pagination
           

            return await
                queryWalks
                .Skip((pageNumber-1)*pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<Walk?> GetByIdAsync(Guid id)
        {

            return await dbContext.walks
                . Include(x => x.Region)
                .Include(x => x.Difficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var DeletedWalk = await dbContext.walks .Include(x => x.Region)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (DeletedWalk == null) return null;
            dbContext.walks.Remove(DeletedWalk);
            await dbContext.SaveChangesAsync();
            return DeletedWalk;
        }

        public async Task<Walk?> UpDateAsync(Guid id, Walk walk)
        {
            var existingWalk = await dbContext.walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            { return null; }
            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.LengthKm = walk.LengthKm;

            await dbContext.SaveChangesAsync();
            return existingWalk;
        }



    }
}

