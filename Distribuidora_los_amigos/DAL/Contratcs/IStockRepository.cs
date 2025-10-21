using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contracts;
using DOMAIN;

namespace DAL.Contratcs
{
    /// <summary>
    /// Establece las operaciones de persistencia orientadas a la gestión de inventario.
    /// </summary>
    public interface IStockRepository : IGenericServiceDAL<Stock>
    {
        /// <summary>
        /// Reduce la cantidad disponible del producto indicado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto cuyo stock se descuenta.</param>
        /// <param name="cantidad">Cantidad que se restará del inventario.</param>
        void DescontarStock(Guid idProducto, int cantidad);

        /// <summary>
        /// Incrementa la cantidad disponible del producto indicado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto cuyo stock se incrementa.</param>
        /// <param name="cantidad">Cantidad que se sumará al inventario.</param>
        void AumentarStock(Guid idProducto, int cantidad);  // ← AGREGAR ESTE MÉTODO

        /// <summary>
        /// Elimina todos los registros de stock asociados al producto especificado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto cuyo stock se eliminará.</param>
        void EliminarStockPorProducto(Guid idProducto);

        /// <summary>
        /// Obtiene los registros de stock vinculados a un producto determinado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto del que se requiere información.</param>
        /// <returns>Una lista con los movimientos o existencias registrados.</returns>
        List<Stock> GetByProducto(Guid idProducto);

    }
}
