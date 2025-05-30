namespace Application.DTOs.Usuario;

public class UsuarioResponseDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } 
    public string Email { get; set; } 
    public IList<string> Role { get; set; } = [];
}