namespace PedidosMvc.Domain.Model.Strategy;
public class NegocioException : Exception
{
    public CargaErros CargaErros { get; set; }
    public NegocioException(CargaErros cargaErros)
    {
        CargaErros = cargaErros;
    }
}