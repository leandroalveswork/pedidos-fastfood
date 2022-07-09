namespace PedidosMvc.Domain.Model.Strategy;
public class CargaErros
{
    public CargaErros()
    {
        MensagensDosErros = new List<string>();
    }
    public List<string> MensagensDosErros { get; set; }
    public void Acumular(string MensagemDoErro)
    {
        MensagensDosErros.Add(MensagemDoErro);
    }
    public void DescarregarEmExceptionSeCarregado()
    {
        if (MensagensDosErros.Count > 0)
        {
            throw new NegocioException(this);
        }
    }
}