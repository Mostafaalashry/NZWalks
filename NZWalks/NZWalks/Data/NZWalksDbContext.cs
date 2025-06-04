using System;
using Microsoft.EntityFrameworkCore;
using NZWalks.Models.Domain;

namespace NZWalks.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {

        }

        /*

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

	  */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var constr = config.GetSection("ConnectionStrings:NZWalksConnectionString").Value;

            optionsBuilder.UseSqlServer(constr);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var difficulties = new List<Difficulty>
                 {
                       new Difficulty { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Easy" },
                       new Difficulty { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Medium" },
                       new Difficulty { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Hard" }
                  };
            var regions = new List<Region> {
                new Region
                {
            Id = Guid.Parse("aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
            Name = "Auckland",
            Code = "AKL",
            RegionImageUrl = "https://example.com/auckland.jpg"
                },
        new Region
                {
            Id = Guid.Parse("aaaaaaa2-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
            Name = "Wellington",
            Code = "WLG",
            RegionImageUrl = "https://example.com/wellington.jpg"
                },
        new Region
               {
            Id = Guid.Parse("aaaaaaa3-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
            Name = "Canterbury",
            Code = "CNT",
            RegionImageUrl = "https://example.com/canterbury.jpg"
               }
            };


            modelBuilder.Entity<Region>().HasData(regions);
            modelBuilder.Entity<Difficulty>().HasData(difficulties);


        }


        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> walks { get; set; }


    }
}

