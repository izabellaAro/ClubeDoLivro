using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BookShelfRepository : BaseRepository<BookShelf>, IBookShelfRepository
{
    public BookShelfRepository(DataContext context) : base(context) { }

    public async Task<IEnumerable<BookShelf>> GetAllBooksShelf(Guid userId)
    {
        return _dbSet.Include(e => e.Livro).Where(e => e.UsuarioId == userId);
    }

    public async Task<BookShelf> ConsultBookShelfById(Guid userId, string bookId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.UsuarioId == userId && x.LivroId == bookId);
    }
}