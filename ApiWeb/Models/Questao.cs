using Google.Cloud.Firestore;

namespace ApiWeb.Models;

[FirestoreData]
public class Questao : IEntidadeDoFirebase
{
    [FirestoreProperty] public string Enunciado { get; set; } = string.Empty;
    [FirestoreProperty] public List<Alternativa> Alternativas { get; set; } = [];
    [FirestoreProperty] public string Id { get; set; } = Guid.NewGuid().ToString("N");
}

[FirestoreData]
public class Alternativa : IPropriedadeComplexaDoFirebase
{
    [FirestoreProperty] public string Texto { get; set; } = string.Empty;
    [FirestoreProperty] public bool Correta { get; set; }
    [FirestoreProperty] public int Ordem { get; set; }
}