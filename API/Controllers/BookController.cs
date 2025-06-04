using Application.DTOs.Book;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/books")]
[ApiController]
[Authorize]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterBook([FromForm] BookRegisterDTO dto)
    {
        var userId = GetUserId();
        var result = await _bookService.CreateBookAsync(userId, dto);

        if (!result.Succeeded)
            return BadRequest(new { error = result.Error });

        return Ok(new { message = result.Data });
    }

    [HttpGet]
    public async Task<IActionResult> GetBooksFromUser()
    {
        var userId = GetUserId();
        return Ok(await _bookService.GetAllBooksAsync(userId));
    }

    [HttpPut("{bookId}")]
    public async Task<IActionResult> UpdateBook(string bookId, [FromForm] BookUpdateDTO dto)
    {
        var userId = GetUserId();
        var result = await _bookService.UpdateBookAsync(userId, bookId, dto);
        
        if (!result.Succeeded)
            return BadRequest(new { result.Error });
        return Ok(new { Message = result.Data });
    }

    [HttpDelete("{bookId}")]
    public async Task<IActionResult> DeleteBook(string bookId)
    {
        var userId = GetUserId();
        var result = await _bookService.DeleteBookAsync(userId, bookId);
        
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