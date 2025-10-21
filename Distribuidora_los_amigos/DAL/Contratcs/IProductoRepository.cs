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
    /// <summary>
    /// Agrupa las operaciones de acceso a datos para la entidad <see cref="Producto"/>.
    /// </summary>
    public interface IProductoRepository : IGenericServiceDAL<Producto>
    {
        /// <summary>
        /// Busca productos filtrando por la categoría indicada.
        /// </summary>
        /// <param name="categoria">Nombre de la categoría a consultar.</param>
        /// <returns>Una lista de productos pertenecientes a la categoría.</returns>
        List<Producto> GetByCategoria(string categoria);

        /// <summary>
        /// Obtiene solamente los productos marcados como activos.
        /// </summary>
        /// <returns>La colección de productos disponibles para la venta.</returns>
        List<Producto> ObtenerProductosActivos();

        /// <summary>
        /// Realiza la baja lógica de un producto especificado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto que se deshabilita.</param>
        void Disable(Guid idProducto);

    }
}
