using PedidosMvc.Domain.Interfaces.Service;
using PedidosMvc.Domain.Model.Strategy;
using PedidosMvc.Domain.Model.ViewModel;

namespace PedidosMvc.Service;
public class ServiceBase : IServiceBase
{
    public JsonResultViewModel GerarJsonResultDoErro(NegocioException nEx)
    {
        return new JsonResultViewModel()
        {
            sucesso = false,
            mensagens = nEx.CargaErros.MensagensDosErros
        };
    }

    public JsonResultViewModel GerarJsonResultOk(string mensagemSucesso)
    {
        return new JsonResultViewModel()
        {
            sucesso = true,
            mensagens = new List<string>() { mensagemSucesso }
        };
    }

    public virtual int GetResultadosPorPaginaPadrao()
    {
        return 8;
    }
}