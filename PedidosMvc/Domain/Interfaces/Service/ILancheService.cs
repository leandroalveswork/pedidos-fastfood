using PedidosMvc.Domain.Model.DtoModel.Lanche;
using PedidosMvc.Domain.Model.ViewModel.Lanche;

namespace PedidosMvc.Domain.Interfaces.Service;
public interface ILancheService : IServiceBase
{
    Task<PesquisaRealizadaLancheViewModel> PesquisarLanchesAsync(PesquisarLancheDtoModel pesquisarLanche, int resultadosPorPagina);
    PesquisarLancheDtoModel GetPesquisarDtoPadrao();
    Task IncluirLancheAsync(IncluirLancheDtoModel lanche);
    Task<DetalhesLancheViewModel> GetLancheByIdAsync(string id);
    Task AlterarLancheAsync(AlterarLancheDtoModel lanche);
    Task AtivarLancheAsync(string id);
    Task InativarLancheAsync(string id);
    Task ExcluirLancheAsync(string id);
}