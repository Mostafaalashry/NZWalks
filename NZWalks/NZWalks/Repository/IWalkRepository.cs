using System;
using NZWalks.Models.Domain;

namespace NZWalks.Repository
{
	public interface IWalkRepository
	{
        Task<List<Walk>> GetAllAsync();                    // Read all regions
        Task<Walk> CreateAsync(Walk walk);                         // Create

    }
}

