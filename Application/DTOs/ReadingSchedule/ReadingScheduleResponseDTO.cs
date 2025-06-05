namespace Application.DTOs.ReadingSchedule;

public class ReadingScheduleResponseDTO
{
    public string Id { get; set; }
    public string LivroId { get; set; }
    public string Livro { get; set; }
    public string Autor {  get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public bool Ativo { get; set; }
}