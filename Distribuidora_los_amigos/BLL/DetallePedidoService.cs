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

        public DetallePedidoService()
        {
            _detallePedidoRepository = FactoryDAL.SqlDetallePedidoRepository;
        }

        public void AgregarDetalle(DetallePedido detalle)
        {
            ValidarDetallePedido(detalle);
            _detallePedidoRepository.Add(detalle);
        }

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

        public void ModificarDetalle(DetallePedido detalle)
        {
            ValidarDetallePedido(detalle);
            _detallePedidoRepository.Update(detalle);
        }

        public void EliminarDetalle(Guid idDetalle)
        {
            _detallePedidoRepository.Remove(idDetalle);
        }

        public List<DetallePedido> ObtenerDetallesPorPedido(Guid idPedido)
        {
            return _detallePedidoRepository.ObtenerDetallesPorPedido(idPedido);
        }
    }
}
