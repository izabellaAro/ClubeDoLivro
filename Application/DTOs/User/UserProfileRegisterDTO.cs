using Microsoft.AspNetCore.Http;

namespace Application.DTOs.User;

public class UserProfileRegisterDTO
{
    public string? Nome { get; set; }
    public string? Bio { get; set; }
    public string? FavoriteGenero { get; set; }
    public IFormFile? Image { get; set; }
}