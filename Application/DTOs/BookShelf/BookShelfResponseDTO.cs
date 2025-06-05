using Domain.Entities;

namespace Application.DTOs.BookShelf;

public class BookShelfResponseDTO
{
    public string LivroId { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string Autor { get; set; } = null!;
    public string Sinopse { get; set; } = null!;
    public string? CapaUrl { get; set; }

    public StatusLeitura Status { get; set; }
    public float? Nota { get; set; }
}