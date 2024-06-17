using dotenv.net.Utilities;
using Google.Cloud.Firestore;

namespace ApiWeb.Config;

public class FirestoreDatabase
{
    public readonly FirestoreDb Db = new FirestoreDbBuilder
    {
        ProjectId = EnvReader.GetStringValue("FIRESTORE_PROJECT_ID")
        , JsonCredentials = File.ReadAllText("Properties/firestoreCredentials.json")
    }.Build();
}