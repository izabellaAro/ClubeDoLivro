using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class BookShelf
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string LivroId { get; set; } = null!;
    public Book Livro { get; set; } = null!;

    public Guid UsuarioId { get; set; }
    public IdentityUser<Guid> Usuario { get; set; } = null!;

    public StatusLeitura Status { get; set; }
    public float? Nota { get; set; }
}

public enum StatusLeitura
{
    Proximo = 0,
    Lendo = 1,
    Lido = 2
}