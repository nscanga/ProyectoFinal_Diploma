using Service.DAL.Implementations.SqlServer.Helpers;
using Service.DOMAIN;
using Service.DOMAIN.DTO;
using Service.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Service.Facade
{
    /// <summary>
    /// Fachada que expone operaciones de usuario combinando validaciones y lógica de negocio.
    /// </summary>
    public static class UserService
    {
        private static UserLogic _userLogic = new UserLogic();
        private static readonly UsuarioRepository _UsuarioDAL = new UsuarioRepository();

        /// <summary>
        /// Valida credenciales de usuario delegando en la lógica correspondiente.
        /// </summary>
        public static bool Login(string username, string password)
        {
            return _userLogic.ValidateUser(username, password);
        }

        /// <summary>
        /// Registra un nuevo usuario verificando previamente el formato del correo.
        /// </summary>
        public static void Register(string username, string password, string email)
        {
            if (!IsValidEmail(email))
            {
                string messageKey = "El formato del correo electrónico no es válido.";
                string translatedMessage = TranslateMessageKey(messageKey);
                throw new ArgumentException(translatedMessage);
            }
            var usuario = new Usuario
            {
                UserName = username
            };
            _userLogic.CreateUser(usuario, password, email);
        }
        /// <summary>
        /// Crea una patente siempre que no exista otra con el mismo nombre.
        /// </summary>
        public static void CreatePatente(Patente patente)
        {
            if (_UsuarioDAL.ExistePatente(patente.Nombre))
            {
                //LoggerService.WriteLog($"Intento de crear una familia con un nombre ya existente: {patente.Nombre}.", System.Diagnostics.TraceLevel.Warning);

                string messageKey = "Ya existe una familia con el mismo nombre.";
                string translatedMessage = TranslateMessageKey(messageKey);

                throw new Exception(translatedMessage);
            }

            _userLogic.CreatePatente(patente);
        }

        /// <summary>
        /// Deshabilita al usuario indicado.
        /// </summary>
        public static void DisableUser(Guid idUsuario)
        {
            _userLogic.DisableUser(idUsuario);
        }

        //public static void EnabledUsuario(Guid idUsuario)
        //{
        //    _userLogic.EnabledUsuario(idUsuario);
        //}

        /// <summary>
        /// Actualiza los accesos asignados a un usuario.
        /// </summary>
        public static void UpdateUserAccesos(Guid idUsuario, List<Acceso> accesos)
        {
            _userLogic.UpdateUserAccesos(idUsuario, accesos);
        }

        /// <summary>
        /// Devuelve todos los usuarios registrados.
        /// </summary>
        public static List<Usuario> GetAllUsuarios()
        {
            return _userLogic.GetAllUsuarios();
        }


        /// <summary>
        /// Obtiene un usuario por nombre.
        /// </summary>
        public static Usuario GetUsuarioByUsername(string username)
        {

            return _userLogic.GetUsuarioByUsername(username);
        }
        /// <summary>
        /// Lista los usuarios con el detalle de familias y patentes.
        /// </summary>
        public static List<UsuarioRolDto> GetUsuariosConFamilasYPatentes()
        {
            return _userLogic.GetUsuariosConFamilasYPatentes();
        }
        /// <summary>
        /// Traduce una clave de mensaje.
        /// </summary>
        private static string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }
        /// <summary>
        /// Valida el formato del correo electrónico.
        /// </summary>
        private static bool IsValidEmail(string email)
        {
            // Expresión regular para validar el formato del correo electrónico
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
        /// <summary>
        /// Guarda el idioma preferido del usuario.
        /// </summary>
        public static void SaveLenguaje(Guid idUsuario, string lenguaje)
        {
            _userLogic.SaveLenguaje(idUsuario, lenguaje);
        }


        /// <summary>
        /// Obtiene el idioma configurado para el usuario.
        /// </summary>
        public static string GetUserLenguaje(Guid idUsuario)
        {
            return _userLogic.GetUserLenguaje(idUsuario);
        }
    }
}

