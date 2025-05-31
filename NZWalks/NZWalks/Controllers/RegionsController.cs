using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;

namespace NZWalks.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class RegionsController:ControllerBase
	{
		private readonly NZWalksDbContext _dbcontext;

        public RegionsController(NZWalksDbContext dbcontext )
        {
			_dbcontext = dbcontext;

		}

        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await _dbcontext.Regions.Select( r => new RegionDTO
             {
                Id = r.Id,
                Name=r.Name,
                Code=r.Code,
                RegionImageUrl= r.RegionImageUrl
             }).ToListAsync();
            return Ok(regions);
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var region = await _dbcontext.Regions
                .Select(r => new RegionDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    Code = r.Code,
                    RegionImageUrl = r.RegionImageUrl
                })
                .FirstOrDefaultAsync(x => x.Id == Id);
                
            if (region==null)
            {
                return NotFound();
            }
            return Ok(region);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegion([FromBody] RequestRegionDTO request)
        {
            // Map DTO to Entity
            var region = new Region
            {
               // Id = Guid.NewGuid(), // Generate new Guid
                Code = request.Code,
                Name = request.Name,
                RegionImageUrl = request.RegionImageUrl
            };

            // Add to database
            await _dbcontext.Regions.AddAsync(region);
            await _dbcontext.SaveChangesAsync();

            // Map back to DTO (optional, but useful)
            var regionDTO = new RegionDTO
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = region.Id }, regionDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO request)
        {
            // Check if region exists
            var existingRegion = await _dbcontext.Regions.FirstOrDefaultAsync(r => r.Id == id);

            if (existingRegion == null)
            {
                return NotFound();
            }

            // Update the region entity
            existingRegion.Code = request.Code;
            existingRegion.Name = request.Name;
            existingRegion.RegionImageUrl = request.RegionImageUrl;

            // Save changes to DB
            await _dbcontext.SaveChangesAsync();

            // Map to DTO for return
            var regionDTO = new RegionDTO
            {
                Id = existingRegion.Id,
                Code = existingRegion.Code,
                Name = existingRegion.Name,
                RegionImageUrl = existingRegion.RegionImageUrl
            };

            return Ok(regionDTO);
        }


        [HttpDelete]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid Id)
        {

            var existingRegion = await _dbcontext.Regions.FirstOrDefaultAsync(x => x.Id == Id);

            if (existingRegion == null)
            {
                return NotFound();
            }
            _dbcontext.Regions.Remove(existingRegion);
            await _dbcontext.SaveChangesAsync();

            return NoContent();
        }


    }
}

