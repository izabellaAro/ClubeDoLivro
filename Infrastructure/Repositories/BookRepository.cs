using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BookRepository : BaseRepository<Book>, IBookRepository
{
    public BookRepository(DataContext context) : base(context) { }

    public async Task<IEnumerable<Book>> GetAllBooksFromUser(Guid userId)
    {
        return _dbSet.Where(l => l.UsuarioId == userId);
    }

    public async Task<Book> ConsultBook(string bookId, Guid userId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == bookId && x.UsuarioId == userId);
    }
}