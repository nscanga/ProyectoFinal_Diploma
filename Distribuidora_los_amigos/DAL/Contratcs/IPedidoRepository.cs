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
    /// Declara las operaciones específicas de persistencia para la entidad <see cref="Pedido"/>.
    /// </summary>
    public interface IPedidoRepository : IGenericServiceDAL<Pedido>
    {
        /// <summary>
        /// Obtiene la lista de pedidos que aún no han sido completados.
        /// </summary>
        /// <returns>Los pedidos cuyo estado se encuentra pendiente.</returns>
        List<Pedido> GetPedidosPendientes();

        /// <summary>
        /// Devuelve el catálogo de estados disponibles para los pedidos.
        /// </summary>
        /// <returns>Una colección de entidades de estado de pedido.</returns>
        List<EstadoPedido> ObtenerEstadosPedido();

        /// <summary>
        /// Actualiza el estado de un pedido existente.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido a actualizar.</param>
        /// <param name="nuevoEstadoId">Identificador del estado que se aplicará.</param>
        void UpdateEstado(Guid idPedido, Guid nuevoEstadoId);
    }
}
