using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Book;

public class BookRegisterDTO
{
    public string Nome { get; set; }
    public string Autor { get; set; }
    public string Sinopse { get; set; }
    public IFormFile? Capa { get; set; }
}