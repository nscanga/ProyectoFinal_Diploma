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
    public class RecuperoPassLogic
    {
        private static DVHLogic _dvhLogic = new DVHLogic();
        private readonly IUsuarioRepository _usuarioRepository;

        public RecuperoPassLogic(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public static string GenerateRecoveryToken()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Genera un número de 6 dígitos
        }

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
        public bool EsValidoRecoveryToken(Usuario usuario, string token)
        {
            return usuario.RecoveryToken == token && usuario.TokenExpiration > DateTime.Now;
        }
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
