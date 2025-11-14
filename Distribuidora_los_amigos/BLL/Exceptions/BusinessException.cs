using System;

namespace BLL.Exceptions
{
    /// <summary>
    /// Excepción base para errores de negocio en la capa BLL.
    /// </summary>
    public class BusinessException : Exception
    {
        /// <summary>
        /// Código de error del negocio
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Crea una nueva excepción de negocio con el mensaje indicado.
        /// </summary>
        /// <param name="message">Descripción del error.</param>
        /// <param name="errorCode">Código de error opcional.</param>
        public BusinessException(string message, string errorCode = null) : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Crea una nueva excepción de negocio con mensaje y excepción interna.
        /// </summary>
        /// <param name="message">Descripción del error.</param>
        /// <param name="innerException">Excepción original.</param>
        /// <param name="errorCode">Código de error opcional.</param>
        public BusinessException(string message, Exception innerException, string errorCode = null)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
