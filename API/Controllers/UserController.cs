using Application.DTOs.Usuario;
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

    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UsuarioUpdateDTO dto)
    {
        var result = await _userService.UpdateUserAsync(dto);

        if (!result.Succeeded)
            return BadRequest(new { result.Error });

        return Ok(new { Message = "Usuário atualizado com sucesso." });
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UsuarioUpdateProfileDTO dto)
    {
        var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var result = await _userService.UpdateProfileAsync(Guid.Parse(userId), dto);

        if (!result.Succeeded)
            return BadRequest(new { result.Error });

        return Ok(new { Message = "Perfil atualizado com sucesso." });
    }
}