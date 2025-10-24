using Service.DAL.Contracts;
using Service.DAL.Implementations.SqlServer.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Service.DAL.Implementations.SqlServer
{
    /// <summary>
    /// Implementación SQL Server para almacenar y recuperar dígitos verificadores horizontales.
    /// </summary>
    public class DVHRepository : IDVHRepository
    {
        /// <summary>
        /// Inserta un DVH asociado a un registro específico de una tabla determinada.
        /// </summary>
        /// <param name="idRegistro">Identificador del registro a proteger.</param>
        /// <param name="nombreTabla">Tabla a la que pertenece el registro.</param>
        /// <param name="dvh">Valor del dígito verificador calculado.</param>
        public void GuardarDVH(Guid idRegistro, string nombreTabla, string dvh)
        {
            try
            {
                // Usar SQL Helper o una conexión directa para insertar el DVH
                SqlHelper.ExecuteNonQuery(
                    "INSERT INTO DigitosVerificadores (IdRegistro, NombreTabla, DVH, FechaModificacion) VALUES (@IdRegistro, @NombreTabla, @DVH, GETDATE())",
                    CommandType.Text,
                    new SqlParameter("@IdRegistro", idRegistro),
                    new SqlParameter("@NombreTabla", nombreTabla),
                    new SqlParameter("@DVH", dvh)
                );
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'DigitosVerificadores'"))
            {
                // La tabla DigitosVerificadores no existe, continuar sin error
                // TODO: Crear la tabla DigitosVerificadores en la base de datos
            }
        }
        
        /// <summary>
        /// Actualiza el DVH almacenado para un registro dado incluyendo la fecha de modificación.
        /// </summary>
        /// <param name="idRegistro">Identificador del registro a actualizar.</param>
        /// <param name="nombreTabla">Tabla a la que pertenece el registro.</param>
        /// <param name="dvh">Nuevo dígito verificador a persistir.</param>
        public void ActualizarDVH(Guid idRegistro, string nombreTabla, string dvh)
        {
            try
            {
                // Query de actualización para el DVH con la fecha de modificación incluida
                string query = "UPDATE DigitosVerificadores SET DVH = @DVH, FechaModificacion = GETDATE() WHERE IdRegistro = @IdRegistro AND NombreTabla = @NombreTabla";

                // Usar SQL Helper para ejecutar la actualización
                SqlHelper.ExecuteNonQuery(
                    query,
                    CommandType.Text,
                    new SqlParameter("@IdRegistro", idRegistro),
                    new SqlParameter("@NombreTabla", nombreTabla),
                    new SqlParameter("@DVH", dvh)
                );
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'DigitosVerificadores'"))
            {
                // La tabla DigitosVerificadores no existe, continuar sin error
                // TODO: Crear la tabla DigitosVerificadores en la base de datos
            }
        }
        
        /// <summary>
        /// Recupera el DVH asociado a un usuario determinado.
        /// </summary>
        /// <param name="idUsuario">Identificador del usuario.</param>
        /// <returns>Cadena con el DVH o <c>null</c> si no existe.</returns>
        public string ObtenerCodigoVerificador(Guid idUsuario)
        {
            string codigoVerificador = null;

            try
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(
                    "SELECT DVH FROM DigitosVerificadores WHERE IdRegistro = @IdRegistro AND NombreTabla = 'Usuario'",
                    CommandType.Text,
                    new SqlParameter("@IdRegistro", idUsuario)))
                {
                    if (reader.Read())
                    {
                        codigoVerificador = reader.GetString(0);  // Recupera el DVH almacenado
                    }
                }
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'DigitosVerificadores'"))
            {
                // La tabla DigitosVerificadores no existe, devolver null
                // TODO: Crear la tabla DigitosVerificadores en la base de datos
                return null;
            }

            return codigoVerificador;
        }
    }
}
