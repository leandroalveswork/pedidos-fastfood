using PedidosMvc.Domain.Interfaces.Repository;
using PedidosMvc.Domain.Interfaces.Service;
using PedidosMvc.Domain.Model.DtoModel;
using PedidosMvc.Domain.Model.DtoModel.Lanche;
using PedidosMvc.Domain.Model.RepoModel;
using PedidosMvc.Domain.Model.Strategy;
using PedidosMvc.Domain.Model.Strategy.Factory;
using PedidosMvc.Domain.Model.ViewModel;
using PedidosMvc.Domain.Model.ViewModel.Lanche;

namespace PedidosMvc.Service;
public class LancheService : ServiceBase, ILancheService
{
    private readonly ILancheRepository _lancheRepository;
    private readonly IItemLancheRepository _itemLancheRepository;
    public LancheService(ILancheRepository lancheRepository, IItemLancheRepository itemLancheRepository)
    {
        _lancheRepository = lancheRepository;
        _itemLancheRepository = itemLancheRepository;
    }

    public async Task<PesquisaRealizadaLancheViewModel> PesquisarLanchesAsync(PesquisarLancheDtoModel pesquisarLanche, int resultadosPorPagina)
    {
        var lanches = await _lancheRepository.SelectAllAsync();
        lanches = new List<LancheRepoModel>(FilterStrategyFactory.GetStringHasFilter<LancheRepoModel>().Apply(lanches, x => x.Nome.ToUpper().RemoverAcentos(), pesquisarLanche.nome.ToUpper().RemoverAcentos()));
        lanches = new List<LancheRepoModel>(FilterStrategyFactory.GetIsGreaterEqualThanBaseFilter<LancheRepoModel>().Apply(lanches, x => x.ValorUnidadeSugerido, pesquisarLanche.valorUnidadeSugeridoMinimo));
        lanches = new List<LancheRepoModel>(FilterStrategyFactory.GetIsLessEqualThanBaseFilter<LancheRepoModel>().Apply(lanches, x => x.ValorUnidadeSugerido, pesquisarLanche.valorUnidadeSugeridoMaximo));
        lanches = new List<LancheRepoModel>(FilterStrategyFactory.GetIsExactBaseFilter<LancheRepoModel>().Apply(lanches, x => x.EstaAtivo, pesquisarLanche.estaAtivo));
        switch (pesquisarLanche.paginacaoPesquisaDto.indiceColunaAOrdenar)
        {
            case 0:
                lanches = pesquisarLanche.paginacaoPesquisaDto.ehOrdenacaoCrescente
                    ? new List<LancheRepoModel>(new BaseSorterStrategy<LancheRepoModel, string>().OrderAscending(lanches, x => x.Nome))
                    : new List<LancheRepoModel>(new BaseSorterStrategy<LancheRepoModel, string>().OrderDescending(lanches, x => x.Nome));
                break;
            case 1:
                lanches = pesquisarLanche.paginacaoPesquisaDto.ehOrdenacaoCrescente
                    ? new List<LancheRepoModel>(new BaseSorterStrategy<LancheRepoModel, double>().OrderAscending(lanches, x => x.ValorUnidadeSugerido))
                    : new List<LancheRepoModel>(new BaseSorterStrategy<LancheRepoModel, double>().OrderDescending(lanches, x => x.ValorUnidadeSugerido));
                break;
            case 2:
                lanches = pesquisarLanche.paginacaoPesquisaDto.ehOrdenacaoCrescente
                    ? new List<LancheRepoModel>(new BaseSorterStrategy<LancheRepoModel, bool>().OrderAscending(lanches, x => x.EstaAtivo))
                    : new List<LancheRepoModel>(new BaseSorterStrategy<LancheRepoModel, bool>().OrderDescending(lanches, x => x.EstaAtivo));
                break;
            default:
                break;
        }
        var lanchesPaged = new PagerStrategy().Apply(lanches, pesquisarLanche.paginacaoPesquisaDto.paginaAtual, resultadosPorPagina, out int totalPages, out int pageInRange);
        return new PesquisaRealizadaLancheViewModel()
        {
            nome = pesquisarLanche.nome,
            valorUnidadeSugeridoMinimo = pesquisarLanche.valorUnidadeSugeridoMinimo,
            valorUnidadeSugeridoMaximo = pesquisarLanche.valorUnidadeSugeridoMaximo,
            estaAtivo = pesquisarLanche.estaAtivo,
            resultados = new List<LanchePesquisadoViewModel>(lanchesPaged.Select(x => new LanchePesquisadoViewModel()
            {
                id = x.Id,
                nome = x.Nome,
                valorUnidadeSugerido = x.ValorUnidadeSugerido,
                estaAtivo = x.EstaAtivo
            })),
            paginacaoRealizada = new PaginacaoRealizadaViewModel()
            {
                paginaAtual = pageInRange,
                resultadosPorPagina = resultadosPorPagina,
                totalPaginas = totalPages,
                totalResultados = lanches.Count,
                indiceColunaAOrdenar = pesquisarLanche.paginacaoPesquisaDto.indiceColunaAOrdenar,
                ehOrdenacaoCrescente = pesquisarLanche.paginacaoPesquisaDto.ehOrdenacaoCrescente
            }
        };
    }

