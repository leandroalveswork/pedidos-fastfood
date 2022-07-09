namespace PedidosMvc.Domain.Model.DtoModel.Lanche;
public class AlterarLancheDtoModel
{
    public string id { get; set; }
    public string nome { get; set; }
    public double valorUnidadeSugerido { get; set; }
    public bool estaAtivo { get; set; }
}