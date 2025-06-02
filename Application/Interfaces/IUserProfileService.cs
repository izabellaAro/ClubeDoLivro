using Application.DTOs.Result;
using Application.DTOs.Usuario;

namespace Application.Interfaces;

public interface IUserProfileService
{
    Task<OperationResult<UserProfileDTO>> GetProfileAsync(Guid userId);
    Task<OperationResult<string>> UpdateProfileAsync(Guid userId, UserProfileDTO dto);
    Task<IEnumerable<UserProfileDTO>> GetAllAsync(int skip = 0, int take = 20);
    Task<OperationResult<string>> DeleteProfileAsync(string userId);
}