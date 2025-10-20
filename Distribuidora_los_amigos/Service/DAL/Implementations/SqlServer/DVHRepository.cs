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
    public class DVHRepository : IDVHRepository
    {
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
