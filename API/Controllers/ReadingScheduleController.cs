using Application.DTOs.ReadingSchedule;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/reading-schedule")]
[ApiController]
public class ReadingScheduleController : ControllerBase
{
    private readonly IReadingScheduleService _scheduleService;

    public ReadingScheduleController(IReadingScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpPost("{bookId}")]
    public async Task<IActionResult> AddSchedule(string bookId, [FromBody] ReadingScheduleRegisterDTO dto)
    {
        var result = await _scheduleService.RegisterScheduleAsync(bookId, dto);
        if (!result.Succeeded)
            return BadRequest(new { error = result.Error });

        return Ok(new { message = result.Data });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSchedule()
    {
        return Ok(await _scheduleService.GetAllSchedules());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchedule(string id, [FromBody] ReadingScheduleUpdateDTO dto)
    {
        var result = await _scheduleService.UpdateScheduleAsync(id, dto);
        if (!result.Succeeded)
            return BadRequest(new { error = result.Error });

        return Ok(new { message = result.Data });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(string id)
    {
        var result = await _scheduleService.DeleteScheduleAsync(id);
        if (!result.Succeeded)
            return BadRequest(new { error = result.Error });

        return Ok(new { message = result.Data });
    }
}