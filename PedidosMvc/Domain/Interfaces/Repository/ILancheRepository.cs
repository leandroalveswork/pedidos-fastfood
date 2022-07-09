using PedidosMvc.Domain.Model.RepoModel;

namespace PedidosMvc.Domain.Interfaces.Repository;
public interface ILancheRepository : IRepositoryBase<LancheRepoModel>
{
    Task<List<LancheRepoModel>> SelectPorPedidoAsync(string idPedido);
}