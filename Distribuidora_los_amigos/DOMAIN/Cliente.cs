using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN
{
    public class Cliente
    {
        public Guid IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string CUIT { get; set; } 
        public bool Activo { get; set; }

        public Cliente()
        {
            IdCliente = Guid.NewGuid();
            Activo = true;
        }
    }
}
