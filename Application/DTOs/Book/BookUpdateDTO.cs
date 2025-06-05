using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Book;

public class BookUpdateDTO
{
    public string? Nome { get; set; } = null!;
    public string? Autor { get; set; } = null!;
    public string? Sinopse { get; set; } = null!;
    public IFormFile? CapaUrl { get; set; }
}