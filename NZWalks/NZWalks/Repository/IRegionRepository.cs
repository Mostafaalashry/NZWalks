using System;
using NZWalks.Models.Domain;

namespace NZWalks.Repository
{
	public interface IRegionRepository
	{
        Task<List<Region>> GetAllAsync();                    // Read all regions
        Task<Region?> GetByIdAsync(Guid id);                  // Read a single region
        Task<Region> CreateAsync(Region region);                         // Create
        Task<Region?> UpdateAsync(Guid id, Region region);              // Update
        Task<Region?> DeleteAsync(Guid id);                            // Delete

    }
}

