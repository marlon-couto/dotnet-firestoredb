using ApiWeb.Models;
using Google.Cloud.Firestore;

namespace ApiWeb.Providers;

public class FirestoreProvider(FirestoreDb db)
{
    public async Task CriarOuAtualizar<T>(T entidade, CancellationToken ct) where T : IEntidadeDoFirebase
    {
        DocumentReference? documento = db.Collection(typeof(T).Name).Document(entidade.Id);
        await documento.SetAsync(entidade, cancellationToken: ct);
    }

    public async Task<T> BuscarPorId<T>(string id, CancellationToken ct) where T : IEntidadeDoFirebase
    {
        DocumentReference? documento = db.Collection(typeof(T).Name).Document(id);
        DocumentSnapshot? snapshot = await documento.GetSnapshotAsync(ct);
        return snapshot.ConvertTo<T>();
    }

    public async Task<T> BuscarPorId<T>(Guid id, CancellationToken ct) where T : IEntidadeDoFirebase
    {
        DocumentReference? documento = db.Collection(typeof(T).Name).Document(id.ToString("N"));
        DocumentSnapshot? snapshot = await documento.GetSnapshotAsync(ct);
        return snapshot.ConvertTo<T>();
    }

    public async Task<IReadOnlyCollection<T>> ListarTodos<T>(int pagina, int quantidade, CancellationToken ct)
        where T : IEntidadeDoFirebase
    {
        CollectionReference? colecao = db.Collection(typeof(T).Name);
        int pontoInicial = (pagina - 1) * quantidade;
        Query? query = colecao.OrderBy(FieldPath.DocumentId).StartAt(pontoInicial).Limit(quantidade);
        QuerySnapshot? snapshot = await query.GetSnapshotAsync(ct);
        return snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
    }

    public async Task<IReadOnlyCollection<T>> ListarIguaisA<T>(string campo, object valor, CancellationToken ct)
        where T : IEntidadeDoFirebase
    {
        Query? query = db.Collection(typeof(T).Name).WhereEqualTo(campo, valor);
        QuerySnapshot? snapshot = await query.GetSnapshotAsync(ct);
        return snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
    }

    public async Task Excluir<T>(string id, CancellationToken ct) where T : IEntidadeDoFirebase
    {
        DocumentReference? documento = db.Collection(typeof(T).Name).Document(id);
        await documento.DeleteAsync(cancellationToken: ct);
    }

    public async Task Excluir<T>(Guid id, CancellationToken ct) where T : IEntidadeDoFirebase
    {
        DocumentReference? documento = db.Collection(typeof(T).Name).Document(id.ToString("N"));
        await documento.DeleteAsync(cancellationToken: ct);
    }
}