namespace Application.DTOs.Usuario;

public class UserProfileDTO
{
    public string Id { get; set; }
    public string? Nome { get; set; }
    public string? Bio { get; set; }
    public string? FavoriteGenero { get; set; }
    public string? ProfilePicture { get; set; }
}