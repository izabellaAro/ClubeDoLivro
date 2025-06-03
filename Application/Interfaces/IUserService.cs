using Application.DTOs.Result;
using Application.DTOs.User;

namespace Application.Interfaces;

public interface IUserService
{
    Task<OperationResult<string>> UpdateUserAsync(UserUpdateDTO dto);
    Task<OperationResult<string>> UpdateProfileAsync(Guid userId, UserUpdateCredentialsDTO dto);
    Task<OperationResult<UserResponseDTO>> GetUserByIdAsync(Guid userId);
    Task<List<UserResponseDTO>> GetAllUsersAsync();
    Task<OperationResult<string>> DeleteUserAsync(Guid userId);
}