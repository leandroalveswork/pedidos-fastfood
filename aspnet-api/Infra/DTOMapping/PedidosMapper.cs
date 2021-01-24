using System.Collections.Generic;
using AspnetApi.Domain.Models;
using AspnetApi.Domain.Models.DTO;

namespace AspnetApi.Infra.DTOMapping
{
    public static class PedidosMapper
    {
        public static PedidoSummary ConvertToSummary(this Pedido pedido) =>
            new PedidoSummary(pedido.Id, pedido.Posicao, pedido.Lanche, pedido.Bebida);

        public static IEnumerable<PedidoSummary> ConvertToSummary(this IEnumerable<Pedido> pedidoList)
        {
            var pedidoSummaryList = new List<PedidoSummary>();
            foreach (var pedido in pedidoList)
            {
                pedidoSummaryList.Add(new PedidoSummary(pedido.Id, pedido.Posicao, pedido.Lanche, pedido.Bebida));
            }
            return pedidoSummaryList;
        }

        public static Pedido ConvertToPedido(this PostPedido postPedido, int posicao) =>
            new Pedido(0, postPedido.SolicitanteId, posicao, postPedido.Lanche, postPedido.Bebida);
    }
}