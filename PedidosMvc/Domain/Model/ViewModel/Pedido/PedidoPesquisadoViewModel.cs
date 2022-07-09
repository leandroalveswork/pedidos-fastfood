namespace PedidosMvc.Domain.Model.ViewModel.Pedido;
public class PedidoPesquisadoViewModel
{
    public string id { get; set; }
    public string nomeCliente { get; set; }
    public string nomePrimeiroItem { get; set; }
    public DateTime horaCriacao { get; set; }
    public DateTime? horaEntrega { get; set; }
    public double valorTotal { get; set; }
    public bool estaAtivo { get; set; }
    public List<DetalhesItemPedidoBebidaViewModel> itensBebida { get; set; }
    public List<DetalhesItemPedidoLancheViewModel> itensLanche { get; set; }
}