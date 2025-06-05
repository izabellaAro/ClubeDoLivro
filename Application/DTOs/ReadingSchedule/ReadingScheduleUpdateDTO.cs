namespace Application.DTOs.ReadingSchedule;

public class ReadingScheduleUpdateDTO
{
    public string? LivroId { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool? Ativo { get; set; }
}