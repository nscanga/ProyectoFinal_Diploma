using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contratcs;
using DAL.Factory;
using DOMAIN;

namespace BLL
{
    public class DetallePedidoService
    {
        private readonly IDetallePedidoRepository _detallePedidoRepository;

        /// <summary>
        /// Inicializa el servicio de detalles de pedido configurando el repositorio correspondiente.
        /// </summary>
        public DetallePedidoService()
        {
            _detallePedidoRepository = FactoryDAL.SqlDetallePedidoRepository;
        }

        /// <summary>
        /// Inserta un nuevo detalle asociado a un pedido específico.
        /// </summary>
        /// <param name="detalle">Detalle del pedido que se guardará.</param>
        public void AgregarDetalle(DetallePedido detalle)
        {
            ValidarDetallePedido(detalle);
            _detallePedidoRepository.Add(detalle);
        }

        /// <summary>
        /// Valida que la cantidad y el precio unitario del detalle sean mayores a cero.
        /// </summary>
        /// <param name="detalle">Detalle a evaluar.</param>
        private void ValidarDetallePedido(DetallePedido detalle)
        {
            if (detalle.Cantidad <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor a 0.");
            }

            if (detalle.PrecioUnitario <= 0)
            {
                throw new ArgumentException("El precio unitario debe ser mayor a 0.");
            }
        }

        /// <summary>
        /// Actualiza un detalle existente luego de validar sus datos.
        /// </summary>
        /// <param name="detalle">Detalle con los cambios a aplicar.</param>
        public void ModificarDetalle(DetallePedido detalle)
        {
            ValidarDetallePedido(detalle);
            _detallePedidoRepository.Update(detalle);
        }

        /// <summary>
        /// Quita un detalle de pedido identificado por su Id.
        /// </summary>
        /// <param name="idDetalle">Identificador del detalle a eliminar.</param>
        public void EliminarDetalle(Guid idDetalle)
        {
            _detallePedidoRepository.Remove(idDetalle);
        }

        /// <summary>
        /// Obtiene todos los detalles que pertenecen a un pedido determinado.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Lista de detalles asociados.</returns>
        public List<DetallePedido> ObtenerDetallesPorPedido(Guid idPedido)
        {
            return _detallePedidoRepository.ObtenerDetallesPorPedido(idPedido);
        }
    }
}
