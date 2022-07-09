namespace PedidosMvc.Domain.Model.ViewModel.Pedido;
public class PesquisaRealizadaPedidoViewModel
{
    public PaginacaoRealizadaViewModel paginacaoRealizada { get; set; }
    public string nomeCliente { get; set; }
    public string nomePrimeiroItem { get; set; }
    public DateTime? diaCriacaoDesde { get; set; }
    public DateTime? diaCriacaoAte { get; set; }
    public bool? estaEntregue { get; set; }
    public double? valorTotalMinimo { get; set; }
    public double? valorTotalMaximo { get; set; }
    public bool? estaAtivo { get; set; }
    public List<PedidoPesquisadoViewModel> resultados { get; set; }
}