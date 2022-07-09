namespace PedidosMvc.Domain.Model.DtoModel.Pedido;
public class PesquisarPedidoDtoModel
{
    public PaginacaoPesquisaDtoModel paginacaoPesquisaDto { get; set; }
    public string nomeCliente { get; set; }
    public string nomePrimeiroItem { get; set; }
    public DateTime? diaCriacaoDesde { get; set; }
    public DateTime? diaCriacaoAte { get; set; }
    public int offsetTempoLocalMilissegundos { get; set; }
    public bool? estaEntregue { get; set; }
    public double? valorTotalMinimo { get; set; }
    public double? valorTotalMaximo { get; set; }
    public bool? estaAtivo { get; set; }
}