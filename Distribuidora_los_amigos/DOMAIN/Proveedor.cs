using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN
{
    public class Proveedor
    {
        public Guid IdProveedor { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Categoria { get; set; }
        public bool Activo { get; set; }

        /// <summary>
        /// Crea un proveedor asignando un identificador único y habilitándolo por defecto.
        /// </summary>
        public Proveedor()
        {
            IdProveedor = Guid.NewGuid();
            Activo = true;
        }
    }
}
