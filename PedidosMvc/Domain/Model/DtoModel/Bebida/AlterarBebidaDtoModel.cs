namespace PedidosMvc.Domain.Model.DtoModel.Bebida;
public class AlterarBebidaDtoModel
{
    public string id { get; set; }
    public string nome { get; set; }
    public double valorLitroSugerido { get; set; }
    public bool estaAtivo { get; set; }
}