using Application.DTOs.Result;
using Application.DTOs.User;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public UserService(UserManager<IdentityUser<Guid>> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<OperationResult<string>> UpdateUserAsync(UserUpdateDTO dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
        if (user == null)
            return OperationResult<string>.Failure("Usuário não encontrado.");

        var result = await UpdateCommonUserData(user, dto.Email, dto.Nome, dto.Senha);
        if (!result.Succeeded)
            return result;

        if (!string.IsNullOrEmpty(dto.Role))
        {
            result = await UpdateUserRole(user, dto.Role);
            if (!result.Succeeded)
                return result;
        }

        return OperationResult<string>.Success("Usuário atualizado com sucesso.");
    }

    public async Task<OperationResult<string>> UpdateProfileAsync(Guid userId, UserUpdateCredentialsDTO dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return OperationResult<string>.Failure("Usuário não encontrado.");

        var result = await UpdateCommonUserData(user, dto.Email, dto.Nome, dto.Senha);
        if (!result.Succeeded)
            return result;

        return OperationResult<string>.Success("Usuário atualizado com sucesso.");
    }

    private async Task<OperationResult<string>> UpdateCommonUserData(IdentityUser<Guid> user, string? email, string? nome, string? senha)
    {
        if (!string.IsNullOrEmpty(email) && email != user.Email)
        {
            var emailResult = await _userManager.SetEmailAsync(user, email);
            if (!emailResult.Succeeded)
                return ErrorFromIdentityResult(emailResult);
        }

        if (!string.IsNullOrEmpty(nome) && nome != user.UserName)
        {
            var usernameResult = await _userManager.SetUserNameAsync(user, nome);
            if (!usernameResult.Succeeded)
                return ErrorFromIdentityResult(usernameResult);
        }

        if (!string.IsNullOrEmpty(senha))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var senhaResult = await _userManager.ResetPasswordAsync(user, token, senha);
            if (!senhaResult.Succeeded)
                return ErrorFromIdentityResult(senhaResult);
        }

        return OperationResult<string>.Success(string.Empty);
    }

    private async Task<OperationResult<string>> UpdateUserRole(IdentityUser<Guid> user, string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
            return OperationResult<string>.Failure($"Role '{role}' não existe.");

        var currentRoles = await _userManager.GetRolesAsync(user);
        var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeRolesResult.Succeeded)
            return ErrorFromIdentityResult(removeRolesResult);

        var addRoleResult = await _userManager.AddToRoleAsync(user, role);
        if (!addRoleResult.Succeeded)
            return ErrorFromIdentityResult(addRoleResult);

        return OperationResult<string>.Success(string.Empty);
    }

    private OperationResult<string> ErrorFromIdentityResult(IdentityResult result)
    {
        var error = string.Join(", ", result.Errors.Select(e => e.Description));
        return OperationResult<string>.Failure(error);
    }

    public async Task<OperationResult<UserResponseDTO>> GetUserByIdAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return OperationResult<UserResponseDTO>.Failure("Usuário não encontrado.");

        var roles = await _userManager.GetRolesAsync(user);

        var dto = new UserResponseDTO
        {
            Id = user.Id,
            Nome = user.UserName,
            Email = user.Email,
            Role = roles
        };

        return OperationResult<UserResponseDTO>.Success(dto);
    }

    public async Task<List<UserResponseDTO>> GetAllUsersAsync()
    {
        var users = _userManager.Users.ToList();
        var result = new List<UserResponseDTO>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            result.Add(new UserResponseDTO
            {
                Id = user.Id,
                Nome = user.UserName,
                Email = user.Email,
                Role = roles
            });
        }

        return result;
    }

    public async Task<OperationResult<string>> DeleteUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return OperationResult<string>.Failure("Usuário não encontrado.");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return OperationResult<string>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));

        return OperationResult<string>.Success("Usuário excluído com sucesso.");
    }
}