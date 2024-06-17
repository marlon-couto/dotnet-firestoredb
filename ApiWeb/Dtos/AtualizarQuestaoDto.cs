namespace ApiWeb.Dtos;

public class AtualizarQuestaoDto
{
    public string? Enunciado { get; set; } = null;
    public List<AlternativaDto>? Alternativas { get; set; } = null;
}