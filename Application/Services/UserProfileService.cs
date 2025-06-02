using Application.DTOs.Result;
using Application.DTOs.Usuario;
using Application.Interfaces;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly UserManager<IdentityUser<Guid>> _userManager;

    public UserProfileService(IUserProfileRepository userProfileRepository, UserManager<IdentityUser<Guid>> userManager)
    {
        _userProfileRepository = userProfileRepository;
        _userManager = userManager;
    }

    public async Task<OperationResult<UserProfileDTO>> GetProfileAsync(Guid userId)
    {
        var profile = await _userProfileRepository.ConsultProfile(userId);
        if (profile == null)
            return OperationResult<UserProfileDTO>.Failure("Perfil não encontrado.");

        var dto = new UserProfileDTO
        {
            Nome = profile.UserName,
            Bio = profile.Bio,
            FavoriteGenero = profile.FavoriteGenero,
            ProfilePicture = profile.ProfilePicture
        };
            
        return OperationResult<UserProfileDTO>.Success(dto);
    }

    public async Task<OperationResult<string>> UpdateProfileAsync(Guid userId, UserProfileDTO dto)
    {
        var profile = await _userProfileRepository.ConsultProfile(userId);
        if (profile == null)
            return OperationResult<string>.Failure("Perfil não encontrado.");

        if (!string.IsNullOrEmpty(dto.Nome))
            profile.UserName = dto.Nome;

        if (!string.IsNullOrEmpty(dto.Bio))
            profile.Bio = dto.Bio;

        if (!string.IsNullOrEmpty(dto.FavoriteGenero))
            profile.FavoriteGenero = dto.FavoriteGenero;

        if (!string.IsNullOrEmpty(dto.ProfilePicture))
            profile.ProfilePicture = dto.ProfilePicture;

        await _userProfileRepository.UpdateAsync(profile);

        return OperationResult<string>.Success("Perfil atualizado com sucesso.");
    }

    public async Task<IEnumerable<UserProfileDTO>> GetAllAsync(int skip = 0, int take = 20)
    {
        var allProfiles = await _userProfileRepository.ConsultAllProfiles(skip, take);
        
        return allProfiles.Select(profile => new UserProfileDTO
        {
            Id = profile.Id,
            Nome = profile.UserName,
            Bio = profile.Bio,
            FavoriteGenero = profile.FavoriteGenero,
            ProfilePicture = profile.ProfilePicture
        }).ToList();
    }

    public async Task<OperationResult<string>> DeleteProfileAsync(string userId)
    {
        var user = await _userProfileRepository.ConsultById(userId);
        if (user is null)
            return OperationResult<string>.Failure("Perfil não encontrado.");

        await _userProfileRepository.DeleteAsync(user);
   
        return OperationResult<string>.Success("Perfil excluído com sucesso.");
    }
}