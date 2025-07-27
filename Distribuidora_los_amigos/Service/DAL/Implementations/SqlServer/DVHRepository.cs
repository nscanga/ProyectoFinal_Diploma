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
            // Usar SQL Helper o una conexión directa para insertar el DVH
            SqlHelper.ExecuteNonQuery(
                "INSERT INTO DigitosVerificadores (IdRegistro, NombreTabla, DVH, FechaModificacion) VALUES (@IdRegistro, @NombreTabla, @DVH, GETDATE())",
                CommandType.Text,
                new SqlParameter("@IdRegistro", idRegistro),
                new SqlParameter("@NombreTabla", nombreTabla),
                new SqlParameter("@DVH", dvh)
            );
        }
        public void ActualizarDVH(Guid idRegistro, string nombreTabla, string dvh)
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
        public string ObtenerCodigoVerificador(Guid idUsuario)
        {
            string codigoVerificador = null;

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

            return codigoVerificador;
        }

        public void GuardarDVHCitaDetalle(Guid idCitaDetalle, string nombreTabla, string dvh)
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
        public string ObtenerCodigoVerificadorCitaDetalle(Guid idCitaDetalle)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;
            string codigoVerificador = null;

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

            return codigoVerificador;
        }
    }
}
