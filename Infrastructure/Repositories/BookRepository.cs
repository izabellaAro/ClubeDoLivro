using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BookRepository : BaseRepository<Book>, IBookRepository
{
    public BookRepository(DataContext context) : base(context) { }

    public async Task<IEnumerable<Book>> GetAllBooks(int skip, int take)
    {
        return await _dbSet.Skip(skip).Take(take).ToListAsync();
    }

    public async Task<Book> ConsultBookById(string bookId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == bookId);
    }
}