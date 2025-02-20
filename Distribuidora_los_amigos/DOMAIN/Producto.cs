using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN
{
    public class Producto
    {
        public Guid IdProducto { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime? Vencimiento { get; set; }
        public string Categoria { get; set; }
        public bool Activo { get; set; }


        public Producto() 
        {
            IdProducto = Guid.NewGuid();
            Activo = true;
        }
    }



}
