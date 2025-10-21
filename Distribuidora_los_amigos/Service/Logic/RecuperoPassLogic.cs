using Service.DAL.Contracts;
using Service.DOMAIN;
using Service.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Logic
{
    /// <summary>
    /// Gestiona el flujo de recuperación y cambio de contraseña de los usuarios.
    /// </summary>
    public class RecuperoPassLogic
    {
        private static DVHLogic _dvhLogic = new DVHLogic();
        private readonly IUsuarioRepository _usuarioRepository;

        /// <summary>
        /// Inicializa la lógica de recuperación con el repositorio de usuarios.
        /// </summary>
        public RecuperoPassLogic(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        /// <summary>
        /// Genera un token numérico de seis dígitos para la recuperación de contraseña.
        /// </summary>
        public static string GenerateRecoveryToken()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Genera un número de 6 dígitos
        }

        /// <summary>
        /// Genera y envía el correo de recuperación asignando el token al usuario.
        /// </summary>
        /// <param name="usuario">Usuario que solicita la recuperación.</param>
        public void GenerarMail(Usuario usuario)
        {
            string token = GenerateRecoveryToken();
            usuario.RecoveryToken = token;
            usuario.TokenExpiration = DateTime.Now.AddMinutes(10); // Expira en 10 minutos

            // Guardar el token y su expiración en la base de datos
            _usuarioRepository.UpdateUsuarioToken(usuario);

            // Enviar el email
            EmailService.SendRecoveryEmail(usuario.Email, token);


            var usuario1 = _usuarioRepository.ObetenerUsuarioById(usuario.IdUsuario);
            if (usuario1 != null)
            {
                _dvhLogic.ActualizarCodigoVerificador(usuario1);
            }
        }
        /// <summary>
        /// Valida que el token recibido coincida con el registrado y que no esté vencido.
        /// </summary>
        /// <param name="usuario">Usuario al que pertenece el token.</param>
        /// <param name="token">Token ingresado por el usuario.</param>
        /// <returns><c>true</c> si el token es válido.</returns>
        public bool EsValidoRecoveryToken(Usuario usuario, string token)
        {
            return usuario.RecoveryToken == token && usuario.TokenExpiration > DateTime.Now;
        }
        /// <summary>
        /// Actualiza la contraseña del usuario y recalcula su DVH.
        /// </summary>
        /// <param name="usuario">Usuario que realiza el cambio.</param>
        /// <param name="newPassword">Nueva contraseña en texto plano.</param>
        public void ChangePassword(Usuario usuario, string newPassword)
        {
            // Encriptar la nueva contraseña
            usuario.Password = CryptographyService.HashMd5(newPassword);

            // Actualizar la contraseña en la base de datos
            _usuarioRepository.UpdatePassword(usuario);

            var usuario1 = _usuarioRepository.ObetenerUsuarioById(usuario.IdUsuario);
            if (usuario1 != null)
            {
                _dvhLogic.ActualizarCodigoVerificador(usuario1);
            }
        }

    }
}
