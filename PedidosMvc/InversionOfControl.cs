using PedidosMvc.Configuration;
using PedidosMvc.Domain.Interfaces.Configuration;
using PedidosMvc.Domain.Interfaces.Repository;
using PedidosMvc.Domain.Interfaces.ScopeData;
using PedidosMvc.Domain.Interfaces.Service;
using PedidosMvc.Domain.Interfaces.UnitOfWork;
using PedidosMvc.Repository;
using PedidosMvc.ScopeData;
using PedidosMvc.Service;

namespace PedidosMvc;
public static class InversionOfControl
{
    public static IServiceCollection AdicionarConfiguracao(this IServiceCollection services)
    {
        services.AddSingleton<IDatabaseConfiguration, MongoDbConfiguration>();
        return services;
    }

    public static IServiceCollection AdicionarDadosEscopo(this IServiceCollection services)
    {
        services.AddScoped<IIClientSessionHandleScopeData, IClientSessionHandleScopeData>();
        services.AddScoped<IMongoClientScopeData, MongoClientScopeData>();
        return services;
    }

    public static IServiceCollection AdicionarUnidadeDeTrabalho(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        return services;
    }

    public static IServiceCollection AdicionarRepositorio(this IServiceCollection services)
    {
        services.AddScoped<IBebidaRepository, BebidaRepository>();
        services.AddScoped<ILancheRepository, LancheRepository>();
        services.AddScoped<IItemBebidaRepository, ItemBebidaRepository>();
        services.AddScoped<IItemLancheRepository, ItemLancheRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        return services;
    }

    public static IServiceCollection AdicionarServico(this IServiceCollection services)
    {
        services.AddScoped<IBebidaService, BebidaService>();
        services.AddScoped<ILancheService, LancheService>();
        services.AddScoped<IPedidoService, PedidoService>();
        return services;
    }
}