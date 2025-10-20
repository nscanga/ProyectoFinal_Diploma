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
    public static class UserService
    {
        private static UserLogic _userLogic = new UserLogic();
        private static readonly UsuarioRepository _UsuarioDAL = new UsuarioRepository();

        public static bool Login(string username, string password)
        {
            return _userLogic.ValidateUser(username, password);
        }

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

        public static void DisableUser(Guid idUsuario)
        {
            _userLogic.DisableUser(idUsuario);
        }

        //public static void EnabledUsuario(Guid idUsuario)
        //{
        //    _userLogic.EnabledUsuario(idUsuario);
        //}

        public static void UpdateUserAccesos(Guid idUsuario, List<Acceso> accesos)
        {
            _userLogic.UpdateUserAccesos(idUsuario, accesos);
        }

        public static List<Usuario> GetAllUsuarios()
        {
            return _userLogic.GetAllUsuarios();
        }


        public static Usuario GetUsuarioByUsername(string username)
        {

            return _userLogic.GetUsuarioByUsername(username);
        }
        public static List<UsuarioRolDto> GetUsuariosConFamilasYPatentes()
        {
            return _userLogic.GetUsuariosConFamilasYPatentes();
        }
        private static string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }
        private static bool IsValidEmail(string email)
        {
            // Expresión regular para validar el formato del correo electrónico
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
        public static void SaveLenguaje(Guid idUsuario, string lenguaje)
        {
            _userLogic.SaveLenguaje(idUsuario, lenguaje);
        }


        public static string GetUserLenguaje(Guid idUsuario)
        {
            return _userLogic.GetUserLenguaje(idUsuario);
        }
    }
}

