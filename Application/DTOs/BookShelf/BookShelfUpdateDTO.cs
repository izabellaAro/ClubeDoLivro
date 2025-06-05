using Domain.Entities;

namespace Application.DTOs.BookShelf;

public class BookShelfUpdateDTO
{
    public StatusLeitura? Status { get; set; }

    public float? Nota { get; set; }
}