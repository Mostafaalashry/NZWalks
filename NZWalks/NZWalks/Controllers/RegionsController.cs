using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repository;

namespace NZWalks.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class RegionsController:ControllerBase
	{
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZWalksDbContext dbcontext, IRegionRepository _RegionRepository)
        {
            regionRepository = _RegionRepository;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await regionRepository.GetAllAsync();
            var regionDTOs = regions.Select(r => new RegionDTO
            {
                Id = r.Id,
                Name = r.Name,
                Code = r.Code,
                RegionImageUrl = r.RegionImageUrl
            }).ToList();
            return Ok(regions);
        }


        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var region = await regionRepository.GetByIdAsync(Id);
          
            return region is null ? NotFound():Ok(
                new RegionDTO
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl

                });
        }

        [HttpPost]
        public async Task<IActionResult> AddRegion([FromBody] RequestRegionDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            // Map DTO to domain model
            var region = new Region
            {
                Code = request.Code,
                Name = request.Name,
                RegionImageUrl = request.RegionImageUrl
            };

            // Create region in the database
            var createdRegion = await regionRepository.CreateAsync(region);

            // Map domain model to response DTO
            var response = new RegionDTO
            {
                Id = createdRegion.Id,
                Code = createdRegion.Code,
                Name = createdRegion.Name,
                RegionImageUrl = createdRegion.RegionImageUrl
            };

            // Return 201 Created with location header pointing to the newly created resource
            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Id },
                response
            );
        }


        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO request)
        {
            if (request is null) return BadRequest("Request body cannot be null.");
            
            // Map DTO to domain model
            var updatedRegion = new Region
            {
                Code = request.Code,
                Name = request.Name,
                RegionImageUrl = request.RegionImageUrl
            };

            // Attempt to update
            var result = await regionRepository.UpdateAsync(id, updatedRegion);

            if (result is null) return NotFound($"Region with ID {id} was not found.");

            // Map result to DTO
            var response = new RegionDTO
            {
                Id = result.Id,
                Code = result.Code,
                Name = result.Name,
                RegionImageUrl = result.RegionImageUrl
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            var deletedRegion = await regionRepository.DeleteAsync(id);

            if (deletedRegion == null)
            {
                return NotFound();
            }

            var response = new RegionDTO
            {
                Id = deletedRegion.Id,
                Code = deletedRegion.Code,
                Name = deletedRegion.Name,
                RegionImageUrl = deletedRegion.RegionImageUrl
            };

            return Ok(response);
        }



    }
}

