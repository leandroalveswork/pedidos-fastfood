using PedidosMvc.Domain.Model.Strategy;
using PedidosMvc.Domain.Model.ViewModel;

namespace PedidosMvc.Domain.Interfaces.Service;
public interface IServiceBase
{
    JsonResultViewModel GerarJsonResultDoErro(NegocioException nEx);
    JsonResultViewModel GerarJsonResultOk(string mensagemSucesso);
    int GetResultadosPorPaginaPadrao();
}