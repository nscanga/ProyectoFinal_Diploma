using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN
{
    public class DetallePedido
    {
        public Guid IdDetallePedido { get; set; }
        public Guid IdPedido { get; set; }
        public Guid IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; } // Cantidad × Precio del producto

        public DetallePedido() 
        {
            IdDetallePedido = Guid.NewGuid();
        }
    }
}
