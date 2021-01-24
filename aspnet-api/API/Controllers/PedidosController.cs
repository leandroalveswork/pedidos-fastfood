using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspnetApi.Domain.Services;
using AspnetApi.Domain.Models.DTO;

namespace AspnetApi.API.Controllers
{
    [Route("api/pedidos")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_pedidoService.RecoverAll());
        }

        [HttpGet("{solicitanteId}")]
        public IActionResult GetFiltered(string solicitanteId)
        {
            return Ok(_pedidoService.RecoverFilterBySolicitante(solicitanteId));
        }

        [HttpPost]
        public IActionResult Post([FromBody] PostPedido pedido)
        {
            return Ok(_pedidoService.Create(pedido));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _pedidoService.Remove(id);
            return Ok();
        }
    }
}
