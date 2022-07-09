using PedidosMvc.Domain.Model.RepoModel;

namespace PedidosMvc.Domain.Interfaces.Repository;
public interface IBebidaRepository : IRepositoryBase<BebidaRepoModel>
{
    Task<List<BebidaRepoModel>> SelectPorPedidoAsync(string idPedido);
}