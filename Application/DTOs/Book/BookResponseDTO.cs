namespace Application.DTOs.Book;

public class BookResponseDTO
{
    public string Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Autor { get; set; } = null!;
    public string Sinopse { get; set; } = null!;
    public string? CapaUrl { get; set; }
}