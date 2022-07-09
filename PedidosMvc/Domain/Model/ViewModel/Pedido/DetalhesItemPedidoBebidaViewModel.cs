namespace PedidosMvc.Domain.Model.ViewModel.Pedido;
public class DetalhesItemPedidoBebidaViewModel
{
    public string id { get; set; }
    public string idBebida { get; set; }
    public string nomeBebida { get; set; }
    public int volumeMl { get; set; }
    public double valorLitro { get; set; }
    public int posicaoLista { get; set; }
}