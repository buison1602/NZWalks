using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    // DbContext có vai trò thao tác với CSDL 
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        // DbSet là một tập hợp các đối tượng được lưu trong CSDL
        // DbSet này sẽ tương ứng với một bảng trong CSDL
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Difficulties
            // Easy, Medium, Hard 

            var difficulties = new List<Difficulty>
            {
                new Difficulty() 
                { 
                    Id = Guid.Parse("ad8182f8-b442-4ea4-9fe0-dc95f96455a7"), 
                    Name = "Easy" 
                },
                new Difficulty() 
                { 
                    Id = Guid.Parse("afa9882b-36da-44e5-965d-9196841547ae"), 
                    Name = "Medium" 
                },
                new Difficulty() 
                { 
                    Id = Guid.Parse("62a957aa-a3f3-4e18-bfef-211bd8696da9"), 
                    Name = "Hard" 
                }
            };

            // Seed difficulties to the database 
            modelBuilder.Entity<Difficulty>().HasData(difficulties);




            // Seed data for Regions
            var regions = new List<Region>()
            {
                new Region
                {
                    Id = Guid.Parse("302cd4ff-5732-411d-82de-cf1f208379fb"),
                    Code = "AUK 1",
                    Name = "Auckland 1",
                    RegionImageUrl = "https://www.doc.govt.nz/globalassets/images/conservation/parks-and-recreation/places-to-visit/auckland/auckland-region.jpg"
                },

                new Region
                {
                    Id = Guid.Parse("302cd4ff-5732-411d-82de-cf1f208379fc"),
                    Code = "AUK 2",
                    Name = "Auckland 2",
                    RegionImageUrl = "https://www.doc.govt.nz/globalassets/images/conservation/parks-and-recreation/places-to-visit/auckland/auckland-region.jpg"
                },

                new Region
                {
                    Id = Guid.Parse("5ca0dd19-f32c-4921-9908-57c64c057a1d"),
                    Code = "AUK 3",
                    Name = "Auckland 3",
                    RegionImageUrl = "https://www.doc.govt.nz/globalassets/images/conservation/parks-and-recreation/places-to-visit/auckland/auckland-region.jpg"
                },

                new Region
                {
                    Id = Guid.Parse("29219b8f-d095-4241-a0a7-b1bb7c233a5b"),
                    Code = "AUK 4",
                    Name = "Auckland 4",
                    RegionImageUrl = "https://www.doc.govt.nz/globalassets/images/conservation/parks-and-recreation/places-to-visit/auckland/auckland-region.jpg"
                },

                new Region
                {
                    Id = Guid.Parse("d21e272f-a127-4ab1-a8b0-c03d61f10035"),
                    Code = "AUK 5",
                    Name = "Auckland 5",
                    RegionImageUrl = "https://www.doc.govt.nz/globalassets/images/conservation/parks-and-recreation/places-to-visit/auckland/auckland-region.jpg"
                },
            };

            // Seed regions to the database
            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
