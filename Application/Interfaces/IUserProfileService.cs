using Application.DTOs.Result;
using Application.DTOs.User;

namespace Application.Interfaces;

public interface IUserProfileService
{
    Task<OperationResult<UserProfileResponseDTO>> GetProfileAsync(Guid userId);
    Task<OperationResult<string>> UpdateProfileAsync(Guid userId, UserProfileRegisterDTO dto);
    Task<IEnumerable<UserProfileResponseDTO>> GetAllAsync(int skip = 0, int take = 20);
    Task<OperationResult<string>> DeleteProfileAsync(string userId);
}