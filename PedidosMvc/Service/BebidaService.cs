using PedidosMvc.Domain.Interfaces.Repository;
using PedidosMvc.Domain.Interfaces.Service;
using PedidosMvc.Domain.Model.DtoModel;
using PedidosMvc.Domain.Model.DtoModel.Bebida;
using PedidosMvc.Domain.Model.RepoModel;
using PedidosMvc.Domain.Model.Strategy;
using PedidosMvc.Domain.Model.Strategy.Factory;
using PedidosMvc.Domain.Model.ViewModel;
using PedidosMvc.Domain.Model.ViewModel.Bebida;

namespace PedidosMvc.Service;
public class BebidaService : ServiceBase, IBebidaService
{
    private readonly IBebidaRepository _bebidaRepository;
    private readonly IItemBebidaRepository _itemBebidaRepository;
    public BebidaService(IBebidaRepository bebidaRepository, IItemBebidaRepository itemBebidaRepository)
    {
        _bebidaRepository = bebidaRepository;
        _itemBebidaRepository = itemBebidaRepository;
    }

    public async Task<PesquisaRealizadaBebidaViewModel> PesquisarBebidasAsync(PesquisarBebidaDtoModel pesquisarBebida, int resultadosPorPagina)
    {
        var bebidas = await _bebidaRepository.SelectAllAsync();
        bebidas = new List<BebidaRepoModel>(FilterStrategyFactory.GetStringHasFilter<BebidaRepoModel>().Apply(bebidas, x => x.Nome.ToUpper().RemoverAcentos(), pesquisarBebida.nome.ToUpper().RemoverAcentos()));
        bebidas = new List<BebidaRepoModel>(FilterStrategyFactory.GetIsGreaterEqualThanBaseFilter<BebidaRepoModel>().Apply(bebidas, x => x.ValorLitroSugerido, pesquisarBebida.valorLitroSugeridoMinimo));
        bebidas = new List<BebidaRepoModel>(FilterStrategyFactory.GetIsLessEqualThanBaseFilter<BebidaRepoModel>().Apply(bebidas, x => x.ValorLitroSugerido, pesquisarBebida.valorLitroSugeridoMaximo));
        bebidas = new List<BebidaRepoModel>(FilterStrategyFactory.GetIsExactBaseFilter<BebidaRepoModel>().Apply(bebidas, x => x.EstaAtivo, pesquisarBebida.estaAtivo));
        switch (pesquisarBebida.paginacaoPesquisaDto.indiceColunaAOrdenar)
        {
            case 0:
                bebidas = pesquisarBebida.paginacaoPesquisaDto.ehOrdenacaoCrescente
                    ? new List<BebidaRepoModel>(new BaseSorterStrategy<BebidaRepoModel, string>().OrderAscending(bebidas, x => x.Nome))
                    : new List<BebidaRepoModel>(new BaseSorterStrategy<BebidaRepoModel, string>().OrderDescending(bebidas, x => x.Nome));
                break;
            case 1:
                bebidas = pesquisarBebida.paginacaoPesquisaDto.ehOrdenacaoCrescente
                    ? new List<BebidaRepoModel>(new BaseSorterStrategy<BebidaRepoModel, double>().OrderAscending(bebidas, x => x.ValorLitroSugerido))
                    : new List<BebidaRepoModel>(new BaseSorterStrategy<BebidaRepoModel, double>().OrderDescending(bebidas, x => x.ValorLitroSugerido));
                break;
            case 2:
                bebidas = pesquisarBebida.paginacaoPesquisaDto.ehOrdenacaoCrescente
                    ? new List<BebidaRepoModel>(new BaseSorterStrategy<BebidaRepoModel, bool>().OrderAscending(bebidas, x => x.EstaAtivo))
                    : new List<BebidaRepoModel>(new BaseSorterStrategy<BebidaRepoModel, bool>().OrderDescending(bebidas, x => x.EstaAtivo));
                break;
            default:
                break;
        }
        var bebidasPaged = new PagerStrategy().Apply(bebidas, pesquisarBebida.paginacaoPesquisaDto.paginaAtual, resultadosPorPagina, out int totalPages, out int pageInRange);
        return new PesquisaRealizadaBebidaViewModel()
        {
            nome = pesquisarBebida.nome,
            valorLitroSugeridoMinimo = pesquisarBebida.valorLitroSugeridoMinimo,
            valorLitroSugeridoMaximo = pesquisarBebida.valorLitroSugeridoMaximo,
            estaAtivo = pesquisarBebida.estaAtivo,
            resultados = new List<BebidaPesquisadaViewModel>(bebidasPaged.Select(x => new BebidaPesquisadaViewModel()
            {
                id = x.Id,
                nome = x.Nome,
                valorLitroSugerido = x.ValorLitroSugerido,
                estaAtivo = x.EstaAtivo
            })),
            paginacaoRealizada = new PaginacaoRealizadaViewModel()
            {
                paginaAtual = pageInRange,
                resultadosPorPagina = resultadosPorPagina,
                totalPaginas = totalPages,
                totalResultados = bebidas.Count,
                indiceColunaAOrdenar = pesquisarBebida.paginacaoPesquisaDto.indiceColunaAOrdenar,
                ehOrdenacaoCrescente = pesquisarBebida.paginacaoPesquisaDto.ehOrdenacaoCrescente
            }
        };
    }

