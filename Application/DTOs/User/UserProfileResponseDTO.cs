namespace Application.DTOs.User;

public class UserProfileResponseDTO
{
    public string Id { get; set; }
    public string? Nome { get; set; }
    public string? Bio { get; set; }
    public string? FavoriteGenero { get; set; }
    public string? ProfilePicture { get; set; }
}