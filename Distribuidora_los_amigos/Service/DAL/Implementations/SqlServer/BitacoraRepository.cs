using Service.DAL.Contracts;
using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Implementations.SqlServer
{
    /// <summary>
    /// Repositorio encargado de consultar los registros de la bitácora almacenada en la base de datos.
    /// </summary>
    public class BitacoraRepository : IBitacoraRepository
    {
        private static string ConnectionString => ConfigurationManager.ConnectionStrings["LogDatabase"].ConnectionString;

        /// <summary>
        /// Recupera todos los registros de la bitácora con su detalle asociado.
        /// </summary>
        /// <returns>Listado completo de entradas de bitácora.</returns>
        public List<Bitacora> GetAll()
        {
            List<Bitacora> bitacoras = new List<Bitacora>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT Id_Log, Date, TraceLevel, Message, StackTrace FROM Log";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Bitacora bitacora = new Bitacora
                            {
                                Id_Log = reader.GetGuid(0),
                                Fecha = reader.GetDateTime(1),
                                TraceLevel = reader.GetString(2),
                                Mensaje = reader.GetString(3),
                                StackTrace = reader.IsDBNull(4) ? null : reader.GetString(4)
                            };
                            bitacoras.Add(bitacora);
                        }
                    }
                }
            }

            return bitacoras;
        }

       
    }
}
