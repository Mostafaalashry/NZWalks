﻿using System;
namespace NZWalks.Models.Domain
{
	public class Walk
	{

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthKm { get; set; }
        public string WalkImageUrl { get; set; }

        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }

        //navigation Properties
        public Difficulty Difficulty { get; set; }
        public Region Region { get; set; }

    }
}

