using Application.DTOs.Result;
using Application.DTOs.Usuario;

namespace Application.Interfaces;

public interface IUserService
{
    Task<OperationResult> UpdateUserAsync(UsuarioUpdateDTO dto);
    Task<OperationResult> UpdateProfileAsync(Guid userId, UsuarioUpdateProfileDTO dto);
}