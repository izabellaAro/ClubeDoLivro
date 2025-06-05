using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReadingScheduleRepository : BaseRepository<ReadingSchedule>, IReadingScheduleRepository
{
    public ReadingScheduleRepository(DataContext context) : base(context) { }

    public async Task<IEnumerable<ReadingSchedule>> ConsultAllSchedule()
    {
        return await _dbSet.Include(a => a.Livro).OrderByDescending(a => a.DataInicio).ToListAsync();
    }

    public async Task<ReadingSchedule?> GetScheduleById(string id)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
    }
}