using PedidosMvc.Domain.Model.DtoModel.Bebida;
using PedidosMvc.Domain.Model.ViewModel.Bebida;

namespace PedidosMvc.Domain.Interfaces.Service;
public interface IBebidaService : IServiceBase
{
    Task<PesquisaRealizadaBebidaViewModel> PesquisarBebidasAsync(PesquisarBebidaDtoModel pesquisarBebida, int resultadosPorPagina);
    PesquisarBebidaDtoModel GetPesquisarDtoPadrao();
    Task IncluirBebidaAsync(IncluirBebidaDtoModel bebida);
    Task<DetalhesBebidaViewModel> GetBebidaByIdAsync(string id);
    Task AlterarBebidaAsync(AlterarBebidaDtoModel bebida);
    Task AtivarBebidaAsync(string id);
    Task InativarBebidaAsync(string id);
    Task ExcluirBebidaAsync(string id);
}