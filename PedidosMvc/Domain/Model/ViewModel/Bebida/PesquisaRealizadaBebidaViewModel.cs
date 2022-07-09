namespace PedidosMvc.Domain.Model.ViewModel.Bebida;
public class PesquisaRealizadaBebidaViewModel
{
    public PaginacaoRealizadaViewModel paginacaoRealizada { get; set; }
    public string nome { get; set; }
    public double? valorLitroSugeridoMinimo { get; set; }
    public double? valorLitroSugeridoMaximo { get; set; }
    public bool? estaAtivo { get; set; }
    public List<BebidaPesquisadaViewModel> resultados { get; set; }
}