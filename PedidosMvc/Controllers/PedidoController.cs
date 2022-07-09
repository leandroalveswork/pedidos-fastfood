using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PedidosMvc.Domain.Interfaces.Service;
using PedidosMvc.Domain.Model.DtoModel.Pedido;
using PedidosMvc.Domain.Model.Strategy;
using PedidosMvc.Domain.Model.ViewModel;

namespace PedidosMvc.Controllers;

public class PedidoController : Controller
{
    private readonly IPedidoService _pedidoService;

    public PedidoController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    public async Task<IActionResult> Index()
    {
        var pesquisarDtoPadrao = _pedidoService.GetPesquisarDtoPadrao();
        var resultadosPorPaginaPadrao = _pedidoService.GetResultadosPorPaginaPadrao();
        var pesquisaRealizada = await _pedidoService.PesquisarPedidosAsync(pesquisarDtoPadrao, resultadosPorPaginaPadrao);
        return View(pesquisaRealizada);
    }

    [HttpPost]
    public async Task<JsonResult> Pesquisar([FromBody]PesquisarPedidoDtoModel pesquisarDto)
    {
        var resultadosPorPaginaPadrao = _pedidoService.GetResultadosPorPaginaPadrao();
        var pesquisaRealizada = await _pedidoService.PesquisarPedidosAsync(pesquisarDto, resultadosPorPaginaPadrao);
        return Json(new JsonResultViewModel()
        {
            sucesso = true,
            retorno = pesquisaRealizada
        });
    }

    public async Task<IActionResult> Incluir()
    {
        var opcoesDeIncluir = await _pedidoService.GetOpcoesIncluirPedidoAsync();
        return View(opcoesDeIncluir);
    }

    [HttpPost]
    public async Task<JsonResult> Incluir([FromBody]IncluirPedidoDtoModel incluirDto)
    {
        try
        {
            await _pedidoService.IncluirPedidoAsync(incluirDto);
            return Json(_pedidoService.GerarJsonResultOk(string.Format(Message.IncluidoComSucesso, "Pedido", "o")));
        }
        catch (NegocioException nEx)
        {
            return Json(_pedidoService.GerarJsonResultDoErro(nEx));
        }
    }

    [HttpPost]
    public async Task<JsonResult> MarcarEntregue(string id)
    {
        try
        {
            await _pedidoService.MarcarPedidoEntregueAsync(id);
            return Json(_pedidoService.GerarJsonResultOk("Pedido marcado como Entregue com sucesso!"));
        }
        catch (NegocioException nEx)
        {
            return Json(_pedidoService.GerarJsonResultDoErro(nEx));
        }
    }

    [HttpPost]
    public async Task<JsonResult> Ativar(string id)
    {
        try
        {
            await _pedidoService.AtivarPedidoAsync(id);
            return Json(_pedidoService.GerarJsonResultOk(string.Format(Message.AtivadoComSucesso, "Pedido", "o")));
        }
        catch (NegocioException nEx)
        {
            return Json(_pedidoService.GerarJsonResultDoErro(nEx));
        }
    }

    [HttpPost]
    public async Task<JsonResult> Inativar(string id)
    {
        try
        {
            await _pedidoService.InativarPedidoAsync(id);
            return Json(_pedidoService.GerarJsonResultOk(string.Format(Message.InativadoComSuceso, "Pedido", "o")));
        }
        catch (NegocioException nEx)
        {
            return Json(_pedidoService.GerarJsonResultDoErro(nEx));
        }
    }
}