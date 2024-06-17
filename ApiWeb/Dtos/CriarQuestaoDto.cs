namespace ApiWeb.Dtos;

public class CriarQuestaoDto
{
    public string Enunciado { get; set; } = string.Empty;
    public List<AlternativaDto> Alternativas { get; set; } = [];
}

public class AlternativaDto
{
    public string Texto { get; set; } = string.Empty;
    public bool Correta { get; set; }
    public int Ordem { get; set; }
}