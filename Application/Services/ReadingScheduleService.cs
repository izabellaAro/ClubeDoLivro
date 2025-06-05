using Application.DTOs.ReadingSchedule;
using Application.DTOs.Result;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;

public class ReadingScheduleService : IReadingScheduleService
{
    private readonly IBookRepository _bookRepository;
    private readonly IReadingScheduleRepository _scheduleRepository;

    public ReadingScheduleService(IBookRepository bookRepository, IReadingScheduleRepository scheduleRepository)
    {
        _bookRepository = bookRepository;
        _scheduleRepository = scheduleRepository;
    }

    public async Task<OperationResult<string>> RegisterScheduleAsync(string id, ReadingScheduleRegisterDTO dto)
    {
        var book = await _bookRepository.ConsultBookById(id);

        if (book == null)
            return OperationResult<string>.Failure("Livro não encontrado.");

        var agenda = new ReadingSchedule
        {
            LivroId = id,
            DataInicio = dto.DataInicio,
            DataFim = dto.DataFim,
            Ativo = true
        };

        await _scheduleRepository.AddAsync(agenda);
        
        return OperationResult<string>.Success("Agenda de leitura criada com sucesso.");
    }

    public async Task<IEnumerable<ReadingScheduleResponseDTO>> GetAllSchedules()
    {
        var agendas = await _scheduleRepository.ConsultAllSchedule();

        return agendas.Select(x => new ReadingScheduleResponseDTO
        {
            Id = x.Id,
            LivroId = x.LivroId,
            Livro = x.Livro.Nome,
            Autor = x.Livro.Autor,
            DataInicio = x.DataInicio,
            DataFim = x.DataFim,
            Ativo = x.Ativo
        }).ToList();
    }

    public async Task<OperationResult<string>> UpdateScheduleAsync(string id, ReadingScheduleUpdateDTO dto)
    {
        var schedule = await _scheduleRepository.GetScheduleById(id);
        if (schedule == null)
            return OperationResult<string>.Failure("Agenda de leitura não encontrada.");

        if (!string.IsNullOrEmpty(dto.LivroId))
        {
            var book = await _bookRepository.ConsultBookById(dto.LivroId);
            if (book == null)
                return OperationResult<string>.Failure("Livro não encontrado.");
            schedule.LivroId = dto.LivroId;
        }

        schedule.DataInicio = dto.DataInicio ?? schedule.DataInicio;
        schedule.DataFim = dto.DataFim ?? schedule.DataFim;

        if (dto.Ativo.HasValue)
        {
            schedule.Ativo = dto.Ativo.Value;
        }

        await _scheduleRepository.UpdateAsync(schedule);

        return OperationResult<string>.Success("Agenda de leitura atualizada com sucesso.");
    }

    public async Task<OperationResult<string>> DeleteScheduleAsync(string id)
    {
        var schedule = await _scheduleRepository.GetScheduleById(id);

        if (schedule == null)
            return OperationResult<string>.Failure("Agenda de leitura não encontrada.");

        await _scheduleRepository.DeleteAsync(schedule);

        return OperationResult<string>.Success("Agenda de leitura excluída com sucesso.");
    }
}