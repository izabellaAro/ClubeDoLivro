using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IBookShelfRepository : IBaseRepository<BookShelf>
{
    Task<IEnumerable<BookShelf>> GetAllBooksShelf(Guid userId);
    Task<BookShelf> ConsultBookShelfById(Guid userId, string bookId);
}