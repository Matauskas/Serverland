namespace Serverland.Auth.Model;

public class ShopRoles
{
    public const string Admin = nameof(Admin);
    public const string ShopUser = nameof(ShopUser);

    public static readonly IReadOnlyCollection<string> All = new[] { Admin, ShopUser };
}