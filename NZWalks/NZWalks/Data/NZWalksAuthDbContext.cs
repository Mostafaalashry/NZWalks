using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Design;

//using Microsoft.AspNet.Identity.EntityFramework;

namespace NZWalks.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext<IdentityUser>

    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options)
            : base(options)
        {
        }
  
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        //    var constr = config.GetSection("ConnectionStrings:NZWalksAuthConnectionString").Value;

        //    optionsBuilder.UseSqlServer(constr);
        //}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var ReaderRoleId = "c3f519cb-0b96-4f25-9419-0f5281f4e8df";
            var WriterRoleId = "a0d8e7b4-7c57-4b0f-b298-69a93d7a99ce";

            var roles = new List<IdentityRole>
             {
               new IdentityRole
               {
                   Id = ReaderRoleId,
                   ConcurrencyStamp=ReaderRoleId,
                   Name = "Reader",
                   NormalizedName = "READER"
               },
              new IdentityRole
               {
                  Id = WriterRoleId,
                  ConcurrencyStamp = WriterRoleId,      
                  Name = "Writer",
                  NormalizedName = "WRITER"
               }
             };
            builder.Entity<IdentityRole>().HasData(roles);

        }
        // Optional: Add your own DbSet properties if needed
        // public DbSet<YourCustomEntity> YourEntities { get; set; }
    }
}

// dotnet ef migrations add AuthDbMigration2 --context NZWalksAuthDbContext
