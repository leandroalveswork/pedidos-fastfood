namespace PedidosMvc.Domain.Model.DtoModel.Bebida;
public class PesquisarBebidaDtoModel
{
    public PaginacaoPesquisaDtoModel paginacaoPesquisaDto { get; set; }
    public string nome { get; set; }
    public double? valorLitroSugeridoMinimo { get; set; }
    public double? valorLitroSugeridoMaximo { get; set; }
    public bool? estaAtivo { get; set; }
}