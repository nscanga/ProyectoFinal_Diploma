using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOMAIN;

namespace DAL.Contracts
{
    /// <summary>
    /// Define las operaciones de acceso a datos para la entidad <see cref="Usuario"/>.
    /// </summary>
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Inserta un nuevo usuario en el origen de datos.
        /// </summary>
        /// <param name="usuario">Entidad que se guardará.</param>
        void AgregarUsuario(Usuario usuario);

        /// <summary>
        /// Recupera un usuario utilizando su identificador único.
        /// </summary>
        /// <param name="id">Identificador del usuario.</param>
        /// <returns>La entidad encontrada o <c>null</c> si no existe.</returns>
        Usuario ObtenerUsuarioPorId(Guid id);

        /// <summary>
        /// Busca un usuario por su nombre de cuenta.
        /// </summary>
        /// <param name="userName">Nombre de usuario a localizar.</param>
        /// <returns>La entidad correspondiente al nombre solicitado.</returns>
        Usuario ObtenerUsuarioPorNombre(string userName);

        /// <summary>
        /// Actualiza los datos persistidos de un usuario existente.
        /// </summary>
        /// <param name="usuario">Entidad con la información modificada.</param>
        void ActualizarUsuario(Usuario usuario);

        /// <summary>
        /// Elimina un usuario según su identificador.
        /// </summary>
        /// <param name="id">Identificador del usuario a borrar.</param>
        void EliminarUsuario(Guid id);

        /// <summary>
        /// Obtiene todos los usuarios registrados.
        /// </summary>
        /// <returns>Una lista con las cuentas disponibles.</returns>
        List<Usuario> ObtenerTodosUsuarios();
    }
}
