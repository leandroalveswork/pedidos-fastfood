namespace PedidosMvc.Domain.Model.ViewModel;
public class JsonResultViewModel
{
    public bool sucesso { get; set; }
    public object? retorno { get; set; }
    public List<string> mensagens { get; set; }
}