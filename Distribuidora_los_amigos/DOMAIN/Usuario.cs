using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN
{
    /// <summary>
    /// Representa a un usuario del sistema.
    /// La asignación de roles se maneja a través de las Familias a las que pertenece.
    /// </summary>
    public class Usuario
    {
        public Guid ID_Usuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        /*
        public ICollection<Familia> Familias { get; set; }

        public Usuario()
        {
            ID_Usuario = Guid.NewGuid();
            Familias = new List<Familia>();
        }
        */
    }
}
