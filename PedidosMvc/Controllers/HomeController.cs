using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PedidosMvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Erro()
    {
        return View();
    }
}
