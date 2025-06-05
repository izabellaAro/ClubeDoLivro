namespace Domain.Entities;

public class ReadingSchedule
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string LivroId { get; set; } = null!;
    public Book Livro { get; set; } = null!;

    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }

    public bool Ativo { get; set; } 
}