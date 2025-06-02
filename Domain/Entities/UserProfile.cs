using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class UserProfile
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public string? Bio { get; set; }
    public string? FavoriteGenero { get; set; }
    public string? ProfilePicture { get; set; }

    public IdentityUser<Guid> User { get; set; } = null!;
}