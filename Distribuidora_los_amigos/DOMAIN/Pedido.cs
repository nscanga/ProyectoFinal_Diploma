using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN
{
    public class Pedido
    {
        public Guid IdPedido { get; set; }
        public Guid IdCliente { get; set; }
        public DateTime FechaPedido { get; set; }
        public decimal Total { get; set; }
        public Guid IdEstadoPedido { get; set; }
        public string NombreEstado { get; set; } 
        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();

        public Pedido() 
        {
            IdPedido = Guid.NewGuid();
        }
    }
}
