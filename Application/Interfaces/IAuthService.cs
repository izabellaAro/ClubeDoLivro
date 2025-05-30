using Application.DTOs.Usuario;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<UsuarioResponseDTO> RegisterAsync(UsuarioRegisterDTO dto);
    Task<UsuarioTokenResponseDTO> LoginAsync(UsuarioLoginDTO dto);
}
