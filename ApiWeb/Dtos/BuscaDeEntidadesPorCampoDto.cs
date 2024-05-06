using ApiWeb.Helpers;

namespace ApiWeb.Dtos;

public class BuscaDeEntidadesPorCampoDto
{
    public string Campo { get; set; } = string.Empty;
    public object Valor { get; set; } = null!;
    public Paginacao Paginacao { get; set; } = null!;
}