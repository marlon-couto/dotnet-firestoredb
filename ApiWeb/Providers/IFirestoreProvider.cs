using ApiWeb.Dtos;
using ApiWeb.Helpers;
using ApiWeb.Models;
using Google.Cloud.Firestore;

namespace ApiWeb.Providers;

public interface IFirestoreProvider
{
    public Task CriarOuAtualizar<T>(T entidade) where T : IEntidadeDoFirebase;
    public Task CriarOuAtualizar<T>(T entidade, DocumentReference doc) where T : IEntidadeDoFirebase;
    public Task<DocumentReference> BuscarDocumento<T>(string id) where T : IEntidadeDoFirebase;
    public Task<DocumentReference> BuscarDocumento<T>(Guid id) where T : IEntidadeDoFirebase;
    public Task<T> BuscarPorId<T>(string id) where T : IEntidadeDoFirebase;
    public Task<T> BuscarPorId<T>(Guid id) where T : IEntidadeDoFirebase;
    public Task<IReadOnlyCollection<T>> ListarTodos<T>(Paginacao paginacao) where T : IEntidadeDoFirebase;

    public Task<IReadOnlyCollection<T>> ListarIguais<T>(BuscaDeEntidadesPorCampoDto buscaDto)
        where T : IEntidadeDoFirebase;

    public Task<IReadOnlyCollection<T>> ListarSemelhantes<T>(BuscaDeEntidadesPorCampoDto buscaDto)
        where T : IEntidadeDoFirebase;

    public Task Excluir<T>(string id) where T : IEntidadeDoFirebase;
    public Task Excluir<T>(Guid id) where T : IEntidadeDoFirebase;
    public Task Excluir(DocumentReference doc);
}