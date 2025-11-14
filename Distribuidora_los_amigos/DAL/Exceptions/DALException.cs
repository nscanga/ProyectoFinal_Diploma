using System;
using System.Data.SqlClient;

namespace DAL
{
    /// <summary>
    /// Excepción específica para errores en la capa de acceso a datos.
    /// Permite categorizar errores de SQL Server y proporcionar contexto adicional.
    /// </summary>
    public class DALException : Exception
    {
        /// <summary>
        /// Tipo de error de DAL
        /// </summary>
        public DALErrorType ErrorType { get; set; }

        /// <summary>
        /// Número de error SQL original
        /// </summary>
        public int SqlErrorNumber { get; set; }

        /// <summary>
        /// Comando SQL que causó el error
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        /// Crea una nueva excepción de DAL con el mensaje y tipo indicados.
        /// </summary>
        /// <param name="message">Descripción del error.</param>
        /// <param name="errorType">Tipo de error de DAL.</param>
        /// <param name="innerException">Excepción original.</param>
        /// <param name="sqlErrorNumber">Número de error SQL.</param>
        /// <param name="commandText">Comando SQL que causó el error.</param>
        public DALException(string message, DALErrorType errorType, Exception innerException = null, int sqlErrorNumber = 0, string commandText = null)
            : base(message, innerException)
        {
            ErrorType = errorType;
            SqlErrorNumber = sqlErrorNumber;
            CommandText = commandText;
        }

        /// <summary>
        /// Crea una excepción DAL genérica.
        /// </summary>
        /// <param name="message">Descripción del error.</param>
        /// <param name="innerException">Excepción original.</param>
        public DALException(string message, Exception innerException = null)
            : base(message, innerException)
        {
            ErrorType = DALErrorType.Unknown;
        }

        /// <summary>
        /// Indica si el error es recuperable (la aplicación puede continuar).
        /// </summary>
        public bool IsRecoverable()
        {
            switch (ErrorType)
            {
                case DALErrorType.ConnectionFailed:
                case DALErrorType.Timeout:
                case DALErrorType.NetworkError:
                    return true; // La aplicación puede continuar sin BD temporalmente
                
                case DALErrorType.Authentication:
                case DALErrorType.DatabaseNotFound:
                case DALErrorType.PermissionDenied:
                    return false; // Requiere intervención inmediata
                
                default:
                    return true;
            }
        }
    }

    /// <summary>
    /// Tipos de errores de la capa de acceso a datos
    /// </summary>
    public enum DALErrorType
    {
        /// <summary>No se puede conectar al servidor</summary>
        ConnectionFailed,
        /// <summary>Timeout de conexión o consulta</summary>
        Timeout,
        /// <summary>Error de autenticación</summary>
        Authentication,
        /// <summary>Base de datos no encontrada</summary>
        DatabaseNotFound,
        /// <summary>Error de red</summary>
        NetworkError,
        /// <summary>Violación de restricción (unique, foreign key, not null)</summary>
        ConstraintViolation,
        /// <summary>Deadlock detectado</summary>
        Deadlock,
        /// <summary>Permisos insuficientes</summary>
        PermissionDenied,
        /// <summary>Otro error no categorizado</summary>
        Unknown
    }
}
