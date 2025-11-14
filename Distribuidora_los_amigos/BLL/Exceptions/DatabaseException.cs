using System;
using System.Data.SqlClient;

namespace BLL.Exceptions
{
    /// <summary>
    /// Excepción específica para errores relacionados con la base de datos.
    /// Permite que la aplicación continúe ejecutándose incluso si hay problemas de conexión.
    /// </summary>
    public class DatabaseException : Exception
    {
        /// <summary>
        /// Tipo de error de base de datos
        /// </summary>
        public DatabaseErrorType ErrorType { get; set; }

        /// <summary>
        /// Número de error SQL original
        /// </summary>
        public int SqlErrorNumber { get; set; }

        /// <summary>
        /// Crea una nueva excepción de base de datos con el mensaje y tipo indicados.
        /// </summary>
        /// <param name="message">Descripción del error.</param>
        /// <param name="errorType">Tipo de error de base de datos.</param>
        /// <param name="innerException">Excepción original.</param>
        public DatabaseException(string message, DatabaseErrorType errorType, Exception innerException = null)
            : base(message, innerException)
        {
            ErrorType = errorType;
            if (innerException is SqlException sqlEx)
            {
                SqlErrorNumber = sqlEx.Number;
            }
        }

        /// <summary>
        /// Construye una excepción que indica que no se puede conectar a la base de datos.
        /// </summary>
        public static DatabaseException ConnectionFailed(Exception innerException)
        {
            return new DatabaseException(
                "No se puede establecer conexión con la base de datos. El servicio no está disponible o la red presenta problemas.",
                DatabaseErrorType.ConnectionFailed,
                innerException);
        }

        /// <summary>
        /// Construye una excepción que indica timeout en la conexión.
        /// </summary>
        public static DatabaseException ConnectionTimeout(Exception innerException)
        {
            return new DatabaseException(
                "Se agotó el tiempo de espera al conectar con la base de datos.",
                DatabaseErrorType.Timeout,
                innerException);
        }

        /// <summary>
        /// Construye una excepción que indica error de autenticación.
        /// </summary>
        public static DatabaseException AuthenticationFailed(Exception innerException)
        {
            return new DatabaseException(
                "Fallo en la autenticación con el servidor de base de datos.",
                DatabaseErrorType.Authentication,
                innerException);
        }

        /// <summary>
        /// Construye una excepción que indica que la base de datos no existe.
        /// </summary>
        public static DatabaseException DatabaseNotFound(Exception innerException)
        {
            return new DatabaseException(
                "No se puede abrir la base de datos. Verifique que la base de datos exista y esté accesible.",
                DatabaseErrorType.DatabaseNotFound,
                innerException);
        }
    }

    /// <summary>
    /// Tipos de errores de base de datos
    /// </summary>
    public enum DatabaseErrorType
    {
        /// <summary>No se puede conectar al servidor</summary>
        ConnectionFailed,
        /// <summary>Timeout de conexión</summary>
        Timeout,
        /// <summary>Error de autenticación</summary>
        Authentication,
        /// <summary>Base de datos no encontrada</summary>
        DatabaseNotFound,
        /// <summary>Error de red</summary>
        NetworkError,
        /// <summary>Otro error no categorizado</summary>
        Unknown
    }
}
