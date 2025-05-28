namespace Application.DTOs.Usuario;

public class UsuarioResponseDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } 
    public string Email { get; set; } 
    public string Token { get; set; }
    public IList<string> Roles { get; set; } = [];
}
