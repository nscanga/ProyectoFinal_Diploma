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
    public class DVHLogic
    {
        private readonly IDVHRepository _dvhRepository;

        private readonly IUsuarioRepository _usuarioRepository;

        // Constructor sin parámetros que inicializa la dependencia
        public DVHLogic()
        {
            _usuarioRepository = FactoryDAL.UsuarioRepository;
            _dvhRepository = new DVHRepository(); // Asume que DVHRepository es la implementación concreta
        }

        public DVHLogic(IDVHRepository dvhRepository)
        {
            _dvhRepository = dvhRepository;
        }
        public string GenerarYGuardarCodigoVerificador(Usuario usuario)
        {
            // Concatenar las propiedades relevantes del usuario para el cálculo del DVH
            string data = usuario.UserName + usuario.Password + usuario.Estado + usuario.Email + usuario.Language
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

        public void ActualizarCodigoVerificador(Usuario usuario)
        {
            // Concatenar las propiedades relevantes del usuario para el cálculo del DVH
            string data = usuario.UserName + usuario.Password + usuario.Estado + usuario.Email + usuario.Language
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
        private string GenerarCodigoVerificador(Usuario usuario)
        {
            // Concatenar las propiedades relevantes del usuario para el cálculo del DVH
            string data = usuario.UserName + usuario.Password + usuario.Estado + usuario.Email + usuario.Language
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
        public bool VerificarDVH(Usuario usuario)
        {

            // Generar el DVH para el usuario
            string codigoVerificadorGenerado = GenerarCodigoVerificador(usuario);

            // Obtener el DVH almacenado de la base de datos
            string codigoVerificadorAlmacenado = _dvhRepository.ObtenerCodigoVerificador(usuario.IdUsuario);

            // Comparar el DVH generado con el almacenado
            return codigoVerificadorGenerado == codigoVerificadorAlmacenado;
        }


        //    public string DVHCitaDetalle(CitaDetalle citaDetalle)
        //    {
        //        Concatenar las propiedades relevantes de CitaDetalle para el cálculo del DVH
        //        string data = citaDetalle.IdCitaDetalle.ToString() +
        //                      citaDetalle.IdCita.ToString() +
        //                      citaDetalle.Fecha.ToString("yyyyMMdd") +
        //                      citaDetalle.HoraInicio.ToString(@"hh\:mm\:ss") +
        //                      citaDetalle.HoraFin.ToString(@"hh\:mm\:ss") +
        //                      citaDetalle.NombreProfesional +
        //                      citaDetalle.NombreTratamiento +
        //                      citaDetalle.EtapaTratamiento +
        //                      citaDetalle.Observaciones +
        //                      citaDetalle.Recomendaciones +
        //                      citaDetalle.IdPaciente.ToString();

        //        Generar el hash(por ejemplo, usando SHA256)
        //        using (SHA256 sha256 = SHA256.Create())
        //        {
        //            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        //            StringBuilder builder = new StringBuilder();
        //            foreach (byte b in bytes)
        //            {
        //                builder.Append(b.ToString("x2"));
        //            }
        //            string codigoVerificador = builder.ToString();

        //            Llamar al DVHRepository para guardar el DVH
        //            _dvhRepository.GuardarDVHCitaDetalle(citaDetalle.IdCitaDetalle, "CitaDetalle", codigoVerificador);

        //            return codigoVerificador; // Retornar el código verificador por si es necesario
        //        }
        //    }

        //    private string GenerarDVHCitaDetalle(CitaDetalle citaDetalle)
        //    {
        //        Concatenar las propiedades relevantes de CitaDetalle para el cálculo del DVH
        //        string data = citaDetalle.IdCitaDetalle.ToString() +
        //                      citaDetalle.IdCita.ToString() +
        //                      citaDetalle.Fecha.ToString("yyyyMMdd") +
        //                      citaDetalle.HoraInicio.ToString(@"hh\:mm\:ss") +
        //                      citaDetalle.HoraFin.ToString(@"hh\:mm\:ss") +
        //                      citaDetalle.NombreProfesional +
        //                      citaDetalle.NombreTratamiento +
        //                      citaDetalle.EtapaTratamiento +
        //                      citaDetalle.Observaciones +
        //                      citaDetalle.Recomendaciones +
        //                      citaDetalle.IdPaciente.ToString();

        //        Generar el hash(por ejemplo, usando SHA256)
        //        using (SHA256 sha256 = SHA256.Create())
        //        {
        //            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));

        //            Convertir el hash a un string hexadecimal
        //            StringBuilder builder = new StringBuilder();
        //            foreach (byte b in bytes)
        //            {
        //                builder.Append(b.ToString("x2")); // Formato hexadecimal
        //            }

        //            return builder.ToString();
        //        }
        //    }
        //    public bool VerificarDVHCitaDetalle(CitaDetalle citaDetalle)
        //    {
        //        Generar el DVH para CitaDetalle
        //        string codigoVerificadorGenerado = GenerarDVHCitaDetalle(citaDetalle);

        //        Obtener el DVH almacenado de la base de datos
        //        string codigoVerificadorAlmacenado = _dvhRepository.ObtenerCodigoVerificadorCitaDetalle(citaDetalle.IdCitaDetalle);

        //        Comparar el DVH generado con el almacenado
        //        return codigoVerificadorGenerado == codigoVerificadorAlmacenado;
        //    }
        //}
    }
}

