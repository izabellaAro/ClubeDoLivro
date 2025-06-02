using Application.DTOs.Usuario;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/profile")]
[ApiController]
[Authorize]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _profileService;

    public UserProfileController(IUserProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = GetUserId();
        var result = await _profileService.GetProfileAsync(userId);

        if (!result.Succeeded)
            return NotFound(new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UserProfileDTO dto)
    {
        var userId = GetUserId();
        var result = await _profileService.UpdateProfileAsync(userId, dto);

        if (!result.Succeeded)
            return BadRequest(new { error = result.Error });

        return Ok(new { message = result.Data });
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _profileService.GetAllAsync();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _profileService.DeleteProfileAsync(id);
        if (!result.Succeeded)
            return BadRequest(new { result.Error });

        return Ok(new { Message = result.Data });
    }

    private Guid GetUserId()
    {
        var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        return Guid.Parse(userId);
    }
}