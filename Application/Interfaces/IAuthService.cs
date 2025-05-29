using Application.DTOs.Usuario;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<UsuarioCreateResponseDTO> RegisterAsync(UsuarioRegisterDTO dto);
    Task<UsuarioTokenResponseDTO> LoginAsync(UsuarioLoginDTO dto);
}
