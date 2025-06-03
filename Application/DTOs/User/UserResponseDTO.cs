namespace Application.DTOs.User;

public class UserResponseDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } 
    public string Email { get; set; } 
    public IList<string> Role { get; set; } = [];
}