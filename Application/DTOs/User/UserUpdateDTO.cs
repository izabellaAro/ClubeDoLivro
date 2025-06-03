namespace Application.DTOs.User;

public class UserUpdateDTO
{
    public string UserId { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Senha { get; set; }
    public string? Role { get; set; }
}
