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
        /// Indica que se intentó procesar un cliente nulo.
        /// </summary>
        public static ClienteException ClienteNulo()
        {
            return new ClienteException("El cliente no puede ser nulo.", "CLIENTE_NULO");
        }

        /// <summary>
        /// Indica que el nombre del cliente es obligatorio.
        /// </summary>
        public static ClienteException NombreRequerido()
        {
            return new ClienteException("El nombre del cliente es obligatorio.", "NOMBRE_REQUERIDO");
        }

        /// <summary>
        /// Indica que la dirección del cliente es obligatoria.
        /// </summary>
        public static ClienteException DireccionRequerida()
        {
            return new ClienteException("La dirección del cliente es obligatoria.", "DIRECCION_REQUERIDA");
        }

        /// <summary>
        /// Indica que el teléfono del cliente es obligatorio.
        /// </summary>
        public static ClienteException TelefonoRequerido()
        {
            return new ClienteException("El teléfono del cliente es obligatorio.", "TELEFONO_REQUERIDO");
        }

        /// <summary>
        /// Indica que el teléfono no es válido.
        /// </summary>
        public static ClienteException TelefonoInvalido(string telefono)
        {
            return new ClienteException(
                $"El teléfono '{telefono}' no es válido. Debe contener al menos 10 dígitos y no puede ser un email.",
                "TELEFONO_INVALIDO");
        }

        /// <summary>
        /// Indica que el email del cliente es obligatorio.
        /// </summary>
        public static ClienteException EmailRequerido()
        {
            return new ClienteException("El email del cliente es obligatorio.", "EMAIL_REQUERIDO");
        }

        /// <summary>
        /// Indica que el email no es válido.
        /// </summary>
        public static ClienteException EmailInvalido(string email)
        {
            return new ClienteException(
                $"El email '{email}' no es válido. Debe tener formato correcto (ejemplo@dominio.com) y no puede ser un número de teléfono.",
                "EMAIL_INVALIDO");
        }

        /// <summary>
        /// Indica que el CUIT del cliente es obligatorio.
        /// </summary>
        public static ClienteException CUITRequerido()
        {
            return new ClienteException("El CUIT del cliente es obligatorio.", "CUIT_REQUERIDO");
        }

        /// <summary>
        /// Indica que el CUIT no es válido.
        /// </summary>
        public static ClienteException CUITInvalido(string cuit)
        {
            return new ClienteException(
                $"El CUIT '{cuit}' no es válido. Debe contener 11 dígitos.",
                "CUIT_INVALIDO");
        }

        /// <summary>
        /// Indica que ya existe un cliente con ese CUIT.
        /// </summary>
        public static ClienteException ClienteDuplicado(string cuit)
        {
            return new ClienteException(
                $"Ya existe un cliente registrado con el CUIT '{cuit}'.",
                "CLIENTE_DUPLICADO");
        }

        /// <summary>
        /// Construye una excepción que indica que el cliente ya existe (legacy).
        /// </summary>
        [Obsolete("Usar ClienteDuplicado() en su lugar")]
        public static ClienteException ClienteYaExiste(string cuit)
        {
            return ClienteDuplicado(cuit);
        }

        /// <summary>
        /// Construye una excepción que indica que el cliente no fue encontrado.
        /// </summary>
        public static ClienteException ClienteNoEncontrado(Guid clienteId)
        {
            return new ClienteException(
                $"No se encontró el cliente con ID: {clienteId}",
                "CLIENTE_NOT_FOUND");
        }

        /// <summary>
        /// Construye una excepción que indica que el cliente no fue encontrado (sobrecarga legacy).
        /// </summary>
        [Obsolete("Usar sobrecarga con Guid")]
        public static ClienteException ClienteNoEncontrado(int clienteId)
        {
            return new ClienteException(
                $"No se encontró el cliente con ID: {clienteId}",
                "CLIENTE_NOT_FOUND");
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
