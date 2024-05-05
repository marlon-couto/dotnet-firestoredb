using Google.Cloud.Firestore;

namespace ApiWeb.Models;

[FirestoreData]
public class Usuario : IEntidadeDoFirebase
{
    public Usuario()
    {
    }

    public Usuario(string nome)
    {
        Nome = nome;
        Id = Guid.NewGuid().ToString("N"); // gera um GUID sem hífens 
    }

    public Usuario(Guid id, string nome)
    {
        Id = id.ToString("N");
        Nome = nome;
    }

    [FirestoreProperty] public string Nome { get; set; } = string.Empty;

    [FirestoreProperty] public string Id { get; set; } = string.Empty;
}