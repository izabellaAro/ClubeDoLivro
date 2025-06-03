using Application.DTOs.Result;
using Application.DTOs.User;
using Application.Interfaces;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IAWSS3Service _awsS3Service;

    public UserProfileService(IUserProfileRepository userProfileRepository, UserManager<IdentityUser<Guid>> userManager,
        IAWSS3Service awsS3Service)
    {
        _userProfileRepository = userProfileRepository;
        _userManager = userManager;
        _awsS3Service = awsS3Service;
    }

    public async Task<OperationResult<UserProfileResponseDTO>> GetProfileAsync(Guid userId)
    {
        var profile = await _userProfileRepository.ConsultProfile(userId);
        if (profile == null)
            return OperationResult<UserProfileResponseDTO>.Failure("Perfil não encontrado.");

        var dto = new UserProfileResponseDTO
        {
            Id = profile.Id,
            Nome = profile.UserName,
            Bio = profile.Bio,
            FavoriteGenero = profile.FavoriteGenero,
            ProfilePicture = profile.ProfilePicture
        };
            
        return OperationResult<UserProfileResponseDTO>.Success(dto);
    }

    public async Task<OperationResult<string>> UpdateProfileAsync(Guid userId, UserProfileRegisterDTO dto)
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

        if (dto.Image != null)
            await UpdatePictureProfileAsync(userId, dto.Image);

        await _userProfileRepository.UpdateAsync(profile);

        return OperationResult<string>.Success("Perfil atualizado com sucesso.");
    }

    public async Task<IEnumerable<UserProfileResponseDTO>> GetAllAsync(int skip = 0, int take = 20)
    {
        var allProfiles = await _userProfileRepository.ConsultAllProfiles(skip, take);
        
        return allProfiles.Select(profile => new UserProfileResponseDTO
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

    private async Task<OperationResult<string>> UpdatePictureProfileAsync(Guid userId, IFormFile image)
    {
        var profile = await _userProfileRepository.ConsultProfile(userId);
        if (profile is null)
            return OperationResult<string>.Failure("Perfil não encontrado.");

        var key = "medias/" + Guid.NewGuid();
        var result = await _awsS3Service.UploadImage("clube-livro", key, image);

        profile.AddImageKey(key);
        await _userProfileRepository.UpdateAsync(profile);

        return OperationResult<string>.Success("Perfil atualizado com sucesso.");
    }
}