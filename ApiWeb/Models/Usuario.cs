using Google.Cloud.Firestore;
using System.Globalization;

namespace ApiWeb.Models;

[FirestoreData]
public class Usuario : IEntidadeDoFirebase
{
    private string _id = Guid.NewGuid().ToString("N");
    private string _nome = string.Empty;

    [FirestoreProperty]
    public string Nome
    {
        get => _nome;
        set => _nome = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
    }

    [FirestoreProperty] public string Id { get => _id; set => _id = Guid.Parse(value).ToString("N"); }
}