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
    /// Expone operaciones específicas para los detalles asociados a un pedido.
    /// </summary>
    public interface IDetallePedidoRepository : IGenericServiceDAL<DetallePedido>
    {
        /// <summary>
        /// Obtiene las líneas de detalle vinculadas a un pedido identificadas por GUID.
        /// </summary>
        /// <param name="idPedido">Identificador único del pedido.</param>
        /// <returns>Una lista con los detalles que pertenecen al pedido indicado.</returns>
        List<DetallePedido> GetDetallesPorPedido(Guid idPedido);

        /// <summary>
        /// Recupera los detalles de un pedido utilizando la estrategia de mapeo tradicional.
        /// </summary>
        /// <param name="idPedido">Identificador único del pedido.</param>
        /// <returns>Una colección de detalles asociados al pedido solicitado.</returns>
        List<DetallePedido> ObtenerDetallesPorPedido(Guid idPedido);

        /// <summary>
        /// Devuelve las filas de detalle mediante lectura directa sobre la tabla de pedidos.
        /// </summary>
        /// <param name="idPedido">Identificador único del pedido.</param>
        /// <returns>Los detalles correspondientes al pedido suministrado.</returns>
        List<DetallePedido> GetByPedido(Guid idPedido); // Nuevo método

    }
}
