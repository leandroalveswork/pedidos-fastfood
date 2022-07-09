using System.Linq.Expressions;
using MongoDB.Driver;
using PedidosMvc.Domain.Interfaces.Configuration;
using PedidosMvc.Domain.Interfaces.Repository;
using PedidosMvc.Domain.Interfaces.ScopeData;
using PedidosMvc.Domain.Model.RepoModel;

namespace PedidosMvc.Repository;
public class ItemBebidaRepository : RepositoryBase<ItemBebidaRepoModel>, IItemBebidaRepository
{
    public ItemBebidaRepository(IMongoClientScopeData mongoClientScopeData, IDatabaseConfiguration databaseConfiguration, IIClientSessionHandleScopeData iclientSessionHandleScopeData)
        : base(mongoClientScopeData, databaseConfiguration, iclientSessionHandleScopeData)
    {
    }

    protected override string GetNomeColecEntidade()
    {
        return _databaseConfiguration.GetNomeColecItemBebidas();
    }

    protected override Expression<Func<ItemBebidaRepoModel, bool>> GetExpressaoFindById(string id)
    {
        return x => x.Id == id;
    }

    public async Task<List<ItemBebidaRepoModel>> SelectPorBebidaAsync(string idBebida)
    {
        var session = _iclientSessionHandleScopeData.GetIClientSessionHandle();
        if (session == null)
        {
            var itensBebida = await GetCollection().Find(x => x.IdBebida == idBebida).ToListAsync();
            return itensBebida;
        }
        else
        {
            var itensBebida = await GetCollection().Find(session, x => x.IdBebida == idBebida).ToListAsync();
            return itensBebida;
        }
    }
}