using ApiWeb.Dtos;
using ApiWeb.Helpers;
using ApiWeb.Models;
using Google.Cloud.Firestore;

namespace ApiWeb.Providers;

public interface IFirestoreProvider
{
    public Task CriarOuAtualizar<T>(T entidade, CancellationToken ct) where T : IEntidadeDoFirebase;

    public Task CriarOuAtualizar<T>(T entidade, DocumentReference doc, CancellationToken ct)
        where T : IEntidadeDoFirebase;

    public Task<DocumentReference> BuscarDocumento<T>(string id, CancellationToken ct) where T : IEntidadeDoFirebase;
    public Task<DocumentReference> BuscarDocumento<T>(Guid id, CancellationToken ct) where T : IEntidadeDoFirebase;
    public Task<T> BuscarPorId<T>(string id, CancellationToken ct) where T : IEntidadeDoFirebase;
    public Task<T> BuscarPorId<T>(Guid id, CancellationToken ct) where T : IEntidadeDoFirebase;

    public Task<IReadOnlyCollection<T>> ListarTodos<T>(Paginacao paginacao, CancellationToken ct)
        where T : IEntidadeDoFirebase;

    public Task<IReadOnlyCollection<T>> ListarIguais<T>(BuscaDeEntidadesPorCampoDto buscaDto, CancellationToken ct)
        where T : IEntidadeDoFirebase;

    public Task<IReadOnlyCollection<T>> ListarSemelhantes<T>(BuscaDeEntidadesPorCampoDto buscaDto, CancellationToken ct)
        where T : IEntidadeDoFirebase;

    public Task Excluir<T>(string id, CancellationToken ct) where T : IEntidadeDoFirebase;
    public Task Excluir<T>(Guid id, CancellationToken ct) where T : IEntidadeDoFirebase;
    public Task Excluir(DocumentReference doc, CancellationToken ct);
}