using Application.DTOs.ReadingSchedule;
using Application.DTOs.Result;

namespace Application.Interfaces;

public interface IReadingScheduleService
{
    Task<OperationResult<string>> RegisterScheduleAsync(string id, ReadingScheduleRegisterDTO dto);
    Task<IEnumerable<ReadingScheduleResponseDTO>> GetAllSchedules();
    Task<OperationResult<string>> UpdateScheduleAsync(string id, ReadingScheduleUpdateDTO dto);
    Task<OperationResult<string>> DeleteScheduleAsync(string id);
}