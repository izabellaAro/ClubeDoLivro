using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class Book
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Nome { get; set; } 
    public string Autor { get; set; } 
    public string Sinopse { get; set; } 
    public string? CapaUrl { get; set; }
    public StatusLeitura Status { get; set; }
    public float? Nota { get; set; }

    public Guid UsuarioId { get; set; }
    public IdentityUser<Guid> Usuario { get; set; } = null!;

    public void AddImageKey(string key)
    {
        CapaUrl = key;
    }
}

public enum StatusLeitura
{
    Proximo = 0,
    Lendo = 1,
    Lido = 2
}