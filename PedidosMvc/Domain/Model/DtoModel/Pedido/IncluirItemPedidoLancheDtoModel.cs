namespace PedidosMvc.Domain.Model.DtoModel.Pedido;
public class IncluirItemPedidoLancheDtoModel
{
    public string idLanche { get; set; }
    public int quantidade { get; set; }
    public double valorUnidade { get; set; }
    public int posicaoLista { get; set; }
}