using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace NZWalks.Models.DTO
{
	public class RequestRegionDTO
	{
        [Required]
        [MinLength(3,ErrorMessage ="code has to be minimum 3 character")]
        [MaxLength(3, ErrorMessage = "code has to be maximum 3 character")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "code has to be maximum 100 character")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}

