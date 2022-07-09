using PedidosMvc.Domain.Model.ViewModel.Bebida;
using PedidosMvc.Domain.Model.ViewModel.Lanche;

namespace PedidosMvc.Domain.Model.ViewModel.Pedido;
public class IncluirPedidoViewModel
{
    public List<BebidaPesquisadaViewModel> opcoesDeBebidas { get; set; }
    public List<LanchePesquisadoViewModel> opcoesDeLanches { get; set; }
}