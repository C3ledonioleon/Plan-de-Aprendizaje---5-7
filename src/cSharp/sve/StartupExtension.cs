using sve.Repositories;
using sve.Repositories.Contracts;
using sve.Services;
using sve.Services.Contracts;

public static class StartupExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IEntradaService, EntradaService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        
        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IClienteRepository, ClienteRepository>();

        services.AddTransient<IEntradaRepository, EntradaRepository>();

        services.AddTransient<IEventoRepository, EventoRepository>();

        services.AddTransient<IFuncionRepository, FuncionRepository>();

        services.AddTransient<ILocalRepository, LocalRepository>();

        services.AddTransient<IOrdenRepository, OrdenRepository>();

        services.AddTransient<ISectorRepository, SectorRepository>();

        services.AddTransient<ITarifaRepository, TarifaRepository>();

        services.AddTransient<IUsuarioRepository, UsuarioRepository>();
        

        return services;
    }

}