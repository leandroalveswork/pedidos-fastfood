using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PedidosMvc.Domain.Interfaces.Service;
using PedidosMvc.Domain.Model.DtoModel.Bebida;
using PedidosMvc.Domain.Model.Strategy;
using PedidosMvc.Domain.Model.ViewModel;

namespace PedidosMvc.Controllers;

public class BebidaController : Controller
{
    private readonly IBebidaService _bebidaService;

    public BebidaController(IBebidaService bebidaService)
    {
        _bebidaService = bebidaService;
    }

    public async Task<IActionResult> Index()
    {
        var pesquisarDtoPadrao = _bebidaService.GetPesquisarDtoPadrao();
        var resultadosPorPaginaPadrao = _bebidaService.GetResultadosPorPaginaPadrao();
        var pesquisaRealizada = await _bebidaService.PesquisarBebidasAsync(pesquisarDtoPadrao, resultadosPorPaginaPadrao);
        return View(pesquisaRealizada);
    }

    [HttpPost]
    public async Task<JsonResult> Pesquisar([FromBody]PesquisarBebidaDtoModel pesquisarDto)
    {
        var resultadosPorPaginaPadrao = _bebidaService.GetResultadosPorPaginaPadrao();
        var pesquisaRealizada = await _bebidaService.PesquisarBebidasAsync(pesquisarDto, resultadosPorPaginaPadrao);
        return Json(new JsonResultViewModel()
        {
            sucesso = true,
            retorno = pesquisaRealizada
        });
    }

    public IActionResult Incluir()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> Incluir([FromBody]IncluirBebidaDtoModel incluirDto)
    {
        try
        {
            await _bebidaService.IncluirBebidaAsync(incluirDto);
            return Json(_bebidaService.GerarJsonResultOk(string.Format(Message.IncluidoComSucesso, "Bebida", "a")));
        }
        catch (NegocioException nEx)
        {
            return Json(_bebidaService.GerarJsonResultDoErro(nEx));
        }
    }

    public async Task<IActionResult> Alterar(string? id)
    {
        var bebida = await _bebidaService.GetBebidaByIdAsync(id ?? string.Empty);
        return View(bebida);
    }

    [HttpPost]
    public async Task<JsonResult> Alterar([FromBody]AlterarBebidaDtoModel alterarDto)
    {
        try
        {
            await _bebidaService.AlterarBebidaAsync(alterarDto);
            return Json(_bebidaService.GerarJsonResultOk(string.Format(Message.AlteradoComSucesso, "Bebida", "a")));
        }
        catch (NegocioException nEx)
        {
            return Json(_bebidaService.GerarJsonResultDoErro(nEx));
        }
    }

    [HttpPost]
    public async Task<JsonResult> Ativar(string id)
    {
        try
        {
            await _bebidaService.AtivarBebidaAsync(id);
            return Json(_bebidaService.GerarJsonResultOk(string.Format(Message.AtivadoComSucesso, "Bebida", "a")));
        }
        catch (NegocioException nEx)
        {
            return Json(_bebidaService.GerarJsonResultDoErro(nEx));
        }
    }

    [HttpPost]
    public async Task<JsonResult> Inativar(string id)
    {
        try
        {
            await _bebidaService.InativarBebidaAsync(id);
            return Json(_bebidaService.GerarJsonResultOk(string.Format(Message.InativadoComSuceso, "Bebida", "a")));
        }
        catch (NegocioException nEx)
        {
            return Json(_bebidaService.GerarJsonResultDoErro(nEx));
        }
    }

    [HttpPost]
    public async Task<JsonResult> Excluir(string id)
    {
        try
        {
            await _bebidaService.ExcluirBebidaAsync(id);
            return Json(_bebidaService.GerarJsonResultOk(string.Format(Message.ExcluidoComSucesso, "Bebida", "a")));
        }
        catch (NegocioException nEx)
        {
            return Json(_bebidaService.GerarJsonResultDoErro(nEx));
        }
    }
}