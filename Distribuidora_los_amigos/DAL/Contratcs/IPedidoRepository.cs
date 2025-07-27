using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contracts;
using DOMAIN;

namespace DAL.Contratcs
{
    public interface IPedidoRepository : IGenericServiceDAL<Pedido>
    {
        List<Pedido> GetPedidosPendientes();
        List<EstadoPedido> ObtenerEstadosPedido();

    }
}
