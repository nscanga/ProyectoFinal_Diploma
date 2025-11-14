using System;

namespace BLL.Exceptions
{
    /// <summary>
    /// Excepción específica para errores relacionados con clientes.
    /// </summary>
    public class ClienteException : BusinessException
    {
        /// <summary>
        /// Crea una nueva excepción de cliente con el mensaje indicado.
        /// </summary>
        /// <param name="message">Descripción del error.</param>
        /// <param name="errorCode">Código de error opcional.</param>
        public ClienteException(string message, string errorCode = null) : base(message, errorCode) { }

        /// <summary>
        /// Construye una excepción que indica que el cliente ya existe.
        /// </summary>
        public static ClienteException ClienteYaExiste(string cuit)
        {
            return new ClienteException($"Ya existe un cliente con CUIT: {cuit}", "CLIENTE_DUPLICADO");
        }

        /// <summary>
        /// Construye una excepción que indica que el cliente no fue encontrado.
        /// </summary>
        public static ClienteException ClienteNoEncontrado(int clienteId)
        {
            return new ClienteException($"No se encontró el cliente con ID: {clienteId}", "CLIENTE_NOT_FOUND");
        }

        /// <summary>
        /// Construye una excepción que indica que los campos del cliente no son válidos.
        /// </summary>
        public static ClienteException CamposInvalidos()
        {
            return new ClienteException("Uno o más campos del cliente son inválidos.", "CAMPOS_INVALIDOS");
        }
    }
}
