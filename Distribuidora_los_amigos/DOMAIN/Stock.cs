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

        /// <summary>
        /// Inicializa un registro de stock generando un identificador para el producto y marcándolo como activo.
        /// </summary>
        public Stock()
        {
            IdProducto = Guid.NewGuid();
            Activo = true;
        }
    }
}
