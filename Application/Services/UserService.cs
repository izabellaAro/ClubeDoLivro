using Application.DTOs.Result;
using Application.DTOs.Usuario;
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

    public async Task<OperationResult> UpdateUserAsync(UsuarioUpdateDTO dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
        if (user == null)
            return OperationResult.Failure("Usuário não encontrado.");

        var result = await UpdateCommonUserData(user, dto.Email, dto.Nome, dto.Senha);
        if (!result.Succeeded)
            return result;

        if (!string.IsNullOrEmpty(dto.Role))
        {
            result = await UpdateUserRole(user, dto.Role);
            if (!result.Succeeded)
                return result;
        }

        return OperationResult.Success();
    }

    public async Task<OperationResult> UpdateProfileAsync(Guid userId, UsuarioUpdateProfileDTO dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return OperationResult.Failure("Usuário não encontrado.");

        var result = await UpdateCommonUserData(user, dto.Email, dto.Nome, dto.Senha);
        if (!result.Succeeded)
            return result;

        return OperationResult.Success();
    }

    private async Task<OperationResult> UpdateCommonUserData(IdentityUser<Guid> user, string? email, string? nome, string? senha)
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

        return OperationResult.Success();
    }

    private async Task<OperationResult> UpdateUserRole(IdentityUser<Guid> user, string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
            return OperationResult.Failure($"Role '{role}' não existe.");

        var currentRoles = await _userManager.GetRolesAsync(user);
        var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeRolesResult.Succeeded)
            return ErrorFromIdentityResult(removeRolesResult);

        var addRoleResult = await _userManager.AddToRoleAsync(user, role);
        if (!addRoleResult.Succeeded)
            return ErrorFromIdentityResult(addRoleResult);

        return OperationResult.Success();
    }

    private OperationResult ErrorFromIdentityResult(IdentityResult result)
    {
        var error = string.Join(", ", result.Errors.Select(e => e.Description));
        return OperationResult.Failure(error);
    }
}