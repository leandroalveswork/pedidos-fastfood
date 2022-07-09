using System.Linq.Expressions;
using PedidosMvc.Domain.Interfaces.Configuration;
using PedidosMvc.Domain.Interfaces.Repository;
using PedidosMvc.Domain.Interfaces.ScopeData;
using PedidosMvc.Domain.Model.RepoModel;

namespace PedidosMvc.Repository;
public class PedidoRepository : RepositoryBase<PedidoRepoModel>, IPedidoRepository
{
    public PedidoRepository(IMongoClientScopeData mongoClientScopeData, IDatabaseConfiguration databaseConfiguration, IIClientSessionHandleScopeData iclientSessionHandleScopeData)
        : base(mongoClientScopeData, databaseConfiguration, iclientSessionHandleScopeData)
    {
    }

    protected override string GetNomeColecEntidade()
    {
        return _databaseConfiguration.GetNomeColecPedidos();
    }

    protected override Expression<Func<PedidoRepoModel, bool>> GetExpressaoFindById(string id)
    {
        return x => x.Id == id;
    }
}