using ApiWeb.Repositories;
using ApiWeb.Services;

namespace ApiWeb.Extensions;

public static class BuilderServicesExtensions
{
    public static IServiceCollection AdicionarInjecaoDeDependencias(this IServiceCollection  servicos)
    {
        servicos.AddScoped<IUsuarioService, UsuarioService>();
        servicos.AddScoped<IQuestaoService, QuestaoService>();
        servicos.AddScoped<IQuestaoRepository, QuestaoRepository>();
        return servicos;
    }
}