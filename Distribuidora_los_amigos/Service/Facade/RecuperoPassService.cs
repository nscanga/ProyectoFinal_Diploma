using Service.DAL.Contracts;
using Service.DAL.Implementations.SqlServer.Helpers;
using Service.DOMAIN;
using Service.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service.Facade
{
    public static class RecuperoPassService
    {
        private static readonly IUsuarioRepository _usuarioRepository = new UsuarioRepository();
        private static readonly RecuperoPassLogic _recuperoPassLogic = new RecuperoPassLogic(_usuarioRepository);
        private static readonly UserLogic _userLogic = new UserLogic();

        public static Usuario GetUsuario(string username)
        {
            var usuario = _usuarioRepository.GetUsuarioCompletos(username);
            if (usuario == null)
            {
                string messageKey = "El usuario no existe.";
                string translatedMessage = TranslateMessageKey(messageKey);
                throw new Exception(translatedMessage);
            }

            if (string.IsNullOrEmpty(usuario.Email))
            {
                string messageKey = "El usuario no tiene un correo registrado.";
                string translatedMessage = TranslateMessageKey(messageKey);
                throw new Exception(translatedMessage);
            }

            return usuario;
        }

        public static void GenerarYEnviarMailRecuperacion(string username)
        {
            var usuario = GetUsuario(username);
            if (usuario != null)
            {
                _recuperoPassLogic.GenerarMail(usuario);
            }
        }

        public static bool ValidateRecoveryToken(string username, string token)
        {
            var usuario = GetUsuario(username);
            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado.");
            }
            return _recuperoPassLogic.EsValidoRecoveryToken(usuario, token);
        }

        private static string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }

        public static bool ChangePassword(string username, string newPassword, string token, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                string messageKey = "Por favor, completa ambos campos.";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage);
                return false;
            }

            if (newPassword != confirmPassword)
            {
                string messageKey = "Las contraseñas no coinciden.";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage);
                return false;
            }

            var usuario = GetUsuario(username);
            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            // Validar el token antes de cambiar la contraseña
            if (!_recuperoPassLogic.EsValidoRecoveryToken(usuario, token))
            {
                string messageKey = "El token no es válido o ha expirado.";
                string translatedMessage = TranslateMessageKey(messageKey);
                throw new Exception(translatedMessage);
            }

            _recuperoPassLogic.ChangePassword(usuario, newPassword);
            return true;
        }
    }
}
