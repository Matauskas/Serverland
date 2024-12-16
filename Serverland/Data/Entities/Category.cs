namespace Serverland.Data.Entities;
using Serverland.Auth.Model;
using System.ComponentModel.DataAnnotations;
using Serverland.Data.DatabaseObjects;

public class Category
{
    public int Id { get; set; }
    public required string Manifacturer { get; set; }
    public required string ServerType { get; set; }

    [Required]
    public required string UserId { get; set; }
     public ShopUser User { get; set; }

    public CategoryDto ToDto()
    {
        return new CategoryDto(Id, Manifacturer, ServerType);
    }
}