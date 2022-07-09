namespace PedidosMvc.Domain.Model.DtoModel.Pedido;
public class IncluirPedidoDtoModel
{
    public string nomeCliente { get; set; }
    public List<IncluirItemPedidoBebidaDtoModel> itensBebida { get; set; }
    public List<IncluirItemPedidoLancheDtoModel> itensLanche { get; set; }
    public bool estaAtivo { get; set; }
}