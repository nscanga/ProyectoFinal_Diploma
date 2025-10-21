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
    /// Representa el contrato específico para administrar los estados de los pedidos.
    /// </summary>
    public interface IEstadoPedidoRepository : IGenericServiceDAL<EstadoPedido>
    {

    }
}
