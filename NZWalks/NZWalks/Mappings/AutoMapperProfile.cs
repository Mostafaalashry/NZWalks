using System;
using AutoMapper;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;

namespace NZWalks.Mappings
{
	public class AutoMapperProfile:Profile
	{
		public AutoMapperProfile()
		{
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<Region, RequestRegionDTO>().ReverseMap();
            CreateMap<Region, UpdateRegionRequestDTO>().ReverseMap();
            CreateMap<Walk, AddWalkDTO>().ReverseMap();
            CreateMap<Walk, WalkDTO>().ReverseMap();
            CreateMap<Walk, UpdateWalkDTO>().ReverseMap();
        }
    }

}

