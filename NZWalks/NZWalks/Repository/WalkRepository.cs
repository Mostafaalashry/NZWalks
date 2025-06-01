using System;
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

        public Task<List<Walk>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}

