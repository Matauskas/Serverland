using Microsoft.EntityFrameworkCore;
using Serverland.Data.Entities;

namespace Serverland.Data
{
    public class ServerDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ServerDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Server> Servers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Part> Parts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQL"));
            Console.WriteLine($"Connection String: {_configuration.GetConnectionString("PostgreSQL")}"); // Log the connection string
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeding initial data for Server entity
            modelBuilder.Entity<Server>().HasData(
                new Server
                {
                    Id = 1,
                    Model = "Hdzl5a8hdf6",
                    Disk_Count = 2,
                    Generation = "ugXGzF",
                    Weight = 12.5,
                    OS = true,
                    categoryId = 1
                },
                new Server
                {
                    Id = 2,
                    Model = "Xyz5000",
                    Disk_Count = 4,
                    Generation = "GenX",
                    Weight = 18.7,
                    OS = false,
                    categoryId = 2
                }
            );

            // Seeding initial data for Part entity
            modelBuilder.Entity<Part>().HasData(
                new Part
                {
                    Id = 1,
                    CPU = "Intel Xeon E5-2620",
                    RAM = "32GB DDR4",
                    Raid = "RAID 0/1/5/10",
                    Network = "1Gbps Ethernet",
                    SSD = "512GB",
                    HDD = "2TB",
                    PSU = "750W",
                    Rails = true,
                    serverId = 1
                },
                new Part
                {
                    Id = 2,
                    CPU = "Intel Xeon E5-2670",
                    RAM = "64GB DDR4",
                    Raid = "RAID 5/6",
                    Network = "1Gbps Ethernet",
                    SSD = "1TB",
                    HDD = "4TB",
                    PSU = "850W",
                    Rails = false,
                    serverId = 1
                },
                new Part
                {
                    Id = 3,
                    CPU = "AMD EPYC 7302P",
                    RAM = "128GB DDR4",
                    Raid = "RAID 10",
                    Network = "10Gbps Ethernet",
                    SSD = "2TB",
                    HDD = "8TB",
                    PSU = "1000W",
                    Rails = true,
                    serverId = 2
                },
                new Part
                {
                    Id = 4,
                    CPU = "Intel Core i9-10900X",
                    RAM = "64GB DDR4",
                    Raid = "RAID 0/1",
                    Network = "1Gbps Ethernet",
                    SSD = "512GB",
                    HDD = "1TB",
                    PSU = "650W",
                    Rails = true,
                    serverId = 2
                },
                new Part
                {
                    Id = 5,
                    CPU = "AMD Ryzen Threadripper 3990X",
                    RAM = "256GB DDR4",
                    Raid = "RAID 0/1/5",
                    Network = "10Gbps Ethernet",
                    SSD = "4TB",
                    HDD = "5TB", // No HDD for this part
                    PSU = "1200W",
                    Rails = false,
                    serverId = 1
                }
            );
        }
    }
}
