using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contracts;
using DOMAIN;

namespace DAL.Contratcs
{
    public interface IStockRepository : IGenericServiceDAL<Stock>
    {
        void DescontarStock(Guid idProducto, int cantidad);
        void EliminarStockPorProducto(Guid idProducto);
        List<Stock> GetByProducto(Guid idProducto);

    }
}
