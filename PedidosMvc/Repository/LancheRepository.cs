using System.Linq.Expressions;
using MongoDB.Driver;
using PedidosMvc.Domain.Interfaces.Configuration;
using PedidosMvc.Domain.Interfaces.Repository;
using PedidosMvc.Domain.Interfaces.ScopeData;
using PedidosMvc.Domain.Model.RepoModel;

namespace PedidosMvc.Repository;
public class LancheRepository : RepositoryBase<LancheRepoModel>, ILancheRepository
{
    public LancheRepository(IMongoClientScopeData mongoClientScopeData, IDatabaseConfiguration databaseConfiguration, IIClientSessionHandleScopeData iclientSessionHandleScopeData)
        : base(mongoClientScopeData, databaseConfiguration, iclientSessionHandleScopeData)
    {
    }

    protected override string GetNomeColecEntidade()
    {
        return _databaseConfiguration.GetNomeColecLanches();
    }

    protected override Expression<Func<LancheRepoModel, bool>> GetExpressaoFindById(string id)
    {
        return x => x.Id == id;
    }

    private IMongoCollection<ItemLancheRepoModel> GetColecItemLanches()
    {
        var client = _mongoClientScopeData.GetMongoClientConnectIfNull();
        var database = client.GetDatabase(_databaseConfiguration.GetNomeBD());
        var collection = database.GetCollection<ItemLancheRepoModel>(_databaseConfiguration.GetNomeColecItemLanches());
        return collection;
    }

    public async Task<List<LancheRepoModel>> SelectPorPedidoAsync(string idPedido)
    {
        var session = _iclientSessionHandleScopeData.GetIClientSessionHandle();
        if (session == null)
        {
            var itensLanche = (await GetColecItemLanches().Find(x => x.IdPedido == idPedido).ToListAsync()).Select(x => x.IdLanche);
            var lanches = await GetCollection().Find(x => itensLanche.Contains(x.Id)).ToListAsync();
            return lanches;
        }
        else
        {
            var itensLanche = (await GetColecItemLanches().Find(x => x.IdPedido == idPedido).ToListAsync()).Select(x => x.IdLanche);
            var lanches = await GetCollection().Find(session, x => itensLanche.Contains(x.Id)).ToListAsync();
            return lanches;
        }
    }
}