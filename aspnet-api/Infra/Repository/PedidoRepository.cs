using System.Collections.Generic;
using System.Linq;
using AspnetApi.Domain.Models;
using AspnetApi.Domain.Repository;
using AspnetApi.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace AspnetApi.Infra.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly FastFoodContext _context;
        public PedidoRepository(FastFoodContext context)
        {
            _context = context;
        }

        public Pedido RecoverById(int id)
        {
            return _context.Pedidos.Find(id);
        }

        public IEnumerable<Pedido> RecoverAll()
        {
            return _context.Pedidos.ToList();
        }

        public void Post(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
        }
        public void Put(Pedido pedido)
        {
            var local = _context.Set<Pedido>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(pedido.Id));

            // check if local is not null 
            if (local != null)
            {
                // detach
                _context.Entry(local).State = EntityState.Detached;
            }
            // set Modified flag in your entry
            _context.Entry(pedido).State = EntityState.Modified;

            
        }
        public void Delete(int id)
        {
            _context.Pedidos.Remove(RecoverById(id));
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}