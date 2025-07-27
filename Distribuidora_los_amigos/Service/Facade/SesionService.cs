using Service.DOMAIN;
using Service.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Facade
{
    public static class SesionService
    {
        private static Usuario _usuarioLogueado;

        public static Usuario UsuarioLogueado
        {
            get { return _usuarioLogueado; }
            set { _usuarioLogueado = value; }
        }

        // Método para limpiar la sesión (logout)
        public static void ClearSession()
        {
            _usuarioLogueado = null;
        }

        // Método para obtener el nombre de los roles del usuario logueado
        public static string ObtenerRolesUsuario()
        {

            return SesionLogic.ObtenerRolesUsuario(_usuarioLogueado);
        }
    }
}
