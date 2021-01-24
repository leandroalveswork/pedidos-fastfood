using System.Collections.Generic;
using AspnetApi.Domain.Models;
using AspnetApi.Domain.Models.DTO;

namespace AspnetApi.Domain.Repository
{
    public interface IPedidoRepository
    {
        Pedido RecoverById(int id);

        IEnumerable<Pedido> RecoverAll();

        void Post(Pedido pedido);

        void Put(Pedido pedido);

        void Delete(int id);

        void SaveChanges();
    }
}