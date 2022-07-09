using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PedidosMvc.Domain.Interfaces.Service;
using PedidosMvc.Domain.Model.DtoModel.Lanche;
using PedidosMvc.Domain.Model.Strategy;
using PedidosMvc.Domain.Model.ViewModel;

namespace PedidosMvc.Controllers;

public class LancheController : Controller
{
    private readonly ILancheService _lancheService;

    public LancheController(ILancheService lancheService)
    {
        _lancheService = lancheService;
    }

    public async Task<IActionResult> Index()
    {
        var pesquisarDtoPadrao = _lancheService.GetPesquisarDtoPadrao();
        var resultadosPorPaginaPadrao = _lancheService.GetResultadosPorPaginaPadrao();
        var pesquisaRealizada = await _lancheService.PesquisarLanchesAsync(pesquisarDtoPadrao, resultadosPorPaginaPadrao);
        return View(pesquisaRealizada);
    }

    [HttpPost]
    public async Task<JsonResult> Pesquisar([FromBody]PesquisarLancheDtoModel pesquisarDto)
    {
        var resultadosPorPaginaPadrao = _lancheService.GetResultadosPorPaginaPadrao();
        var pesquisaRealizada = await _lancheService.PesquisarLanchesAsync(pesquisarDto, resultadosPorPaginaPadrao);
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
    public async Task<JsonResult> Incluir([FromBody]IncluirLancheDtoModel incluirDto)
    {
        try
        {
            await _lancheService.IncluirLancheAsync(incluirDto);
            return Json(_lancheService.GerarJsonResultOk(string.Format(Message.IncluidoComSucesso, "Lanche", "o")));
        }
        catch (NegocioException nEx)
        {
            return Json(_lancheService.GerarJsonResultDoErro(nEx));
        }
    }

    public async Task<IActionResult> Alterar(string? id)
    {
        var bebida = await _lancheService.GetLancheByIdAsync(id ?? string.Empty);
        return View(bebida);
    }

    [HttpPost]
    public async Task<JsonResult> Alterar([FromBody]AlterarLancheDtoModel alterarDto)
    {
        try
        {
            await _lancheService.AlterarLancheAsync(alterarDto);
            return Json(_lancheService.GerarJsonResultOk(string.Format(Message.AlteradoComSucesso, "Lanche", "o")));
        }
        catch (NegocioException nEx)
        {
            return Json(_lancheService.GerarJsonResultDoErro(nEx));
        }
    }

    [HttpPost]
    public async Task<JsonResult> Ativar(string id)
    {
        try
        {
            await _lancheService.AtivarLancheAsync(id);
            return Json(_lancheService.GerarJsonResultOk(string.Format(Message.AtivadoComSucesso, "Lanche", "o")));
        }
        catch (NegocioException nEx)
        {
            return Json(_lancheService.GerarJsonResultDoErro(nEx));
        }
    }

    [HttpPost]
    public async Task<JsonResult> Inativar(string id)
    {
        try
        {
            await _lancheService.InativarLancheAsync(id);
            return Json(_lancheService.GerarJsonResultOk(string.Format(Message.InativadoComSuceso, "Lanche", "o")));
        }
        catch (NegocioException nEx)
        {
            return Json(_lancheService.GerarJsonResultDoErro(nEx));
        }
    }

    [HttpPost]
    public async Task<JsonResult> Excluir(string id)
    {
        try
        {
            await _lancheService.ExcluirLancheAsync(id);
            return Json(_lancheService.GerarJsonResultOk(string.Format(Message.ExcluidoComSucesso, "Lanche", "o")));
        }
        catch (NegocioException nEx)
        {
            return Json(_lancheService.GerarJsonResultDoErro(nEx));
        }
    }
}