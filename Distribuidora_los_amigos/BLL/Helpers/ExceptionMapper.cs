using System;
using BLL.Exceptions;
using DAL;

namespace BLL.Helpers
{
    /// <summary>
    /// Clase auxiliar para convertir excepciones de DAL en excepciones de BLL.
    /// Proporciona un punto centralizado para manejar errores de la capa de datos.
    /// </summary>
    public static class ExceptionMapper
    {
        /// <summary>
        /// Convierte una excepción de DAL en una excepción de BLL apropiada.
        /// </summary>
        /// <param name="dalEx">Excepción de la capa DAL.</param>
        /// <param name="contextMessage">Mensaje de contexto adicional.</param>
        /// <returns>Excepción de negocio correspondiente.</returns>
        public static DatabaseException MapToBusinessException(DALException dalEx, string contextMessage = null)
        {
            string message = contextMessage ?? dalEx.Message;
            DatabaseErrorType errorType;

            switch (dalEx.ErrorType)
            {
                case DALErrorType.ConnectionFailed:
                case DALErrorType.NetworkError:
                    errorType = DatabaseErrorType.ConnectionFailed;
                    return new DatabaseException(message, errorType, dalEx);

                case DALErrorType.Timeout:
                    errorType = DatabaseErrorType.Timeout;
                    return new DatabaseException(message, errorType, dalEx);

                case DALErrorType.Authentication:
                    errorType = DatabaseErrorType.Authentication;
                    return new DatabaseException(message, errorType, dalEx);

                case DALErrorType.DatabaseNotFound:
                    errorType = DatabaseErrorType.DatabaseNotFound;
                    return new DatabaseException(message, errorType, dalEx);

                default:
                    errorType = DatabaseErrorType.Unknown;
                    return new DatabaseException(message, errorType, dalEx);
            }
        }

        /// <summary>
        /// Ejecuta una operación de DAL y convierte excepciones a BLL.
        /// </summary>
        /// <typeparam name="T">Tipo de retorno de la operación.</typeparam>
        /// <param name="operation">Operación a ejecutar.</param>
        /// <param name="contextMessage">Mensaje de contexto para errores.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <exception cref="DatabaseException">Si hay un error de base de datos.</exception>
        public static T ExecuteWithMapping<T>(Func<T> operation, string contextMessage = null)
        {
            try
            {
                return operation();
            }
            catch (DALException dalEx)
            {
                throw MapToBusinessException(dalEx, contextMessage);
            }
        }

        /// <summary>
        /// Ejecuta una operación de DAL sin retorno y convierte excepciones a BLL.
        /// </summary>
        /// <param name="operation">Operación a ejecutar.</param>
        /// <param name="contextMessage">Mensaje de contexto para errores.</param>
        /// <exception cref="DatabaseException">Si hay un error de base de datos.</exception>
        public static void ExecuteWithMapping(Action operation, string contextMessage = null)
        {
            try
            {
                operation();
            }
            catch (DALException dalEx)
            {
                throw MapToBusinessException(dalEx, contextMessage);
            }
        }
    }
}
