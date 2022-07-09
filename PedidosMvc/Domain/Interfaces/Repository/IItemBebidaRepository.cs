using PedidosMvc.Domain.Model.RepoModel;

namespace PedidosMvc.Domain.Interfaces.Repository;
public interface IItemBebidaRepository : IRepositoryBase<ItemBebidaRepoModel>
{
    Task<List<ItemBebidaRepoModel>> SelectPorBebidaAsync(string idBebida);
}