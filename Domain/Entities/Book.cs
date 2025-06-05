namespace Domain.Entities;

public class Book
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Nome { get; set; } = null!;
    public string Autor { get; set; } = null!;
    public string Sinopse { get; set; } = null!;
    public string? CapaUrl { get; set; }

    public ICollection<BookShelf> Usuarios { get; set; } = new List<BookShelf>();

    public void AddImageKey(string key)
    {
        CapaUrl = key;
    }
}