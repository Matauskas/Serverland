namespace Serverland.Data.Entities;
using Serverland.Data.DatabaseObjects;

public class Category
{
    public int Id { get; set; }
    public required string Manifacturer { get; set; }
    public required string ServerType { get; set; }

    public CategoryDto ToDto()
    {
        return new CategoryDto(Id, Manifacturer, ServerType);
    }
}