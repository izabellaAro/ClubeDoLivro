using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IBookRepository : IBaseRepository<Book>
{
    Task<IEnumerable<Book>> GetAllBooksFromUser(Guid userId);
    Task<Book> ConsultBook(string bookId, Guid userId);
}