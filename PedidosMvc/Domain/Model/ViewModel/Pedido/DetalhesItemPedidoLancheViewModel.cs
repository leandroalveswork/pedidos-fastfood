namespace PedidosMvc.Domain.Model.ViewModel.Pedido;
public class DetalhesItemPedidoLancheViewModel
{
    public string id { get; set; }
    public string idLanche { get; set; }
    public string nomeLanche { get; set; }
    public int quantidade { get; set; }
    public double valorUnidade { get; set; }
    public int posicaoLista { get; set; }
}