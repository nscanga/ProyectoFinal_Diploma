using System;

namespace BLL.Exceptions
{
    /// <summary>
    /// Excepción específica para errores relacionados con proveedores.
    /// </summary>
    public class ProveedorException : BusinessException
    {
        /// <summary>
        /// Crea una nueva excepción de proveedor con el mensaje indicado.
        /// </summary>
        /// <param name="message">Descripción del error.</param>
        /// <param name="errorCode">Código de error opcional.</param>
        public ProveedorException(string message, string errorCode = null) : base(message, errorCode) { }

        /// <summary>
        /// Indica que se intentó procesar un proveedor nulo.
        /// </summary>
        public static ProveedorException ProveedorNulo()
        {
            return new ProveedorException("El proveedor no puede ser nulo.", "PROVEEDOR_NULO");
        }

        /// <summary>
        /// Indica que el nombre del proveedor es obligatorio.
        /// </summary>
        public static ProveedorException NombreRequerido()
        {
            return new ProveedorException("El nombre del proveedor es obligatorio.", "NOMBRE_REQUERIDO");
        }

        /// <summary>
        /// Indica que la dirección del proveedor es obligatoria.
        /// </summary>
        public static ProveedorException DireccionRequerida()
        {
            return new ProveedorException("La dirección del proveedor es obligatoria.", "DIRECCION_REQUERIDA");
        }

        /// <summary>
        /// Indica que el email del proveedor es obligatorio.
        /// </summary>
        public static ProveedorException EmailRequerido()
        {
            return new ProveedorException("El email del proveedor es obligatorio.", "EMAIL_REQUERIDO");
        }

        /// <summary>
        /// Indica que el email no es válido.
        /// </summary>
        public static ProveedorException EmailInvalido(string email)
        {
            return new ProveedorException(
                $"El email '{email}' no es válido. Debe tener formato correcto (ejemplo@dominio.com).",
                "EMAIL_INVALIDO");
        }

        /// <summary>
        /// Indica que el teléfono del proveedor es obligatorio.
        /// </summary>
        public static ProveedorException TelefonoRequerido()
        {
            return new ProveedorException("El teléfono del proveedor es obligatorio.", "TELEFONO_REQUERIDO");
        }

        /// <summary>
        /// Indica que el teléfono no es válido.
        /// </summary>
        public static ProveedorException TelefonoInvalido(string telefono)
        {
            return new ProveedorException(
                $"El teléfono '{telefono}' no es válido. Debe contener al menos 10 dígitos.",
                "TELEFONO_INVALIDO");
        }

        /// <summary>
        /// Indica que la categoría del proveedor es obligatoria.
        /// </summary>
        public static ProveedorException CategoriaRequerida()
        {
            return new ProveedorException("La categoría del proveedor es obligatoria.", "CATEGORIA_REQUERIDA");
        }

        /// <summary>
        /// Indica que el proveedor no fue encontrado.
        /// </summary>
        public static ProveedorException ProveedorNoEncontrado(Guid proveedorId)
        {
            return new ProveedorException(
                $"No se encontró el proveedor con ID: {proveedorId}",
                "PROVEEDOR_NOT_FOUND");
        }

        /// <summary>
        /// Construye una excepción que indica que los campos del proveedor no son válidos.
        /// </summary>
        public static ProveedorException CamposInvalidos()
        {
            return new ProveedorException("Uno o más campos del proveedor son inválidos.", "CAMPOS_INVALIDOS");
        }
    }
}
