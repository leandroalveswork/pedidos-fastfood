namespace PedidosMvc.Domain.Model.DtoModel.Lanche;
public class PesquisarLancheDtoModel
{
    public PaginacaoPesquisaDtoModel paginacaoPesquisaDto { get; set; }
    public string nome { get; set; }
    public double? valorUnidadeSugeridoMinimo { get; set; }
    public double? valorUnidadeSugeridoMaximo { get; set; }
    public bool? estaAtivo { get; set; }
}