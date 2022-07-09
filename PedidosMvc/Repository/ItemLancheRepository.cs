using System.Linq.Expressions;
using MongoDB.Driver;
using PedidosMvc.Domain.Interfaces.Configuration;
using PedidosMvc.Domain.Interfaces.Repository;
using PedidosMvc.Domain.Interfaces.ScopeData;
using PedidosMvc.Domain.Model.RepoModel;

namespace PedidosMvc.Repository;
public class ItemLancheRepository : RepositoryBase<ItemLancheRepoModel>, IItemLancheRepository
{
    public ItemLancheRepository(IMongoClientScopeData mongoClientScopeData, IDatabaseConfiguration databaseConfiguration, IIClientSessionHandleScopeData iclientSessionHandleScopeData)
        : base(mongoClientScopeData, databaseConfiguration, iclientSessionHandleScopeData)
    {
    }

    protected override string GetNomeColecEntidade()
    {
        return _databaseConfiguration.GetNomeColecItemLanches();
    }

    protected override Expression<Func<ItemLancheRepoModel, bool>> GetExpressaoFindById(string id)
    {
        return x => x.Id == id;
    }

    public async Task<List<ItemLancheRepoModel>> SelectPorLancheAsync(string idLanche)
    {
        var session = _iclientSessionHandleScopeData.GetIClientSessionHandle();
        if (session == null)
        {
            var itensLanche = await GetCollection().Find(x => x.IdLanche == idLanche).ToListAsync();
            return itensLanche;
        }
        else
        {
            var itensLanche = await GetCollection().Find(session, x => x.IdLanche == idLanche).ToListAsync();
            return itensLanche;
        }
    }
}