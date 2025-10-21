using Service.DOMAIN;
using Service.DOMAIN.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Contracts
{
    /// <summary>
    /// Define las operaciones de acceso a datos específicas para usuarios y su esquema de permisos.
    /// </summary>
    public interface IUsuarioRepository : IGenericServiceDAL<Usuario>
    {
        /// <summary>
        /// Obtiene un usuario a partir de su nombre de inicio de sesión.
        /// </summary>
        Usuario GetUsuarioByUsername(string username);
        /// <summary>
        /// Registra un nuevo usuario en la base de datos.
        /// </summary>
        void CreateUsuario(Usuario usuario);
        /// <summary>
        /// Marca un usuario como deshabilitado.
        /// </summary>
        void DisableUsuario(Guid idUsuario);
        /// <summary>
        /// Reemplaza la lista de accesos (patentes/familias) de un usuario.
        /// </summary>
        void UpdateAccesos(Guid idUsuario, List<Acceso> accesos);

        /// <summary>
        /// Vuelve a habilitar un usuario previamente bloqueado.
        /// </summary>
        void EnabledUsuario(Guid idUsuario);
        /// <summary>
        /// Agrega una nueva patente al sistema.
        /// </summary>
        void CreatePatente(Patente patente);

        /// <summary>
        /// Obtiene un usuario por su identificador.
        /// </summary>
        Usuario GetUsuarioById(Guid idUsuario);

        /// <summary>
        /// Recupera un usuario completo tolerando columnas opcionales.
        /// </summary>
        Usuario ObetenerUsuarioById(Guid idUsuario);
        /// <summary>
        /// Determina si el usuario posee acceso a un tipo de patente específico.
        /// </summary>
        bool HasAccess(Guid idUsuario, TipoAcceso tipoAcceso);

        /// <summary>
        /// Lista usuarios con sus familias y patentes relacionadas.
        /// </summary>
        List<UsuarioRolDto> GetUsuariosConFamilasYPatentes();
        /// <summary>
        /// Verifica la existencia de una patente por nombre.
        /// </summary>
        bool ExistePatente(string nombrePatente);

        /// <summary>
        /// Obtiene un usuario incluyendo sus datos de recuperación.
        /// </summary>
        Usuario GetUsuarioCompletos(string username);

        /// <summary>
        /// Actualiza el token de recuperación asignado al usuario.
        /// </summary>
        void UpdateUsuarioToken(Usuario usuario);
        /// <summary>
        /// Modifica la contraseña del usuario.
        /// </summary>
        void UpdatePassword(Usuario usuario);

        /// <summary>
        /// Establece el idioma preferido de un usuario.
        /// </summary>
        void UpdateLenguaje(Guid idUsuario, string lenguaje);

        /// <summary>
        /// Recupera el idioma configurado para el usuario indicado.
        /// </summary>
        string GetUserLenguaje(Guid idUsuario);
    }
}
