using ApiWeb.Dtos;
using ApiWeb.Models;
using ApiWeb.Providers;
using ApiWeb.Repositories;

namespace ApiWeb.Services;

public class QuestaoService(IFirestoreProvider fs, IQuestaoRepository questaoRepository) : IQuestaoService
{
    public async Task<Questao> CriarQuestao(CriarQuestaoDto dto)
    {
        var questao = new Questao
        {
            Enunciado = dto.Enunciado
            , Alternativas = dto.Alternativas.Select(a =>
                new Alternativa { Correta = a.Correta, Ordem = a.Ordem, Texto = a.Texto }).ToList()
        };

        await fs.CriarOuAtualizar(questao);
        return questao;
    }

    public async Task<(List<Questao> questoesEncontradas, int contagem)> BuscarTextoEmAlternativas(string termoDeBusca
        , int pagina
        , int quantidadePorPagina)
    {
        return await questaoRepository.BuscarTextoEmAlternativas(termoDeBusca, pagina, quantidadePorPagina);
    }

    public async Task<Questao> AtualizarQuestao(Guid questaoId, AtualizarQuestaoDto dto)
    {
        var questao = await fs.BuscarPorId<Questao>(questaoId);
        var questaoAtualizada = new Questao
        {
            Enunciado = dto.Enunciado ?? questao.Enunciado
            , Id = questaoId.ToString("N")
            , Alternativas = dto.Alternativas?.Select(a =>
                new Alternativa { Correta = a.Correta, Ordem = a.Ordem, Texto = a.Texto }).ToList() ?? questao.Alternativas
        };

        await fs.CriarOuAtualizar(questaoAtualizada);
        return questaoAtualizada;
    }
}