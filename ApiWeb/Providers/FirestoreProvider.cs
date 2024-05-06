using ApiWeb.Dtos;
using ApiWeb.Exceptions;
using ApiWeb.Helpers;
using ApiWeb.Models;
using Google.Cloud.Firestore;

namespace ApiWeb.Providers;

public class FirestoreProvider(FirestoreDb db)
{
    public async Task CriarOuAtualizar<T>(T entidade, CancellationToken ct) where T : IEntidadeDoFirebase
    {
        DocumentReference? doc = db.Collection(typeof(T).Name).Document(entidade.Id);
        await doc.SetAsync(entidade, cancellationToken: ct);
    }

    public async Task CriarOuAtualizar<T>(T entidade, DocumentReference doc, CancellationToken ct)
        where T : IEntidadeDoFirebase
    {
        await doc.SetAsync(entidade, cancellationToken: ct);
    }

    public async Task<DocumentReference> BuscarDocumento<T>(string id, CancellationToken ct)
        where T : IEntidadeDoFirebase
    {
        DocumentReference? doc = db.Collection(typeof(T).Name).Document(id);
        DocumentSnapshot? snapshot = await doc.GetSnapshotAsync(ct);
        if (!snapshot.Exists)
        {
            throw new NaoEncontradoException($"Entidade \"{typeof(T).Name}\" não encontrada.");
        }

        return doc;
    }

    public async Task<DocumentReference> BuscarDocumento<T>(Guid id, CancellationToken ct)
        where T : IEntidadeDoFirebase
    {
        DocumentReference? doc = db.Collection(typeof(T).Name).Document(id.ToString("N"));
        DocumentSnapshot? snapshot = await doc.GetSnapshotAsync(ct);
        if (!snapshot.Exists)
        {
            throw new NaoEncontradoException($"Entidade \"{typeof(T).Name}\" não encontrada.");
        }

        return doc;
    }

    public async Task<T> BuscarPorId<T>(string id, CancellationToken ct) where T : IEntidadeDoFirebase
    {
        DocumentReference? doc = db.Collection(typeof(T).Name).Document(id);
        DocumentSnapshot? snapshot = await doc.GetSnapshotAsync(ct);
        if (!snapshot.Exists)
        {
            throw new NaoEncontradoException($"Entidade \"{typeof(T).Name}\" não encontrada.");
        }

        return snapshot.ConvertTo<T>();
    }

    public async Task<T> BuscarPorId<T>(Guid id, CancellationToken ct) where T : IEntidadeDoFirebase
    {
        DocumentReference? doc = db.Collection(typeof(T).Name).Document(id.ToString("N"));
        DocumentSnapshot? snapshot = await doc.GetSnapshotAsync(ct);
        if (!snapshot.Exists)
        {
            throw new NaoEncontradoException($"Entidade \"{typeof(T).Name}\" não encontrada.");
        }

        return snapshot.ConvertTo<T>();
    }

    public async Task<IReadOnlyCollection<T>> ListarTodos<T>(Paginacao paginacao, CancellationToken ct)
        where T : IEntidadeDoFirebase
    {
        CollectionReference? colecao = db.Collection(typeof(T).Name);
        int pontoInicial = (paginacao.Pagina - 1) * paginacao.QuantidadePorPagina;
        Query? query = colecao.OrderBy(FieldPath.DocumentId)
            .StartAt(pontoInicial.ToString())
            .Limit(paginacao.QuantidadePorPagina);
        QuerySnapshot? snapshot = await query.GetSnapshotAsync(ct);
        if (snapshot.Count == 0)
        {
            throw new NaoEncontradoException($"Nenhuma entidade \"{typeof(T).Name}\" encontrada.");
        }

        return snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
    }

    public async Task<IReadOnlyCollection<T>> ListarIguais<T>(BuscaDeEntidadesPorCampoDto buscaDto
        , CancellationToken ct)
        where T : IEntidadeDoFirebase
    {
        CollectionReference? colecao = db.Collection(typeof(T).Name);
        int pontoInicial = (buscaDto.Paginacao.Pagina - 1) * buscaDto.Paginacao.QuantidadePorPagina;
        Query? query = colecao.WhereEqualTo(buscaDto.Campo, buscaDto.Valor)
            .OrderBy(buscaDto.Campo)
            .StartAt(pontoInicial.ToString())
            .Limit(buscaDto.Paginacao.QuantidadePorPagina);
        QuerySnapshot? snapshot = await query.GetSnapshotAsync(ct);
        if (snapshot.Count == 0)
        {
            throw new NaoEncontradoException(
                $"Nenhuma entidade \"{typeof(T).Name}\" encontrada com esses critérios de busca.");
        }

        return snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
    }

    public async Task<IReadOnlyCollection<T>> ListarSemelhantes<T>(BuscaDeEntidadesPorCampoDto buscaDto
        , CancellationToken ct)
        where T : IEntidadeDoFirebase
    {
        CollectionReference? colecao = db.Collection(typeof(T).Name);
        int pontoInicial = (buscaDto.Paginacao.Pagina - 1) * buscaDto.Paginacao.QuantidadePorPagina;
        Query? query = colecao.WhereGreaterThanOrEqualTo(buscaDto.Campo, buscaDto.Valor)
            .WhereLessThanOrEqualTo(buscaDto.Campo, buscaDto.Valor + "\uf8ff")
            .OrderBy(buscaDto.Campo)
            .StartAt(pontoInicial.ToString())
            .Limit(buscaDto.Paginacao.QuantidadePorPagina);
        QuerySnapshot? snapshot = await query.GetSnapshotAsync(ct);
        if (snapshot.Count == 0)
        {
            throw new NaoEncontradoException(
                $"Nenhuma entidade \"{typeof(T).Name}\" encontrada com esses critérios de busca.");
        }

        return snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
    }

    public async Task Excluir<T>(string id, CancellationToken ct) where T : IEntidadeDoFirebase
    {
        DocumentReference? doc = db.Collection(typeof(T).Name).Document(id);
        await doc.DeleteAsync(cancellationToken: ct);
    }

    public async Task Excluir<T>(Guid id, CancellationToken ct) where T : IEntidadeDoFirebase
    {
        DocumentReference? doc = db.Collection(typeof(T).Name).Document(id.ToString("N"));
        await doc.DeleteAsync(cancellationToken: ct);
    }

    public async Task Excluir(DocumentReference doc, CancellationToken ct)
    {
        await doc.DeleteAsync(cancellationToken: ct);
    }
}