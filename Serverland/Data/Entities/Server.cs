namespace Serverland.Data.Entities;

public class Server
{
    public int Id { get; set; }
    public required string Model { get; set; }
    public required int Disk_Count { get; set; }
    public required string Generation { get; set; }
    public required double Weight { get; set; }
    public required bool OS { get; set; }
    public int categoryId { get; set; }
}