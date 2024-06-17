using ApiWeb.Dtos;
using ApiWeb.Models;

namespace ApiWeb.Services;

public interface IQuestaoService
{
    Task<Questao> CriarQuestao(CriarQuestaoDto dto);

    Task<(List<Questao> questoesEncontradas, int contagem)> BuscarTextoEmAlternativas(string termoDeBusca
        , int pagina
        , int quantidadePorPagina);

    Task<Questao> AtualizarQuestao(Guid questaoId, AtualizarQuestaoDto dto);
}