using System;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repository;
using NZWalks.CustomActionFilters;
namespace NZWalks.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WalkController:ControllerBase
	{
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository1;


        public WalkController(IMapper _mapper , IWalkRepository walkRepository)
		{
			mapper = _mapper;
            walkRepository1 = walkRepository;

        }

		[HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateAsync([FromBody]AddWalkDTO addWalkDTO)
		{
			if (addWalkDTO == null) return NoContent();

			var WalkDomain = mapper.Map<Walk>(addWalkDTO);

			var createdWalk = await walkRepository1.CreateAsync(WalkDomain);

			var walkDTO = mapper.Map<WalkDTO>(createdWalk);

            return Ok(walkDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? region,
            [FromQuery] string? difficulity,
            [FromQuery] double? minLength,
            [FromQuery] double? maxLength,
            [FromQuery] string? name,
            [FromQuery] bool? isAsyindingLength,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 3)
        {
          
            var WalkDomain = await walkRepository1.GetAllAsync(region,difficulity,minLength,maxLength,name, isAsyindingLength,pageNumber,pageSize);

            var walkDTO = mapper.Map <List< WalkDTO >> (WalkDomain);
            
            return Ok(walkDTO);
        }
        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            //get region from  db
            var region = await walkRepository1.GetByIdAsync(Id);
            //map result to dto
            return region is null ? NotFound() : Ok(mapper.Map<RegionDTO>(region));
        }
        [HttpDelete]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid Id)
        {
            //get region from  db
            var region = await walkRepository1.DeleteAsync(Id);
            //map result to dto
            return region is null ? NotFound() : Ok(mapper.Map<RegionDTO>(region));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, [FromBody] UpdateWalkDTO request)
        {
          

                // Map DTO to domain model
                var updatedWalk = mapper.Map<Walk>(request);

                // Attempt to update
                var result = walkRepository1.UpDateAsync(id, updatedWalk);

                if (result is null) return NotFound($"Region with ID {id} was not found.");

                // Map result to DTO
                return Ok(mapper.Map<WalkDTO>(result));
            }
           
          

        }
    }


