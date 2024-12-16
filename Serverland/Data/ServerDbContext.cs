using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Serverland.Data.Entities;
using Serverland.Auth.Model;

namespace Serverland.Data
{
    public class ServerDbContext : IdentityDbContext<ShopUser>
    {
        private readonly IConfiguration _configuration;

        public ServerDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Server> Servers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQL"));
            Console.WriteLine($"Connection String: {_configuration.GetConnectionString("PostgreSQL")}"); // Log the connection string
        }

    }
}
