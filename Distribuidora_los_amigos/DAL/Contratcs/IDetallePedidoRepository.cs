using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contracts;
using DOMAIN;

namespace DAL.Contratcs
{
    public interface IDetallePedidoRepository : IGenericServiceDAL<DetallePedido>
    {
        List<DetallePedido> GetDetallesPorPedido(Guid idPedido);
        List<DetallePedido> ObtenerDetallesPorPedido(Guid idPedido);
        List<DetallePedido> GetByPedido(Guid idPedido); // Nuevo método

    }
}
