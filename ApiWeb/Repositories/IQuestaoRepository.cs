using ApiWeb.Models;

namespace ApiWeb.Repositories;

public interface IQuestaoRepository
{
    Task<(List<Questao> questoesEncontradas, int contagem)> BuscarTextoEmAlternativas(string termoDeBusca
        , int pagina
        , int quantidadePorPagina);
}