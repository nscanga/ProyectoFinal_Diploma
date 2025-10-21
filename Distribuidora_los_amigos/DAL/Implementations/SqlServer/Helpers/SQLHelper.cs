using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DAL
{
    /// <summary>
    /// Proporciona utilidades comunes para ejecutar comandos SQL sobre SQL Server.
    /// </summary>
    internal static class SqlHelper
    {
        /// <summary>
        /// Cadena de conexión utilizada por los métodos auxiliares.
        /// </summary>
        public readonly static string conString;

        /// <summary>
        /// Inicializa los valores estáticos resolviendo la cadena de conexión configurada.
        /// </summary>
        static SqlHelper()
        {
            conString = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;
        }

        /// <summary>
        /// Ejecuta un comando que no devuelve resultados (INSERT, UPDATE, DELETE).
        /// </summary>
        /// <param name="commandText">Texto del comando a ejecutar.</param>
        /// <param name="commandType">Tipo de comando (texto, procedimiento almacenado, etc.).</param>
        /// <param name="parameters">Parámetros de entrada para el comando.</param>
        /// <returns>Cantidad de filas afectadas.</returns>
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
        /// Reemplaza valores nulos en parámetros por <see cref="DBNull.Value"/>.
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
        /// Set the connection, command, and then execute the command and only return one value.
        /// </summary>
        /// <param name="commandText">Texto del comando a ejecutar.</param>
        /// <param name="commandType">Tipo de comando.</param>
        /// <param name="parameters">Parámetros del comando.</param>
        /// <returns>Valor escalar devuelto por la consulta.</returns>
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
        /// Set the connection, command, and then execute the command with query and return the reader.
        /// </summary>
        /// <param name="commandText">Texto del comando a ejecutar.</param>
        /// <param name="commandType">Tipo de comando.</param>
        /// <param name="parameters">Parámetros del comando.</param>
        /// <returns>Lector de datos asociado al resultado.</returns>
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
        /// Ejecuta una consulta y materializa el resultado en un <see cref="DataTable"/>.
        /// </summary>
        /// <param name="commandText">Texto del comando a ejecutar.</param>
        /// <param name="commandType">Tipo de comando.</param>
        /// <param name="parameters">Parámetros del comando.</param>
        /// <returns>Tabla con los registros devueltos por la consulta.</returns>
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

