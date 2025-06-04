using System;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.Domain;

namespace NZWalks.Repository
{
	public interface IWalkRepository
	{
        Task<List<Walk>> GetAllAsync(string? region=null,
            string? difficulity=null,
            double? minLength=null,
            double? maxLength=null,
            string? name=null,
            bool? isAsyindingLength = null,
            int pageNumber = 1,
            int pageSize = 3);                    // Read all regions
        Task<Walk> CreateAsync(Walk walk);                         // Create
        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk?> DeleteAsync(Guid id);
        Task<Walk?> UpDateAsync(Guid id,Walk walk);
    }
}

