using Application.DTOs.BookShelf;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/books-shelf")]
[ApiController]
[Authorize]
public class BookShelfController : ControllerBase
{
    private readonly IBookShelfService _bookService;

    public BookShelfController(IBookShelfService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost("{bookId}")]
    public async Task<IActionResult> RegisterBookShelf(string bookId, [FromBody] BookShelfRegisterDTO dto)
    {
        var userId = GetUserId();

        var result = await _bookService.AddBookToShelfAsync(userId, bookId, dto);
        if (!result.Succeeded)
            return BadRequest(new { error = result.Error });

        return Ok(new { message = result.Data });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var userId = GetUserId();
        return Ok(await _bookService.GetAllBooksShelfAsync(userId));
    }

    [HttpPut("{bookId}")]
    public async Task<IActionResult> UpdateBook([FromBody] BookShelfUpdateDTO dto, string bookId)
    {
        var userId = GetUserId();

        var result = await _bookService.UpdateBookShelfAsync(userId, bookId, dto);
        if (!result.Succeeded)
            return BadRequest(new { error = result.Error });

        return Ok(new { message = result.Data });
    }

    [HttpDelete("{bookId}")]
    public async Task<IActionResult> DeleteBook(string bookId)
    {
        var userId = GetUserId();

        var result = await _bookService.DeleteBookToShelfAsync(userId, bookId);
        if (!result.Succeeded)
            return BadRequest(new { error = result.Error });

        return Ok(new { message = result.Data });
    }

    private Guid GetUserId()
    {
        var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        return Guid.Parse(userId);
    }
}
