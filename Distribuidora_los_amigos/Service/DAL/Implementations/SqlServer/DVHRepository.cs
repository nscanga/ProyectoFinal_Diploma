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

        /// <summary>
        /// Inserta el DVH correspondiente a un detalle de cita en la tabla de verificadores.
        /// </summary>
        /// <param name="idCitaDetalle">Identificador del detalle de cita.</param>
        /// <param name="nombreTabla">Nombre de la tabla donde se encuentra el detalle.</param>
        /// <param name="dvh">Valor calculado del DVH.</param>
        public void GuardarDVHCitaDetalle(Guid idCitaDetalle, string nombreTabla, string dvh)
        {
            try
            {
                // Obtener la cadena de conexión desde el archivo de configuración
                string connectionString = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Crear el comando SQL para insertar o actualizar el DVH junto con la fecha
                    string query = @"
                INSERT INTO DigitosVerificadores (IdRegistro, NombreTabla, DVH, FechaModificacion)
                VALUES (@IdRegistro, @NombreTabla, @DVH, GETDATE())";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Agregar parámetros al comando
                        command.Parameters.AddWithValue("@IdRegistro", idCitaDetalle);
                        command.Parameters.AddWithValue("@NombreTabla", nombreTabla);
                        command.Parameters.AddWithValue("@DVH", dvh);
                       

                        // Abrir la conexión y ejecutar el comando
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid object name 'DigitosVerificadores'"))
            {
                // La tabla DigitosVerificadores no existe, continuar sin error
                // TODO: Crear la tabla DigitosVerificadores en la base de datos
            }
        }
        /// <summary>
        /// Obtiene el DVH almacenado para un detalle de cita determinado.
        /// </summary>
        /// <param name="idCitaDetalle">Identificador del detalle de cita.</param>
        /// <returns>Cadena con el DVH asociado o <c>null</c> si no se encuentra.</returns>
        public string ObtenerCodigoVerificadorCitaDetalle(Guid idCitaDetalle)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;
            string codigoVerificador = null;

            try
            {
                // Consulta para obtener el DVH de la CitaDetalle
                string query = @"
                 SELECT DVH 
                FROM DigitosVerificadores 
                WHERE IdRegistro = @IdRegistro AND NombreTabla = 'CitaDetalle'";

                // Usamos SqlConnection para ejecutar la consulta
                using (SqlConnection connection = new SqlConnection(connectionString)) // Asegúrate de tener tu connectionString
                {
                      connection.Open();

                        // Ejecutamos la consulta con SqlCommand
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Asignamos el valor del parámetro IdRegistro
                            command.Parameters.AddWithValue("@IdRegistro", idCitaDetalle);

                            // Ejecutamos la consulta y leemos el resultado
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // Recuperamos el DVH almacenado para la CitaDetalle
                                    codigoVerificador = reader.GetString(0);
                                }
                            }
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
