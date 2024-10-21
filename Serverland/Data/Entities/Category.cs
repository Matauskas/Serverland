namespace Serverland.Data.Entities;

public class Category
{
    public int Id { get; set; }
    public required string Manifacturer { get; set; }
    public required string ServerType { get; set; }
}