using Application.DTOs.Result;
using Application.DTOs.User;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<OperationResult<string>> RegisterAsync(UserRegisterDTO dto);
    Task<UserTokenResponseDTO> LoginAsync(UserLoginDTO dto);
}