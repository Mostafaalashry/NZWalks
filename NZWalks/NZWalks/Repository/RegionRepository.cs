﻿using System;
using NZWalks.Data;
using NZWalks.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.Repository
{
    public class RegionRepository : IRegionRepository
    {
        public readonly NZWalksDbContext dbContext;

        public RegionRepository(NZWalksDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
            { return null; }
            dbContext.Regions.Remove(existingRegion);
            await dbContext.SaveChangesAsync();
            return existingRegion;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }


        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
            { return null; }
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;
            existingRegion.Code = region.Code;
            await dbContext.SaveChangesAsync();
            return existingRegion;
        }

    }
}

