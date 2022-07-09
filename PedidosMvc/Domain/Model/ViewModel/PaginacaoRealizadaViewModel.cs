namespace PedidosMvc.Domain.Model.ViewModel;
public class PaginacaoRealizadaViewModel
{
    public int paginaAtual { get; set; }
    public int resultadosPorPagina { get; set; }
    public int totalPaginas { get; set; }
    public int totalResultados { get; set; }
    public int indiceColunaAOrdenar { get; set; }
    public bool ehOrdenacaoCrescente { get; set; }
}