using ApiWeb.Config;
using ApiWeb.Exceptions;
using ApiWeb.Models;
using Google.Cloud.Firestore;

namespace ApiWeb.Repositories;

public class QuestaoRepository : IQuestaoRepository
{
    private readonly FirestoreDb _db = new FirestoreDatabase().Db;

    public async Task<(List<Questao> questoesEncontradas, int contagem)> BuscarTextoEmAlternativas(string termoDeBusca
        , int pagina
        , int quantidadePorPagina)
    {
        var colecao = _db.Collection("Questao");
        var pontoInicial = (pagina - 1) * quantidadePorPagina;
        var query = colecao.OrderBy(FieldPath.DocumentId);
        var snapshot = await query.GetSnapshotAsync();
        var docs = snapshot.Documents.Select(x => x.ConvertTo<Questao>()).ToList();
        var questoesEncontradas = new List<Questao>();
        foreach (var questao in docs)
        {
            var textosDasAlternativas = questao.Alternativas.Select(a => a.Texto);
            foreach (var texto in textosDasAlternativas)
            {
                var termoFormatado = termoDeBusca.Replace('%', ' ');
                if (texto.Contains(termoFormatado, StringComparison.OrdinalIgnoreCase))
                {
                    questoesEncontradas.Add(questao);
                }
            }
        }

        var contagem = questoesEncontradas.Count;
        if (contagem == 0)
        {
            throw new NaoEncontradoException("Nenhuma entidade \"Questao\" encontrada.");
        }

        var questoesPaginadas = questoesEncontradas.Skip(pontoInicial).Take(quantidadePorPagina).ToList();
        return (questoesPaginadas, contagem);
    }
}