using ApiWeb.Exceptions;
using ApiWeb.Models;
using Google.Cloud.Firestore;

namespace ApiWeb.Providers;

public class FirestoreProvider(FirestoreDb db) : IFirestoreProvider
{
    public async Task CriarOuAtualizar<T>(T entidade) where T : IEntidadeDoFirebase
    {
        var doc = db.Collection(typeof(T).Name).Document(entidade.Id);
        await doc.SetAsync(entidade);
    }

    public async Task Atualizar<T>(T entidade, DocumentReference doc) where T : IEntidadeDoFirebase
    {
        await doc.SetAsync(entidade);
    }

    public async Task<DocumentReference> BuscarDocumento<T>(string id) where T : IEntidadeDoFirebase
    {
        var doc = db.Collection(typeof(T).Name).Document(id);
        var snapshot = await doc.GetSnapshotAsync();
        if (!snapshot.Exists)
        {
            throw new NaoEncontradoException($"Entidade \"{typeof(T).Name}\" não encontrada.");
        }

        return doc;
    }

    public async Task<DocumentReference> BuscarDocumento<T>(Guid id) where T : IEntidadeDoFirebase
    {
        var doc = db.Collection(typeof(T).Name).Document(id.ToString("N"));
        var snapshot = await doc.GetSnapshotAsync();
        if (!snapshot.Exists)
        {
            throw new NaoEncontradoException($"Entidade \"{typeof(T).Name}\" não encontrada.");
        }

        return doc;
    }

    public async Task<T> BuscarPorId<T>(string id) where T : IEntidadeDoFirebase
    {
        var doc = db.Collection(typeof(T).Name).Document(id);
        var snapshot = await doc.GetSnapshotAsync();
        if (!snapshot.Exists)
        {
            throw new NaoEncontradoException($"Entidade \"{typeof(T).Name}\" não encontrada.");
        }

        return snapshot.ConvertTo<T>();
    }

    public async Task<T> BuscarPorId<T>(Guid id) where T : IEntidadeDoFirebase
    {
        var doc = db.Collection(typeof(T).Name).Document(id.ToString("N"));
        var snapshot = await doc.GetSnapshotAsync();
        if (!snapshot.Exists)
        {
            throw new NaoEncontradoException($"Entidade \"{typeof(T).Name}\" não encontrada.");
        }

        return snapshot.ConvertTo<T>();
    }

    public async Task<(List<T> docs, int contagem)> ListarTodos<T>(int pagina = 1, int quantidadePorPagina = 10)
        where T : IEntidadeDoFirebase
    {
        var colecao = db.Collection(typeof(T).Name);
        var contagem = (await colecao.GetSnapshotAsync()).Count;
        if (contagem == 0)
        {
            throw new NaoEncontradoException($"Nenhuma entidade \"{typeof(T).Name}\" encontrada.");
        }

        var pontoInicial = pagina * quantidadePorPagina;
        var query = colecao.OrderBy(FieldPath.DocumentId)
            .StartAt(pontoInicial.ToString())
            .Limit(quantidadePorPagina);

        var snapshot = await query.GetSnapshotAsync();
        var docs = snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
        return (docs, contagem);
    }

    public async Task<(List<T> docs, int contagem)> ListarIguais<T>(string campo
        , object valor
        , int pagina = 1
        , int quantidadePorPagina = 10)
        where T : IEntidadeDoFirebase
    {
        var colecao = db.Collection(typeof(T).Name);
        var query = colecao.WhereEqualTo(campo, valor);
        var contagem = (await query.GetSnapshotAsync()).Count;
        if (contagem == 0)
        {
            throw new NaoEncontradoException(
                $"Nenhuma entidade \"{typeof(T).Name}\" encontrada com esses critérios de busca.");
        }

        var pontoInicial = pagina * quantidadePorPagina;
        var snapshot = await query.OrderBy(campo)
            .StartAt(pontoInicial.ToString())
            .Limit(quantidadePorPagina)
            .GetSnapshotAsync();

        var docs = snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
        return (docs, contagem);
    }

    public async Task<(List<T> docs, int contagem)> ListarSemelhantes<T>(string campo
        , string valor
        , int pagina = 1
        , int quantidadePorPagina = 10)
        where T : IEntidadeDoFirebase
    {
        var colecao = db.Collection(typeof(T).Name);
        var pontoInicial = pagina * quantidadePorPagina;
        var query = colecao.WhereGreaterThanOrEqualTo(campo, valor)
            .WhereLessThanOrEqualTo(campo, valor.Replace('%', ' ') + "\uf8ff");

        var contagem = (await query.GetSnapshotAsync()).Count;
        if (contagem == 0)
        {
            throw new NaoEncontradoException(
                $"Nenhuma entidade \"{typeof(T).Name}\" encontrada com esses critérios de busca.");
        }

        var snapshot = await query.OrderBy(campo)
            .StartAt(pontoInicial.ToString())
            .Limit(quantidadePorPagina)
            .GetSnapshotAsync();

        var docs = snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
        return (docs, contagem);
    }

    public async Task<(List<T> docs, int contagem)> ListarSemelhantesEmArray<T>(string campo
        , string valor
        , int pagina = 1
        , int quantidadePorPagina = 10)
        where T : IEntidadeDoFirebase
    {
        var colecao = db.Collection(typeof(T).Name);
        var pontoInicial = pagina * quantidadePorPagina;
        var query = colecao.WhereArrayContains(campo, valor.Replace('%', ' '));
        var contagem = (await query.GetSnapshotAsync()).Count;
        if (contagem == 0)
        {
            throw new NaoEncontradoException(
                $"Nenhuma entidade \"{typeof(T).Name}\" encontrada com esses critérios de busca.");
        }

        var snapshot = await query.OrderBy(campo)
            .StartAt(pontoInicial.ToString())
            .Limit(quantidadePorPagina)
            .GetSnapshotAsync();

        var docs = snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
        return (docs, contagem);
    }

    public async Task Excluir<T>(string id) where T : IEntidadeDoFirebase
    {
        var doc = db.Collection(typeof(T).Name).Document(id);
        await doc.DeleteAsync();
    }

    public async Task Excluir<T>(Guid id) where T : IEntidadeDoFirebase
    {
        var doc = db.Collection(typeof(T).Name).Document(id.ToString("N"));
        await doc.DeleteAsync();
    }

    public async Task Excluir(DocumentReference doc)
    {
        await doc.DeleteAsync();
    }
}