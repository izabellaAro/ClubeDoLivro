using Application.DTOs.Result;
using Application.DTOs.Usuario;

namespace Application.Interfaces;

public interface IUserService
{
    Task<OperationResult<string>> UpdateUserAsync(UsuarioUpdateDTO dto);
    Task<OperationResult<string>> UpdateProfileAsync(Guid userId, UsuarioUpdateProfileDTO dto);
    Task<OperationResult<UsuarioResponseDTO>> GetUserByIdAsync(Guid userId);
    Task<List<UsuarioResponseDTO>> GetAllUsersAsync();
    Task<OperationResult<string>> DeleteUserAsync(Guid userId);
}