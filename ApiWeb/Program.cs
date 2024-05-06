using ApiWeb.Middlewares;
using ApiWeb.Providers;
using dotenv.net;
using dotenv.net.Utilities;
using Google.Cloud.Firestore;

DotEnv.Load();
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IFirestoreProvider>(_ => new FirestoreProvider(new FirestoreDbBuilder
{
    ProjectId = EnvReader.GetStringValue("FIRESTORE_PROJECT_ID")
    , JsonCredentials = File.ReadAllText("Properties/firestoreCredentials.json")
}.Build()));
builder.Services.AddControllers();
WebApplication app = builder.Build();
app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();