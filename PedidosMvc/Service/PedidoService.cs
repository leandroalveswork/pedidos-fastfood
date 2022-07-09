using PedidosMvc.Domain.Interfaces.Repository;
using PedidosMvc.Domain.Interfaces.Service;
using PedidosMvc.Domain.Interfaces.UnitOfWork;
using PedidosMvc.Domain.Model.DtoModel;
using PedidosMvc.Domain.Model.DtoModel.Pedido;
using PedidosMvc.Domain.Model.RepoModel;
using PedidosMvc.Domain.Model.Strategy;
using PedidosMvc.Domain.Model.Strategy.Factory;
using PedidosMvc.Domain.Model.ViewModel;
using PedidosMvc.Domain.Model.ViewModel.Bebida;
using PedidosMvc.Domain.Model.ViewModel.Lanche;
using PedidosMvc.Domain.Model.ViewModel.Pedido;

namespace PedidosMvc.Service;
public class PedidoService : ServiceBase, IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IItemBebidaRepository _itemBebidaRepository;
    private readonly IItemLancheRepository _itemLancheRepository;
    private readonly IBebidaRepository _bebidaRepository;
    private readonly ILancheRepository _lancheRepository;
    private readonly IUnitOfWork _unitOfWork;
    public PedidoService(IPedidoRepository pedidoRepository,
                         IItemBebidaRepository itemBebidaRepository,
                         IItemLancheRepository itemLancheRepository,
                         IBebidaRepository bebidaRepository,
                         ILancheRepository lancheRepository,
                         IUnitOfWork unitOfWork)
    {
        _pedidoRepository = pedidoRepository;
        _itemBebidaRepository = itemBebidaRepository;
        _itemLancheRepository = itemLancheRepository;
        _bebidaRepository = bebidaRepository;
        _lancheRepository = lancheRepository;
        _unitOfWork = unitOfWork;
    }

    private string GetNomePrimeiroItem(PedidoRepoModel pedido, List<ItemBebidaRepoModel> itensBebida, List<ItemLancheRepoModel> itensLanche, List<BebidaRepoModel> bebidas, List<LancheRepoModel> lanches)
    {
        var primeiroItemBebida = itensBebida.FirstOrDefault(x => x.IdPedido == pedido.Id && x.PosicaoLista == 1);
        if (primeiroItemBebida != null)
        {
            return bebidas.First(x => x.Id == primeiroItemBebida.IdBebida).Nome;
        }
        else
        {
            var primeiroItemLanche = itensLanche.First(x => x.IdPedido == pedido.Id && x.PosicaoLista == 1);
            return lanches.First(x => x.Id == primeiroItemLanche.IdLanche).Nome;
        }
    }

    private double GetValorTotal(PedidoRepoModel pedido, List<ItemBebidaRepoModel> itensBebida, List<ItemLancheRepoModel> itensLanche)
    {
        double valorTotal = 0;
        foreach (var itemBebida in itensBebida.Where(x => x.IdPedido == pedido.Id))
        {
            valorTotal += ((double)(itemBebida.VolumeMl) / 1000) * itemBebida.ValorLitro;
        }
        foreach (var itemLanche in itensLanche.Where(x => x.IdPedido == pedido.Id))
        {
            valorTotal += itemLanche.Quantidade * itemLanche.ValorUnidade;
        }
        return valorTotal;
    }

    public async Task<PesquisaRealizadaPedidoViewModel> PesquisarPedidosAsync(PesquisarPedidoDtoModel pesquisarPedido, int resultadosPorPagina)
    {
        var pedidos = await _pedidoRepository.SelectAllAsync();
        var itensBebida = await _itemBebidaRepository.SelectAllAsync();
        var itensLanche = await _itemLancheRepository.SelectAllAsync();
        var bebidas = await _bebidaRepository.SelectAllAsync();
        var lanches = await _lancheRepository.SelectAllAsync();
        pedidos = new List<PedidoRepoModel>(FilterStrategyFactory.GetStringHasFilter<PedidoRepoModel>().Apply(pedidos, x => x.NomeCliente.ToUpper().RemoverAcentos(), pesquisarPedido.nomeCliente.ToUpper().RemoverAcentos()));
        pedidos = new List<PedidoRepoModel>(FilterStrategyFactory.GetStringHasFilter<PedidoRepoModel>().Apply(pedidos, x => GetNomePrimeiroItem(x, itensBebida, itensLanche, bebidas, lanches).ToUpper().RemoverAcentos(), pesquisarPedido.nomePrimeiroItem.ToUpper().RemoverAcentos()));
        pedidos = new List<PedidoRepoModel>(FilterStrategyFactory.GetIsGreaterEqualThanBaseFilter<PedidoRepoModel>().Apply(pedidos, x => x.HoraCriacao - TimeSpan.FromMilliseconds(pesquisarPedido.offsetTempoLocalMilissegundos), pesquisarPedido.diaCriacaoDesde));
        pedidos = new List<PedidoRepoModel>(FilterStrategyFactory.GetIsLessEqualThanBaseFilter<PedidoRepoModel>().Apply(pedidos, x => x.HoraCriacao - TimeSpan.FromMilliseconds(pesquisarPedido.offsetTempoLocalMilissegundos), pesquisarPedido.diaCriacaoAte + TimeSpan.FromDays(1)));
        pedidos = new List<PedidoRepoModel>(FilterStrategyFactory.GetIsExactBaseFilter<PedidoRepoModel>().Apply(pedidos, x => x.HoraEntrega != null, pesquisarPedido.estaEntregue));
        pedidos = new List<PedidoRepoModel>(FilterStrategyFactory.GetIsGreaterEqualThanBaseFilter<PedidoRepoModel>().Apply(pedidos, x => GetValorTotal(x, itensBebida, itensLanche), pesquisarPedido.valorTotalMinimo));
        pedidos = new List<PedidoRepoModel>(FilterStrategyFactory.GetIsLessEqualThanBaseFilter<PedidoRepoModel>().Apply(pedidos, x => GetValorTotal(x, itensBebida, itensLanche), pesquisarPedido.valorTotalMaximo));
        pedidos = new List<PedidoRepoModel>(FilterStrategyFactory.GetIsExactBaseFilter<PedidoRepoModel>().Apply(pedidos, x => x.EstaAtivo, pesquisarPedido.estaAtivo));
        switch (pesquisarPedido.paginacaoPesquisaDto.indiceColunaAOrdenar)
        {
            case 0:
                pedidos = pesquisarPedido.paginacaoPesquisaDto.ehOrdenacaoCrescente
                    ? new List<PedidoRepoModel>(new BaseSorterStrategy<PedidoRepoModel, string>().OrderAscending(pedidos, x => x.NomeCliente))
                    : new List<PedidoRepoModel>(new BaseSorterStrategy<PedidoRepoModel, string>().OrderDescending(pedidos, x => x.NomeCliente));
                break;
            case 1:
                pedidos = pesquisarPedido.paginacaoPesquisaDto.ehOrdenacaoCrescente
                    ? new List<PedidoRepoModel>(new BaseSorterStrategy<PedidoRepoModel, string>().OrderAscending(pedidos, x => GetNomePrimeiroItem(x, itensBebida, itensLanche, bebidas, lanches)))
                    : new List<PedidoRepoModel>(new BaseSorterStrategy<PedidoRepoModel, string>().OrderDescending(pedidos, x => GetNomePrimeiroItem(x, itensBebida, itensLanche, bebidas, lanches)));
                break;
            case 2:
                pedidos = pesquisarPedido.paginacaoPesquisaDto.ehOrdenacaoCrescente
                    ? new List<PedidoRepoModel>(new BaseSorterStrategy<PedidoRepoModel, DateTime>().OrderAscending(pedidos, x => x.HoraCriacao))
                    : new List<PedidoRepoModel>(new BaseSorterStrategy<PedidoRepoModel, DateTime>().OrderDescending(pedidos, x => x.HoraCriacao));
                break;
            case 3:
                pedidos = pesquisarPedido.paginacaoPesquisaDto.ehOrdenacaoCrescente
                    ? new List<PedidoRepoModel>(new BaseSorterStrategy<PedidoRepoModel, bool>().OrderAscending(pedidos, x => x.HoraEntrega != null))
                    : new List<PedidoRepoModel>(new BaseSorterStrategy<PedidoRepoModel, bool>().OrderDescending(pedidos, x => x.HoraEntrega != null));
                break;
            case 4:
                pedidos = pesquisarPedido.paginacaoPesquisaDto.ehOrdenacaoCrescente
                    ? new List<PedidoRepoModel>(new BaseSorterStrategy<PedidoRepoModel, DateTime?>().OrderAscending(pedidos, x => x.HoraEntrega))
                    : new List<PedidoRepoModel>(new BaseSorterStrategy<PedidoRepoModel, DateTime?>().OrderDescending(pedidos, x => x.HoraEntrega));
                break;
            case 5:
                pedidos = pesquisarPedido.paginacaoPesquisaDto.ehOrdenacaoCrescente
                    ? new List<PedidoRepoModel>(new BaseSorterStrategy<PedidoRepoModel, double>().OrderAscending(pedidos, x => GetValorTotal(x, itensBebida, itensLanche)))
                    : new List<PedidoRepoModel>(new BaseSorterStrategy<PedidoRepoModel, double>().OrderDescending(pedidos, x => GetValorTotal(x, itensBebida, itensLanche)));
                break;
            case 6:
                pedidos = pesquisarPedido.paginacaoPesquisaDto.ehOrdenacaoCrescente
                    ? new List<PedidoRepoModel>(new BaseSorterStrategy<PedidoRepoModel, bool>().OrderAscending(pedidos, x => x.EstaAtivo))
                    : new List<PedidoRepoModel>(new BaseSorterStrategy<PedidoRepoModel, bool>().OrderDescending(pedidos, x => x.EstaAtivo));
                break;
            default:
                break;
        }
        var pedidosPaged = new PagerStrategy().Apply(pedidos, pesquisarPedido.paginacaoPesquisaDto.paginaAtual, resultadosPorPagina, out int totalPages, out int pageInRange);
        return new PesquisaRealizadaPedidoViewModel()
        {
            nomeCliente = pesquisarPedido.nomeCliente,
            nomePrimeiroItem = pesquisarPedido.nomePrimeiroItem,
            diaCriacaoDesde = pesquisarPedido.diaCriacaoDesde,
            diaCriacaoAte = pesquisarPedido.diaCriacaoAte,
            estaEntregue = pesquisarPedido.estaEntregue,
            valorTotalMinimo = pesquisarPedido.valorTotalMinimo,
            valorTotalMaximo = pesquisarPedido.valorTotalMaximo,
            estaAtivo = pesquisarPedido.estaAtivo,
            resultados = new List<PedidoPesquisadoViewModel>(pedidosPaged.Select(x =>
            {
                var resultadoMapeado = new PedidoPesquisadoViewModel()
                {
                    id = x.Id,
                    nomeCliente = x.NomeCliente,
                    nomePrimeiroItem = GetNomePrimeiroItem(x, itensBebida, itensLanche, bebidas, lanches),
                    horaCriacao = x.HoraCriacao,
                    horaEntrega = x.HoraEntrega,
                    valorTotal = GetValorTotal(x, itensBebida, itensLanche),
                    estaAtivo = x.EstaAtivo,
                    itensBebida = new List<DetalhesItemPedidoBebidaViewModel>(),
                    itensLanche = new List<DetalhesItemPedidoLancheViewModel>()
                };
                foreach (var itemBebidaIt in itensBebida.Where(y => y.IdPedido == x.Id))
                {
                    resultadoMapeado.itensBebida.Add(new DetalhesItemPedidoBebidaViewModel()
                    {
                        id = itemBebidaIt.Id,
                        idBebida = itemBebidaIt.IdBebida,
                        nomeBebida = bebidas.First(y => y.Id == itemBebidaIt.IdBebida).Nome,
                        volumeMl = itemBebidaIt.VolumeMl,
                        valorLitro = itemBebidaIt.ValorLitro,
                        posicaoLista = itemBebidaIt.PosicaoLista
                    });
                }
                foreach (var itemLancheIt in itensLanche.Where(y => y.IdPedido == x.Id))
                {
                    resultadoMapeado.itensLanche.Add(new DetalhesItemPedidoLancheViewModel()
                    {
                        id = itemLancheIt.Id,
                        idLanche = itemLancheIt.IdLanche,
                        nomeLanche = lanches.First(y => y.Id == itemLancheIt.IdLanche).Nome,
                        quantidade = itemLancheIt.Quantidade,
                        valorUnidade = itemLancheIt.ValorUnidade,
                        posicaoLista = itemLancheIt.PosicaoLista
                    });
                }
                return resultadoMapeado;
            })),
            paginacaoRealizada = new PaginacaoRealizadaViewModel()
            {
                paginaAtual = pageInRange,
                resultadosPorPagina = resultadosPorPagina,
                totalPaginas = totalPages,
                totalResultados = lanches.Count,
                indiceColunaAOrdenar = pesquisarPedido.paginacaoPesquisaDto.indiceColunaAOrdenar,
                ehOrdenacaoCrescente = pesquisarPedido.paginacaoPesquisaDto.ehOrdenacaoCrescente
            }
        };
    }
    
    public PesquisarPedidoDtoModel GetPesquisarDtoPadrao()
    {
        return new PesquisarPedidoDtoModel()
        {
            nomeCliente = "",
            nomePrimeiroItem = "",
            diaCriacaoDesde = null,
            diaCriacaoAte = null,
            estaEntregue = null,
            valorTotalMinimo = null,
            valorTotalMaximo = null,
            estaAtivo = true,
            paginacaoPesquisaDto = new PaginacaoPesquisaDtoModel()
            {
                paginaAtual = 1,
                indiceColunaAOrdenar = 0,
                ehOrdenacaoCrescente = true
            }
        };
    }

    public async Task<IncluirPedidoViewModel> GetOpcoesIncluirPedidoAsync()
    {
        var bebidas = await _bebidaRepository.SelectAllAsync();
        var lanches = await _lancheRepository.SelectAllAsync();
        return new IncluirPedidoViewModel()
        {
            opcoesDeBebidas = new List<BebidaPesquisadaViewModel>(bebidas.Select(x => new BebidaPesquisadaViewModel()
            {
                id = x.Id,
                nome = x.Nome,
                valorLitroSugerido = x.ValorLitroSugerido,
                estaAtivo = x.EstaAtivo
            })),
            opcoesDeLanches = new List<LanchePesquisadoViewModel>(lanches.Select(x => new LanchePesquisadoViewModel()
            {
                id = x.Id,
                nome = x.Nome,
                valorUnidadeSugerido = x.ValorUnidadeSugerido,
                estaAtivo = x.EstaAtivo
            }))
        };
    }

    public async Task IncluirPedidoAsync(IncluirPedidoDtoModel pedido)
    {
        var cargaErros = new CargaErros();
        if (string.IsNullOrWhiteSpace(pedido.nomeCliente))
        {
            cargaErros.Acumular("Nome Cliente obrigatório");
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        var novoPedido = new PedidoRepoModel()
        {
            Id = new MongoId().Id,
            NomeCliente = pedido.nomeCliente,
            HoraCriacao = DateTime.Now,
            HoraEntrega = null,
            EstaAtivo = pedido.estaAtivo
        };
        bool posicoesOk = true;
        for (int posicaoAtual = 1; posicaoAtual <= pedido.itensBebida.Count + pedido.itensLanche.Count; posicaoAtual++)
        {
            if (!pedido.itensBebida.Any(x => x.posicaoLista == posicaoAtual) && !pedido.itensLanche.Any(x => x.posicaoLista == posicaoAtual))
            {
                posicoesOk = false;
                break;
            }
        }
        if (!posicoesOk)
        {
            int posicaoAtual = 1;
            foreach (var itemLanchePosicionado in pedido.itensLanche)
            {
                itemLanchePosicionado.posicaoLista = posicaoAtual;
                posicaoAtual++;
            }
            foreach (var itemBebidaPosicionado in pedido.itensBebida)
            {
                itemBebidaPosicionado.posicaoLista = posicaoAtual;
                posicaoAtual++;
            }
        }
        await _unitOfWork.StartTransactionAsync();
        try
        {
            foreach (var itemBebida in pedido.itensBebida)
            {
                if (!await _bebidaRepository.ExistsAsync(itemBebida.idBebida))
                {
                    cargaErros.Acumular(string.Format(Message.NaoEncontrado, "Uma das bebidas", "a"));
                    cargaErros.DescarregarEmExceptionSeCarregado();
                }
                var novoItemBebida = new ItemBebidaRepoModel()
                {
                    Id = new MongoId().Id,
                    IdPedido = novoPedido.Id,
                    IdBebida = itemBebida.idBebida,
                    VolumeMl = itemBebida.volumeMl,
                    ValorLitro = itemBebida.valorLitro,
                    PosicaoLista = itemBebida.posicaoLista
                };
                await _itemBebidaRepository.InsertAsync(novoItemBebida);
            }
            foreach (var itemLanche in pedido.itensLanche)
            {
                if (!await _lancheRepository.ExistsAsync(itemLanche.idLanche))
                {
                    cargaErros.Acumular(string.Format(Message.NaoEncontrado, "Um dos lanches", "o"));
                    cargaErros.DescarregarEmExceptionSeCarregado();
                }
                var novoItemLanche = new ItemLancheRepoModel()
                {
                    Id = new MongoId().Id,
                    IdPedido = novoPedido.Id,
                    IdLanche = itemLanche.idLanche,
                    Quantidade = itemLanche.quantidade,
                    ValorUnidade = itemLanche.valorUnidade,
                    PosicaoLista = itemLanche.posicaoLista
                };
                await _itemLancheRepository.InsertAsync(novoItemLanche);
            }
            await _pedidoRepository.InsertAsync(novoPedido);
        }
        catch (NegocioException nEx)
        {
            await _unitOfWork.RollbackAsync();
            throw nEx;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw ex;
        }
        await _unitOfWork.CommitAsync();
    }

    public async Task MarcarPedidoEntregueAsync(string id)
    {
        var pedidoAlterado = await _pedidoRepository.SelectByIdAsync(id);
        var cargaErros = new CargaErros();
        if (pedidoAlterado == null)
        {
            cargaErros.Acumular(string.Format(Message.NaoEncontrado, "Pedido", "o"));
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        if (pedidoAlterado.HoraEntrega != null)
        {
            cargaErros.Acumular("Esse pedido já foi marcado como entregue.");
        }
        pedidoAlterado.HoraEntrega = DateTime.Now;
        await _pedidoRepository.UpdateAsync(id, pedidoAlterado);
    }

    public async Task AtivarPedidoAsync(string id)
    {
        var pedidoAlterado = await _pedidoRepository.SelectByIdAsync(id);
        var cargaErros = new CargaErros();
        if (pedidoAlterado == null)
        {
            cargaErros.Acumular(string.Format(Message.NaoEncontrado, "Pedido", "o"));
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        if (pedidoAlterado.EstaAtivo == true)
        {
            return;
        }
        pedidoAlterado.EstaAtivo = true;
        await _pedidoRepository.UpdateAsync(id, pedidoAlterado);
    }

    public async Task InativarPedidoAsync(string id)
    {
        var pedidoAlterado = await _pedidoRepository.SelectByIdAsync(id);
        var cargaErros = new CargaErros();
        if (pedidoAlterado == null)
        {
            cargaErros.Acumular(string.Format(Message.NaoEncontrado, "Pedido", "o"));
        }
        cargaErros.DescarregarEmExceptionSeCarregado();
        if (pedidoAlterado.EstaAtivo == false)
        {
            return;
        }
        pedidoAlterado.EstaAtivo = false;
        await _pedidoRepository.UpdateAsync(id, pedidoAlterado);
    }
}