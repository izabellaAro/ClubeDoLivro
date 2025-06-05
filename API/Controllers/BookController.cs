using Application.DTOs.Book;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        var result = await _bookService.CreateBookAsync(dto);

        if (!result.Succeeded)
            return BadRequest(new { error = result.Error });

        return Ok(new { message = result.Data });
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return Ok(await _bookService.GetAllBooksAsync(skip, take));
    }

    [HttpPut("{bookId}")]
    public async Task<IActionResult> UpdateBook(string bookId, [FromForm] BookUpdateDTO dto)
    {
        var result = await _bookService.UpdateBookAsync(bookId, dto);
        
        if (!result.Succeeded)
            return BadRequest(new { result.Error });
        return Ok(new { Message = result.Data });
    }

    [HttpDelete("{bookId}")]
    public async Task<IActionResult> DeleteBook(string bookId)
    {
        var result = await _bookService.DeleteBookAsync(bookId);
        
        if (!result.Succeeded)
            return BadRequest(new { result.Error });
        return Ok(new { Message = result.Data });
    }
}