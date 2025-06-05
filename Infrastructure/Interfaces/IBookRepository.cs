using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IBookRepository : IBaseRepository<Book>
{
    Task<IEnumerable<Book>> GetAllBooks(int skip, int take);
    Task<Book> ConsultBookById(string bookId);
}