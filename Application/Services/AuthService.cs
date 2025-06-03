using Application.DTOs.Result;
using Application.DTOs.User;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IUserProfileRepository _userProfileRepository;

    public AuthService(UserManager<IdentityUser<Guid>> userManager, RoleManager<IdentityRole<Guid>> roleManager,
        IConfiguration configuration, IUserProfileRepository userProfileRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _userProfileRepository = userProfileRepository;
    }

    public async Task<UserTokenResponseDTO> LoginAsync(UserLoginDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email)
            ?? throw new ApplicationException("Usuário não encontrado.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Senha);

        if (!isPasswordValid)
            throw new ApplicationException("Credenciais inválidas.");

        var roles = await _userManager.GetRolesAsync(user);

        var token = await GenerateJwtToken(user);

        return new UserTokenResponseDTO
        {
            Token = token
        };
    }

    public async Task<OperationResult<string>> RegisterAsync(UserRegisterDTO dto)
    {
        var user = new IdentityUser<Guid>
        {
            UserName = dto.Nome,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Senha);

        if (!result.Succeeded)
            throw new ApplicationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        await EnsureRolesExist();

        await _userManager.AddToRoleAsync(user, dto.Role);

        var profile = new UserProfile
        {
            UserId = user.Id,
            UserName = user.UserName,
            Bio = null,
            FavoriteGenero = null,
            ProfilePicture = null
        };

        await _userProfileRepository.AddAsync(profile);

        return OperationResult<string>.Success("Usuário e perfil criados com sucesso.");
    }

    private async Task<string> GenerateJwtToken(IdentityUser<Guid> user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.Email, user.Email!),
            new (ClaimTypes.Name, user.Email!)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"], 
            claims: claims,
            expires: DateTime.Now.AddHours(4),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task EnsureRolesExist()
    {
        if (!await _roleManager.RoleExistsAsync("Admin"))
            await _roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));

        if (!await _roleManager.RoleExistsAsync("Membro"))
            await _roleManager.CreateAsync(new IdentityRole<Guid>("Membro"));
    }
}