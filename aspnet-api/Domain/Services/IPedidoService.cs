using System.Collections.Generic;
using AspnetApi.Domain.Models;
using AspnetApi.Domain.Models.DTO;

namespace AspnetApi.Domain.Services
{
    public interface IPedidoService
    {
        IEnumerable<PedidoSummary> RecoverAll();
        IEnumerable<PedidoSummary> RecoverFilterBySolicitante(string SolicitanteId);

        PedidoSummary Create(PostPedido pedido);

        void Remove(int id);
    }
}