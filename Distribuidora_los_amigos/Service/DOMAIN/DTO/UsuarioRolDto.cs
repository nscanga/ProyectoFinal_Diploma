using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DOMAIN.DTO
{
    /// <summary>
    /// DTO que representa un usuario con su familia asignada y estado.
    /// </summary>
    public class UsuarioRolDto
    {
        /// <summary>
        /// Nombre de usuario.
        /// </summary>
        public string Usuario { get; set; }

        /// <summary>
        /// Familia (rol) asignada al usuario. Puede contener múltiples familias separadas por comas.
        /// </summary>
        public string Familia { get; set; }

        /// <summary>
        /// Estado del usuario (Habilitado/Deshabilitado).
        /// </summary>
        public string Estado { get; set; }
    }
}
