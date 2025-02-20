using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contracts;
using DAL.Implementations.SqlServer;
using DOMAIN;

namespace DAL.Contratcs
{
    public interface IProductoRepository : IGenericServiceDAL<Producto>
    {
        List<Producto> GetByCategoria(string categoria);

        List<Producto> ObtenerProductosActivos();


        void Disable(Guid idProducto);

    }
}
