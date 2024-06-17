using ApiWeb.Config;
using ApiWeb.Extensions;
using ApiWeb.Middlewares;
using ApiWeb.Providers;
using dotenv.net;

DotEnv.Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IFirestoreProvider, FirestoreProvider>(_ =>
    new FirestoreProvider(new FirestoreDatabase().Db));

builder.Services.AdicionarInjecaoDeDependencias();
builder.Services.AddControllers();
var app = builder.Build();
app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();