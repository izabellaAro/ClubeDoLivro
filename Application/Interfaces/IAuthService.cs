using Application.DTOs.Usuario;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<UsuarioResponseDTO> RegisterAsync(UsuarioRegisterDTO dto);
    Task<UsuarioResponseDTO> LoginAsync(UsuarioLoginDTO dto);
}
