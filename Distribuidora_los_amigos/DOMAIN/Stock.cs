using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN
{
    public class Stock
    {
        public Guid IdStock { get; set; }
        public Guid IdProducto { get; set; }
        public int Cantidad { get; set; }
        public string Tipo { get; set; }
        public bool Activo { get; set; }


        public Stock()
        {
            IdProducto = Guid.NewGuid();
            Activo = true;
        }
    }
}
