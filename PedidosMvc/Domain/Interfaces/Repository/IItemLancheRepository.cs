using PedidosMvc.Domain.Model.RepoModel;

namespace PedidosMvc.Domain.Interfaces.Repository;
public interface IItemLancheRepository : IRepositoryBase<ItemLancheRepoModel>
{
    Task<List<ItemLancheRepoModel>> SelectPorLancheAsync(string idLanche);
}