using System;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.Models.DTO
{
	public class UpdateWalkDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        [Range(0, 200)]
        public double LengthKm { get; set; }

        public string WalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }

    }
}

