using Service.DOMAIN;
using Service.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Facade
{
    /// <summary>
    /// Administra la información de la sesión del usuario en la capa de presentación.
    /// </summary>
    public static class SesionService
    {
        private static Usuario _usuarioLogueado;

        /// <summary>
        /// Usuario autenticado actualmente en el sistema.
        /// </summary>
        public static Usuario UsuarioLogueado
        {
            get { return _usuarioLogueado; }
            set { _usuarioLogueado = value; }
        }

        // Método para limpiar la sesión (logout)
        /// <summary>
        /// Limpia la sesión del usuario, efectuando el cierre de sesión.
        /// </summary>
        public static void ClearSession()
        {
            _usuarioLogueado = null;
        }

        // Método para obtener el nombre de los roles del usuario logueado
        /// <summary>
        /// Devuelve la lista de roles asociados al usuario autenticado.
        /// </summary>
        public static string ObtenerRolesUsuario()
        {

            return SesionLogic.ObtenerRolesUsuario(_usuarioLogueado);
        }
    }
}
