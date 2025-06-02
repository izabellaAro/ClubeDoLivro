using Application.DTOs.Result;
using Application.DTOs.Usuario;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<OperationResult<string>> RegisterAsync(UsuarioRegisterDTO dto);
    Task<UsuarioTokenResponseDTO> LoginAsync(UsuarioLoginDTO dto);
}