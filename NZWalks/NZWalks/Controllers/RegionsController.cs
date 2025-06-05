using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repository;
using System.ComponentModel.DataAnnotations;
using NZWalks.CustomActionFilters;
using Microsoft.AspNetCore.Authorization;

namespace NZWalks.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController:ControllerBase
	{
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository _RegionRepository, IMapper _mapper)
        {
            regionRepository = _RegionRepository;
            mapper = _mapper;
        }

        //
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        { //get regions from database
            var regions = await regionRepository.GetAllAsync();
            //map to dto
            return Ok(mapper.Map<List<RegionDTO>>(regions));
        }
        //
        //
        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            //get region from  db
            var region = await regionRepository.GetByIdAsync(Id);
            //map result to dto
            return region is null ? NotFound() : Ok(mapper.Map<RegionDTO>(region));

        }
        //
        //
        [HttpPost]
        public async Task<IActionResult> AddRegion([FromBody] RequestRegionDTO request)
        {
            if (ModelState.IsValid)
            {
                // Map DTO to domain model
                var region = mapper.Map<Region>(request);

                // Create region in the database
                var createdRegion = await regionRepository.CreateAsync(region);

                // Map domain model to response DTO
                var response = mapper.Map<RegionDTO>(createdRegion);

                // Return 201 Created with location header pointing to the newly created resource
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = response.Id },
                    response
                );
            }

          else
            {
                return BadRequest("Request body cannot be null.");
            }

         
        }
        //
        //
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO request)
        {
           
                // Map DTO to domain model
                var updatedRegion = mapper.Map<Region>(request);

                // Attempt to update
                var result = await regionRepository.UpdateAsync(id, updatedRegion);

                if (result is null) return NotFound($"Region with ID {id} was not found.");

                // Map result to DTO
                return Ok(mapper.Map<RegionDTO>(result));
          

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
            // Map result to DTO
            return Ok(mapper.Map<RegionDTO>(deletedRegion));
        }

    }
}

