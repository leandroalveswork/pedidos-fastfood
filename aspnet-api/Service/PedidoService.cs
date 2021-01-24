using System;
using System.Collections.Generic;
using AspnetApi.Domain.Models;
using AspnetApi.Domain.Models.DTO;
using AspnetApi.Domain.Repository;
using AspnetApi.Domain.Services;
using AspnetApi.Infra.DTOMapping;

namespace AspnetApi.Service
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        public PedidoService(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public IEnumerable<PedidoSummary> RecoverAll()
        {
            return _pedidoRepository.RecoverAll().ConvertToSummary();
        }

        public IEnumerable<PedidoSummary> RecoverFilterBySolicitante(string solicitanteId)
        {
            var pedidos = _pedidoRepository.RecoverAll();
            var filteredPedidos = new List<PedidoSummary>();
            foreach (Pedido pedido in pedidos)
            {
                if (pedido.SolicitanteId == solicitanteId)
                {
                    filteredPedidos.Add(pedido.ConvertToSummary());
                }
            }
            return filteredPedidos;
        }

        public PedidoSummary Create(PostPedido pedido)
        {
            var pedidos = _pedidoRepository.RecoverAll();
            var maxValue = 0;
            foreach (Pedido pedidoItem in pedidos)
            {
                maxValue = maxValue < pedidoItem.Posicao ? pedidoItem.Posicao : maxValue;
            }
            var pedidoAddParam = pedido.ConvertToPedido(maxValue + 1);
            _pedidoRepository.Post(pedidoAddParam);
            _pedidoRepository.SaveChanges();
            return pedidoAddParam.ConvertToSummary();
        }

        public void Remove(int id)
        {
            var pedidoRemovido = _pedidoRepository.RecoverById(id);
            _pedidoRepository.Delete(id);
            var pedidos = _pedidoRepository.RecoverAll();
            foreach (Pedido pedidoItem in pedidos)
            {
                if (pedidoRemovido.Posicao < pedidoItem.Posicao)
                {
                    var pedidoShift = new Pedido(pedidoItem.Id, pedidoItem.SolicitanteId, pedidoItem.Posicao - 1, pedidoItem.Lanche, pedidoItem.Bebida);
                    _pedidoRepository.Put(pedidoShift);
                }
            }
            _pedidoRepository.SaveChanges();
        }
    }
}