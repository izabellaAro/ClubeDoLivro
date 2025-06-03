using Application.DTOs.User;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        if (!result.Succeeded)
            return BadRequest(new { result.Error });

        return Ok(result.Data);
    }

    [Authorize]
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllUsersAsync();
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO dto)
    {
        var result = await _userService.UpdateUserAsync(dto);

        if (!result.Succeeded)
            return BadRequest(new { result.Error });

        return Ok(new { Message = "Usuário atualizado com sucesso." });
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UserUpdateCredentialsDTO dto)
    {
        var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var result = await _userService.UpdateProfileAsync(Guid.Parse(userId), dto);

        if (!result.Succeeded)
            return BadRequest(new { result.Error });

        return Ok(new { Message = "Perfil atualizado com sucesso." });
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result.Succeeded)
            return BadRequest(new { result.Error });

        return Ok(new { Message = result.Data });
    }
}