    public PesquisarLancheDtoModel GetPesquisarDtoPadrao()
    {
        return new PesquisarLancheDtoModel()
        {
            nome = "",
            valorUnidadeSugeridoMinimo = null,
            valorUnidadeSugeridoMaximo = null,
            estaAtivo = true,
            paginacaoPesquisaDto = new PaginacaoPesquisaDtoModel()
            {
                paginaAtual = 1,
                indiceColunaAOrdenar = 0,
                ehOrdenacaoCrescente = true
            }
        };
    }

    public async Task IncluirLancheAsync(IncluirLancheDtoModel lanche)
    {
        var cargaErros = new CargaErros();
        if (string.IsNullOrWhiteSpace(lanche.nome))
        {
            cargaErros.Acumular("Nome obrigatório");
        }
        if (lanche.valorUnidadeSugerido <= 0)
        {
            cargaErros.Acumular("O valor da Unidade é obrigatório e deve ser maior que zero.");
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        var novoLanche = new LancheRepoModel()
        {
            Id = new MongoId().Id,
            Nome = lanche.nome,
            ValorUnidadeSugerido = lanche.valorUnidadeSugerido,
            EstaAtivo = lanche.estaAtivo
        };
        await _lancheRepository.InsertAsync(novoLanche);
    }

    public async Task<DetalhesLancheViewModel> GetLancheByIdAsync(string id)
    {
        var lanche = await _lancheRepository.SelectByIdAsync(id);
        if (lanche == null)
        {
            return new DetalhesLancheViewModel()
            {
                encontrado = false
            };
        }
        return new DetalhesLancheViewModel()
        {
            encontrado = true,
            id = lanche.Id,
            nome = lanche.Nome,
            valorUnidadeSugerido = lanche.ValorUnidadeSugerido,
            estaAtivo = lanche.EstaAtivo
        };
    }

    public async Task AlterarLancheAsync(AlterarLancheDtoModel lanche)
    {
        var cargaErros = new CargaErros();
        if (!(await _lancheRepository.ExistsAsync(lanche.id)))
        {
            cargaErros.Acumular(string.Format(Message.NaoEncontrado, "Lanche", "o"));
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        if (string.IsNullOrWhiteSpace(lanche.nome))
        {
            cargaErros.Acumular("Nome obrigatório");
        }
        if (lanche.valorUnidadeSugerido <= 0)
        {
            cargaErros.Acumular("O valor da Unidade é obrigatório e deve ser maior que zero.");
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        var lancheAlterado = new LancheRepoModel()
        {
            Id = lanche.id,
            Nome = lanche.nome,
            ValorUnidadeSugerido = lanche.valorUnidadeSugerido,
            EstaAtivo = lanche.estaAtivo
        };
        await _lancheRepository.UpdateAsync(lanche.id, lancheAlterado);
    }

    public async Task AtivarLancheAsync(string id)
    {
        var lancheAlterado = await _lancheRepository.SelectByIdAsync(id);
        var cargaErros = new CargaErros();
        if (lancheAlterado == null)
        {
            cargaErros.Acumular(string.Format(Message.NaoEncontrado, "Lanche", "o"));
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        if (lancheAlterado.EstaAtivo == true)
        {
            return;
        }
        lancheAlterado.EstaAtivo = true;
        await _lancheRepository.UpdateAsync(id, lancheAlterado);
    }

    public async Task InativarLancheAsync(string id)
    {
        var lancheAlterado = await _lancheRepository.SelectByIdAsync(id);
        var cargaErros = new CargaErros();
        if (lancheAlterado == null)
        {
            cargaErros.Acumular(string.Format(Message.NaoEncontrado, "Lanche", "a"));
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        if (lancheAlterado.EstaAtivo == false)
        {
            return;
        }
        lancheAlterado.EstaAtivo = false;
        await _lancheRepository.UpdateAsync(id, lancheAlterado);
    }

    public async Task ExcluirLancheAsync(string id)
    {
        var cargaErros = new CargaErros();
        if (!(await _lancheRepository.ExistsAsync(id)))
        {
            cargaErros.Acumular(string.Format(Message.NaoEncontrado, "Lanche", "o"));
        }
        else if ((await _itemLancheRepository.SelectPorLancheAsync(id)).Count > 0)
        {
            cargaErros.Acumular("O lanche não pode ser excluído - há um pedido já associado.");
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        await _lancheRepository.DeleteAsync(id);
    }
}