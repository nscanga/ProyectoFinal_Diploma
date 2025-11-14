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
        /// <exception cref="DALException">Se lanza cuando hay un error de base de datos.</exception>
        public static Int32 ExecuteNonQuery(String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            CheckNullables(parameters);

            try
            {
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
            catch (SqlException sqlEx)
            {
                throw HandleSqlException(sqlEx, commandText);
            }
            catch (Exception ex)
            {
                throw new DALException("Error inesperado al ejecutar comando SQL.", ex);
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
        /// <exception cref="DALException">Se lanza cuando hay un error de base de datos.</exception>
        public static Object ExecuteScalar(String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            try
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
            catch (SqlException sqlEx)
            {
                throw HandleSqlException(sqlEx, commandText);
            }
            catch (Exception ex)
            {
                throw new DALException("Error inesperado al ejecutar comando SQL.", ex);
            }
        }

        /// <summary>
        /// Set the connection, command, and then execute the command with query and return the reader.
        /// </summary>
        /// <param name="commandText">Texto del comando a ejecutar.</param>
        /// <param name="commandType">Tipo de comando.</param>
        /// <param name="parameters">Parámetros del comando.</param>
        /// <returns>Lector de datos asociado al resultado.</returns>
        /// <exception cref="DALException">Se lanza cuando hay un error de base de datos.</exception>
        public static SqlDataReader ExecuteReader(String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(conString);

            try
            {
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
            catch (SqlException sqlEx)
            {
                conn?.Dispose();
                throw HandleSqlException(sqlEx, commandText);
            }
            catch (Exception ex)
            {
                conn?.Dispose();
                throw new DALException("Error inesperado al ejecutar comando SQL.", ex);
            }
        }

        /// <summary>
        /// Ejecuta una consulta y materializa el resultado en un <see cref="DataTable"/>.
        /// </summary>
        /// <param name="commandText">Texto del comando a ejecutar.</param>
        /// <param name="commandType">Tipo de comando.</param>
        /// <param name="parameters">Parámetros del comando.</param>
        /// <returns>Tabla con los registros devueltos por la consulta.</returns>
        /// <exception cref="DALException">Se lanza cuando hay un error de base de datos.</exception>
        public static DataTable ExecuteDataTable(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            try
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
            catch (SqlException sqlEx)
            {
                throw HandleSqlException(sqlEx, commandText);
            }
            catch (Exception ex)
            {
                throw new DALException("Error inesperado al ejecutar comando SQL.", ex);
            }
        }

        /// <summary>
        /// Convierte SqlException en DALException categorizada según el error.
        /// </summary>
        /// <param name="sqlEx">Excepción SQL original.</param>
        /// <param name="commandText">Comando que generó el error.</param>
        /// <returns>Excepción DAL personalizada.</returns>
        private static DALException HandleSqlException(SqlException sqlEx, string commandText)
        {
            DALErrorType errorType;
            string message;

            switch (sqlEx.Number)
            {
                case -1: // Timeout
                case -2:
                    errorType = DALErrorType.Timeout;
                    message = "Se agotó el tiempo de espera al conectar con la base de datos.";
                    break;

                case 2: // Error de red - El equipo remoto rechazó la conexión
                case 10053: // Error de red
                case 10054: // Conexión interrumpida
                case 10060: // Timeout de conexión
                case 10061: // Conexión rechazada
                    errorType = DALErrorType.ConnectionFailed;
                    message = "No se puede establecer conexión con el servidor de base de datos. Verifique que el servidor esté disponible.";
                    break;

                case 18456: // Error de autenticación
                    errorType = DALErrorType.Authentication;
                    message = "Fallo en la autenticación con el servidor de base de datos. Verifique las credenciales de conexión.";
                    break;

                case 4060: // No se puede abrir la base de datos
                    errorType = DALErrorType.DatabaseNotFound;
                    message = "No se puede abrir la base de datos. Verifique que la base de datos exista y esté accesible.";
                    break;

                case 2627: // Violación de Clave Única
                    errorType = DALErrorType.ConstraintViolation;
                    message = "Violación de restricción única. El registro ya existe.";
                    break;

                case 547: // Violación de Clave Foránea
                    errorType = DALErrorType.ConstraintViolation;
                    message = "Violación de restricción de clave foránea.";
                    break;

                case 515: // Violación de Restricción de Null
                    errorType = DALErrorType.ConstraintViolation;
                    message = "Se intentó insertar un valor NULL en una columna que no permite valores NULL.";
                    break;

                case 1205: // Deadlock
                    errorType = DALErrorType.Deadlock;
                    message = "La transacción ha fallado debido a un bloqueo (deadlock).";
                    break;

                case 229: // Permisos insuficientes
                    errorType = DALErrorType.PermissionDenied;
                    message = "El usuario no tiene permisos suficientes para realizar esta operación.";
                    break;

                default:
                    errorType = DALErrorType.Unknown;
                    message = "Error al ejecutar la operación en la base de datos.";
                    break;
            }

            return new DALException(message, errorType, sqlEx, sqlEx.Number, commandText);
        }
    }
}

