using System.Linq.Expressions;
using MongoDB.Driver;
using PedidosMvc.Domain.Interfaces.Configuration;
using PedidosMvc.Domain.Interfaces.Repository;
using PedidosMvc.Domain.Interfaces.ScopeData;
using PedidosMvc.Domain.Model.RepoModel;

namespace PedidosMvc.Repository;
public class BebidaRepository : RepositoryBase<BebidaRepoModel>, IBebidaRepository
{
    public BebidaRepository(IMongoClientScopeData mongoClientScopeData, IDatabaseConfiguration databaseConfiguration, IIClientSessionHandleScopeData iclientSessionHandleScopeData)
        : base(mongoClientScopeData, databaseConfiguration, iclientSessionHandleScopeData)
    {
    }

    protected override string GetNomeColecEntidade()
    {
        return _databaseConfiguration.GetNomeColecBebidas();
    }

    protected override Expression<Func<BebidaRepoModel, bool>> GetExpressaoFindById(string id)
    {
        return x => x.Id == id;
    }

    private IMongoCollection<ItemBebidaRepoModel> GetColecItemBebidas()
    {
        var client = _mongoClientScopeData.GetMongoClientConnectIfNull();
        var database = client.GetDatabase(_databaseConfiguration.GetNomeBD());
        var collection = database.GetCollection<ItemBebidaRepoModel>(_databaseConfiguration.GetNomeColecItemBebidas());
        return collection;
    }

    public async Task<List<BebidaRepoModel>> SelectPorPedidoAsync(string idPedido)
    {
        var session = _iclientSessionHandleScopeData.GetIClientSessionHandle();
        if (session == null)
        {
            var itensBebida = (await GetColecItemBebidas().Find(x => x.IdPedido == idPedido).ToListAsync()).Select(x => x.IdBebida);
            var bebidas = await GetCollection().Find(x => itensBebida.Contains(x.Id)).ToListAsync();
            return bebidas;
        }
        else
        {
            var itensBebida = (await GetColecItemBebidas().Find(x => x.IdPedido == idPedido).ToListAsync()).Select(x => x.IdBebida);
            var bebidas = await GetCollection().Find(session, x => itensBebida.Contains(x.Id)).ToListAsync();
            return bebidas;
        }
    }
}