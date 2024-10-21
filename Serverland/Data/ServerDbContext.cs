using Microsoft.EntityFrameworkCore;
using Serverland.Data.Entities;

namespace Serverland.Data;

public class ServerDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Server> Servers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Part> Parts { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(("PostgreSQL")));
        Console.WriteLine($"Connection String: {configuration.GetConnectionString(("PostgreSQL"))}"); // Log the connection string
    }
}