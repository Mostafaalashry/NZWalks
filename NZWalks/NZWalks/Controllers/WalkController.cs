using System;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repository;

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
		public async Task<IActionResult> CreateAsync([FromBody]AddWalkDTO addWalkDTO)
		{
			if (addWalkDTO == null) return NoContent();

			var WalkDomain = mapper.Map<Walk>(addWalkDTO);

			var createdWalk = await walkRepository1.CreateAsync(WalkDomain);

			var walkDTO = mapper.Map<WalkDTO>(createdWalk);

            return Ok(walkDTO);
        }

    }
}

/*
 {
  "name": "string",
  "description": "string",
  "lengthKm": 0,
  "walkImageUrl": "string",
  "difficultyId": "A1B2C3D4-E5F6-7890-ABCD-1234567890AB",
  "regionId": "89c021fd-b781-4c30-2ea1-08dd9fa8c51f"
}
 */