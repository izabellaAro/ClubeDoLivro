using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IReadingScheduleRepository : IBaseRepository<ReadingSchedule>
{
    Task<IEnumerable<ReadingSchedule>> ConsultAllSchedule();
    Task<ReadingSchedule?> GetScheduleById(string id);
}