    public PesquisarBebidaDtoModel GetPesquisarDtoPadrao()
    {
        return new PesquisarBebidaDtoModel()
        {
            nome = "",
            valorLitroSugeridoMinimo = null,
            valorLitroSugeridoMaximo = null,
            estaAtivo = true,
            paginacaoPesquisaDto = new PaginacaoPesquisaDtoModel()
            {
                paginaAtual = 1,
                indiceColunaAOrdenar = 0,
                ehOrdenacaoCrescente = true
            }
        };
    }

    public async Task IncluirBebidaAsync(IncluirBebidaDtoModel bebida)
    {
        var cargaErros = new CargaErros();
        if (string.IsNullOrWhiteSpace(bebida.nome))
        {
            cargaErros.Acumular("Nome obrigatório");
        }
        if (bebida.valorLitroSugerido <= 0)
        {
            cargaErros.Acumular("O valor do Litro é obrigatório e deve ser maior que zero.");
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        var novaBebida = new BebidaRepoModel()
        {
            Id = new MongoId().Id,
            Nome = bebida.nome,
            ValorLitroSugerido = bebida.valorLitroSugerido,
            EstaAtivo = bebida.estaAtivo
        };
        await _bebidaRepository.InsertAsync(novaBebida);
    }

    public async Task<DetalhesBebidaViewModel> GetBebidaByIdAsync(string id)
    {
        var bebida = await _bebidaRepository.SelectByIdAsync(id);
        if (bebida == null)
        {
            return new DetalhesBebidaViewModel()
            {
                encontrado = false
            };
        }
        return new DetalhesBebidaViewModel()
        {
            encontrado = true,
            id = bebida.Id,
            nome = bebida.Nome,
            valorLitroSugerido = bebida.ValorLitroSugerido,
            estaAtivo = bebida.EstaAtivo
        };
    }

    public async Task AlterarBebidaAsync(AlterarBebidaDtoModel bebida)
    {
        var cargaErros = new CargaErros();
        if (!(await _bebidaRepository.ExistsAsync(bebida.id)))
        {
            cargaErros.Acumular(string.Format(Message.NaoEncontrado, "Bebida", "a"));
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        if (string.IsNullOrWhiteSpace(bebida.nome))
        {
            cargaErros.Acumular("Nome obrigatório");
        }
        if (bebida.valorLitroSugerido <= 0)
        {
            cargaErros.Acumular("O valor do Litro é obrigatório e deve ser maior que zero.");
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        var bebidaAlterada = new BebidaRepoModel()
        {
            Id = bebida.id,
            Nome = bebida.nome,
            ValorLitroSugerido = bebida.valorLitroSugerido,
            EstaAtivo = bebida.estaAtivo
        };
        await _bebidaRepository.UpdateAsync(bebida.id, bebidaAlterada);
    }

    public async Task AtivarBebidaAsync(string id)
    {
        var bebidaAlterada = await _bebidaRepository.SelectByIdAsync(id);
        var cargaErros = new CargaErros();
        if (bebidaAlterada == null)
        {
            cargaErros.Acumular(string.Format(Message.NaoEncontrado, "Bebida", "a"));
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        if (bebidaAlterada.EstaAtivo == true)
        {
            return;
        }
        bebidaAlterada.EstaAtivo = true;
        await _bebidaRepository.UpdateAsync(id, bebidaAlterada);
    }

    public async Task InativarBebidaAsync(string id)
    {
        var bebidaAlterada = await _bebidaRepository.SelectByIdAsync(id);
        var cargaErros = new CargaErros();
        if (bebidaAlterada == null)
        {
            cargaErros.Acumular(string.Format(Message.NaoEncontrado, "Bebida", "a"));
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        if (bebidaAlterada.EstaAtivo == false)
        {
            return;
        }
        bebidaAlterada.EstaAtivo = false;
        await _bebidaRepository.UpdateAsync(id, bebidaAlterada);
    }

    public async Task ExcluirBebidaAsync(string id)
    {
        var cargaErros = new CargaErros();
        if (!(await _bebidaRepository.ExistsAsync(id)))
        {
            cargaErros.Acumular(string.Format(Message.NaoEncontrado, "Bebida", "a"));
        }
        else if ((await _itemBebidaRepository.SelectPorBebidaAsync(id)).Count > 0)
        {
            cargaErros.Acumular("A bebida não pode ser excluída - há um pedido já associado.");
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        await _bebidaRepository.DeleteAsync(id);
    }
}