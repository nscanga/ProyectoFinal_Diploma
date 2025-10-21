using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Service.DAL.Implementations.SqlServer.Helpers
{
    /// <summary>
    /// Utilidades centralizadas para ejecutar comandos SQL Server utilizando la cadena de conexión del servicio.
    /// </summary>
    internal static class SqlHelper
    {
        public readonly static string conString;

        static SqlHelper()
        {
            conString = ConfigurationManager.ConnectionStrings["MiConexion2"].ConnectionString;
        }

        /// <summary>
        /// Ejecuta un comando que no retorna resultados, devolviendo la cantidad de filas afectadas.
        /// </summary>
        /// <param name="commandText">Consulta o procedimiento a ejecutar.</param>
        /// <param name="commandType">Tipo de comando (texto o procedimiento almacenado).</param>
        /// <param name="parameters">Parámetros a incluir en la ejecución.</param>
        public static Int32 ExecuteNonQuery(String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            CheckNullables(parameters);

            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    // There're three command types: StoredProcedure, Text, TableDirect. The TableDirect 
                    // type is only for OLE DB.  
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Reemplaza valores nulos por <see cref="DBNull.Value"/> para evitar errores al enviar parámetros.
        /// </summary>
        /// <param name="parameters">Colección de parámetros a validar.</param>
        private static void CheckNullables(SqlParameter[] parameters)
        {
            foreach (SqlParameter item in parameters)
            {
                if (item.SqlValue == null)
                {
                    item.SqlValue = DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Configura la conexión y ejecuta el comando devolviendo únicamente el primer valor encontrado.
        /// </summary>
        /// <param name="commandText">Consulta o procedimiento a ejecutar.</param>
        /// <param name="commandType">Tipo de comando.</param>
        /// <param name="parameters">Parámetros opcionales del comando.</param>
        public static Object ExecuteScalar(String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Ejecuta una consulta y retorna un lector de datos manteniendo la conexión abierta hasta su cierre.
        /// </summary>
        /// <param name="commandText">Consulta a ejecutar.</param>
        /// <param name="commandType">Tipo de comando.</param>
        /// <param name="parameters">Parámetros opcionales del comando.</param>
        public static SqlDataReader ExecuteReader(String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(conString);

            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(parameters);

                conn.Open();
                // When using CommandBehavior.CloseConnection, the connection will be closed when the 
                // IDataReader is closed.
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
        }

        /// <summary>
        /// Ejecuta una consulta y devuelve los resultados cargados en un <see cref="DataTable"/>.
        /// </summary>
        /// <param name="commandText">Consulta o procedimiento a ejecutar.</param>
        /// <param name="commandType">Tipo de comando.</param>
        /// <param name="parameters">Parámetros utilizados en la ejecución.</param>
        public static DataTable ExecuteDataTable(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = commandType;

                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);  // Llenar el DataTable con los resultados de la consulta
                        return dt;
                    }
                }
            }
        }
    }
}
