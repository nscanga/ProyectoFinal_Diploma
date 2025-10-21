using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    /// <summary>
    /// Excepción específica para errores relacionados con entidades de producto.
    /// </summary>
    public class ProductoException : Exception
    {
        /// <summary>
        /// Crea una nueva excepción de producto con el mensaje indicado.
        /// </summary>
        /// <param name="message">Descripción del error.</param>
        public ProductoException(string message) : base(message) { }

        /// <summary>
        /// Construye una excepción que indica que los campos del producto no son válidos.
        /// </summary>
        /// <returns>Instancia de <see cref="ProductoException"/> con el mensaje predefinido.</returns>
        public static ProductoException CamposInvalidos()
        {
            return new ProductoException("Uno o más campos del producto son inválidos.");
        }
    }
}
