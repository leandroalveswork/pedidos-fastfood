using PedidosMvc.Domain.Model.DtoModel.Pedido;
using PedidosMvc.Domain.Model.ViewModel.Pedido;

namespace PedidosMvc.Domain.Interfaces.Service;
public interface IPedidoService : IServiceBase
{
    Task<PesquisaRealizadaPedidoViewModel> PesquisarPedidosAsync(PesquisarPedidoDtoModel pesquisarPedido, int resultadosPorPagina);
    PesquisarPedidoDtoModel GetPesquisarDtoPadrao();
    Task<IncluirPedidoViewModel> GetOpcoesIncluirPedidoAsync();
    Task IncluirPedidoAsync(IncluirPedidoDtoModel pedido);
    Task MarcarPedidoEntregueAsync(string id);
    Task AtivarPedidoAsync(string id);
    Task InativarPedidoAsync(string id);
}