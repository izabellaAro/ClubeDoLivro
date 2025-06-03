namespace Application.DTOs.User;

public class UserRegisterDTO
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string Role { get; set; } = "Membro";
}
