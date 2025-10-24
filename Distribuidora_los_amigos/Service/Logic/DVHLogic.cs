using Service.DAL.Contracts;
using Service.DAL.FactoryServices;
using Service.DAL.Implementations.SqlServer;
using Service.DAL.Implementations.SqlServer.Helpers;
using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Service.Logic
{
    /// <summary>
    /// Encapsula la lógica de cálculo y persistencia de dígitos verificadores horizontales.
    /// </summary>
    public class DVHLogic
    {
        private readonly IDVHRepository _dvhRepository;

        private readonly IUsuarioRepository _usuarioRepository;

        /// <summary>
        /// Crea la lógica empleando las implementaciones concretas configuradas.
        /// </summary>
        public DVHLogic()
        {
            _usuarioRepository = FactoryDAL.UsuarioRepository;
            _dvhRepository = new DVHRepository(); // Asume que DVHRepository es la implementación concreta
        }

        /// <summary>
        /// Permite inyectar un repositorio personalizado para pruebas o alternativas de persistencia.
        /// </summary>
        /// <param name="dvhRepository">Repositorio responsable de guardar los DVH.</param>
        public DVHLogic(IDVHRepository dvhRepository)
        {
            _dvhRepository = dvhRepository;
        }
        /// <summary>
        /// Calcula el DVH del usuario y lo almacena retornando el valor generado.
        /// </summary>
        /// <param name="usuario">Usuario sobre el cual se calcula el DVH.</param>
        /// <returns>Cadena resultante del cálculo del DVH.</returns>
        public string GenerarYGuardarCodigoVerificador(Usuario usuario)
        {
            // Concatenar las propiedades relevantes del usuario para el cálculo del DVH
            string data = usuario.UserName + usuario.Password + usuario.Estado + usuario.Email + usuario.Lenguaje
                    + usuario.RecoveryToken + usuario.TokenExpiration.ToString("yyyyMMddHHmmss");

            // Generar el hash (por ejemplo, usando SHA256)
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                string codigoVerificador = builder.ToString();

                // Llamar al DVHRepository para guardar el DVH
                _dvhRepository.GuardarDVH(usuario.IdUsuario, "Usuario", codigoVerificador);

                return codigoVerificador; // Retornar el código verificador por si es necesario
            }
        }

        /// <summary>
        /// Recalcula y actualiza el DVH almacenado para el usuario.
        /// </summary>
        /// <param name="usuario">Usuario cuyos datos se verifican.</param>
        public void ActualizarCodigoVerificador(Usuario usuario)
        {
            // Concatenar las propiedades relevantes del usuario para el cálculo del DVH
            string data = usuario.UserName + usuario.Password + usuario.Estado + usuario.Email + usuario.Lenguaje
                    + usuario.RecoveryToken + usuario.TokenExpiration.ToString("yyyyMMddHHmmss");

            // Generar el hash (por ejemplo, usando SHA256)
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                string codigoVerificador = builder.ToString();

                // Actualizar el DVH en la base de datos
                _dvhRepository.ActualizarDVH(usuario.IdUsuario, "Usuario", codigoVerificador);
            }
        }
        /// <summary>
        /// Genera el DVH correspondiente a los datos del usuario sin persistirlo.
        /// </summary>
        /// <param name="usuario">Usuario base del cálculo.</param>
        /// <returns>Código verificador en formato hexadecimal.</returns>
        private string GenerarCodigoVerificador(Usuario usuario)
        {
            // Concatenar las propiedades relevantes del usuario para el cálculo del DVH
            string data = usuario.UserName + usuario.Password + usuario.Estado + usuario.Email + usuario.Lenguaje
                + usuario.RecoveryToken + usuario.TokenExpiration.ToString("yyyyMMddHHmmss");

            // Generar el hash (por ejemplo, usando SHA256)
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));

                // Convertir el hash a un string hexadecimal
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Formato hexadecimal
                }

                return builder.ToString();
            }
        }
        /// <summary>
        /// Compara el DVH generado con el almacenado para detectar inconsistencias.
        /// </summary>
        /// <param name="usuario">Usuario a validar.</param>
        /// <returns><c>true</c> cuando ambos DVH coinciden.</returns>
        public bool VerificarDVH(Usuario usuario)
        {

            // Generar el DVH para el usuario
            string codigoVerificadorGenerado = GenerarCodigoVerificador(usuario);

            // Obtener el DVH almacenado de la base de datos
            string codigoVerificadorAlmacenado = _dvhRepository.ObtenerCodigoVerificador(usuario.IdUsuario);

            // Comparar el DVH generado con el almacenado
            return codigoVerificadorGenerado == codigoVerificadorAlmacenado;
        }
    }
}

