using System.ComponentModel.DataAnnotations;
using Serverland.Auth.Model;
public class Session
{
    public Guid Id { get; set; }
    public string lastRefreshToken { get; set; }
    public DateTimeOffset InitiatedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public bool isRevoked { get; set; }

    [Required]
    public required string UserId { get; set; }
    
    public ShopUser User { get; set; }

}