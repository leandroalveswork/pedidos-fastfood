namespace PedidosMvc.Domain.Model.ViewModel.Lanche;
public class PesquisaRealizadaLancheViewModel
{
    public PaginacaoRealizadaViewModel paginacaoRealizada { get; set; }
    public string nome { get; set; }
    public double? valorUnidadeSugeridoMinimo { get; set; }
    public double? valorUnidadeSugeridoMaximo { get; set; }
    public bool? estaAtivo { get; set; }
    public List<LanchePesquisadoViewModel> resultados { get; set; }
}