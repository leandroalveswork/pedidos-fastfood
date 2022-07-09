namespace PedidosMvc.Domain.Model.ViewModel.Lanche;
public class DetalhesLancheViewModel
{
    public bool encontrado { get; set; }
    public string id { get; set; }
    public string nome { get; set; }
    public double valorUnidadeSugerido { get; set; }
    public bool estaAtivo { get; set; }
}