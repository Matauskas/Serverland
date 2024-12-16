namespace Serverland.Data.Entities;
using Serverland.Auth.Model;
using System.ComponentModel.DataAnnotations;
using Serverland.Data.DatabaseObjects;

public class Server
{
    public int Id { get; set; }
    public required string Model { get; set; }
    public required int Disk_Count { get; set; }
    public required string Generation { get; set; }
    public required double Weight { get; set; }
    public required bool OS { get; set; }
    public int categoryId { get; set; }

    [Required]
    public required string UserId { get; set; }
    public ShopUser User { get; set; }
    public ServerDto ToDto()
    {
        return new ServerDto(Id, Model, Disk_Count, Generation, Weight, OS, categoryId);
    }
}