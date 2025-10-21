using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Logic
{
    /// <summary>
    /// Proporciona utilidades relacionadas con la sesión del usuario.
    /// </summary>
    public static class SesionLogic
    {
        // Método para obtener los roles (familias) del usuario logueado
        /// <summary>
        /// Devuelve los nombres de los roles asociados al usuario autenticado.
        /// </summary>
        /// <param name="usuario">Usuario del cual se obtienen las familias.</param>
        /// <returns>Cadena con los roles o un mensaje predeterminado.</returns>
        public static string ObtenerRolesUsuario(Usuario usuario)
        {
            // Verificar que el usuario no sea nulo
            if (usuario == null)
            {
                return "Usuario no logueado";
            }

            // Obtener las familias (roles) del usuario
            List<Familia> familiasDelUsuario = usuario.GetFamilias();

            // Retornar una cadena con los nombres de las familias o "Sin rol asignado" si no tiene roles
            return familiasDelUsuario.Any()
                ? string.Join(", ", familiasDelUsuario.Select(f => f.Nombre))
                : "Sin rol asignado";
        }
    }
}
