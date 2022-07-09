namespace PedidosMvc.Domain.Model.DtoModel;
public class PaginacaoPesquisaDtoModel
{
    public int paginaAtual { get; set; }
    public int indiceColunaAOrdenar { get; set; }
    public bool ehOrdenacaoCrescente { get; set; }
}