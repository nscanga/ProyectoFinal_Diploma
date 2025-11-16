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
    public class ProductoException : BusinessException
    {
        /// <summary>
        /// Crea una nueva excepción de producto con el mensaje indicado.
        /// </summary>
        /// <param name="message">Descripción del error.</param>
        /// <param name="errorCode">Código de error opcional.</param>
        public ProductoException(string message, string errorCode = null) : base(message, errorCode) { }

        /// <summary>
        /// Construye una excepción que indica que los campos del producto no son válidos.
        /// </summary>
        /// <returns>Instancia de <see cref="ProductoException"/> con el mensaje predefinido.</returns>
        public static ProductoException CamposInvalidos()
        {
            return new ProductoException("Uno o más campos del producto son inválidos.", "CAMPOS_INVALIDOS");
        }

        /// <summary>
        /// Indica que se intentó procesar un producto nulo.
        /// </summary>
        public static ProductoException ProductoNulo()
        {
            return new ProductoException("El producto no puede ser nulo.", "PRODUCTO_NULO");
        }

        /// <summary>
        /// Indica que el nombre del producto es obligatorio.
        /// </summary>
        public static ProductoException NombreRequerido()
        {
            return new ProductoException("El nombre del producto es obligatorio.", "NOMBRE_REQUERIDO");
        }

        /// <summary>
        /// Indica que la categoría del producto es obligatoria.
        /// </summary>
        public static ProductoException CategoriaRequerida()
        {
            return new ProductoException("La categoría del producto es obligatoria.", "CATEGORIA_REQUERIDA");
        }

        /// <summary>
        /// Indica que el precio del producto no es válido.
        /// </summary>
        public static ProductoException PrecioInvalido(decimal precio)
        {
            return new ProductoException($"El precio ({precio:C2}) debe ser mayor a cero.", "PRECIO_INVALIDO");
        }

        /// <summary>
        /// Indica que la fecha de ingreso es obligatoria.
        /// </summary>
        public static ProductoException FechaIngresoRequerida()
        {
            return new ProductoException("La fecha de ingreso del producto es obligatoria.", "FECHA_INGRESO_REQUERIDA");
        }

        /// <summary>
        /// Indica que la fecha de ingreso no puede ser futura.
        /// </summary>
        public static ProductoException FechaIngresoInvalida(DateTime fechaIngreso)
        {
            return new ProductoException($"La fecha de ingreso ({fechaIngreso:dd/MM/yyyy}) no puede ser en el futuro.", "FECHA_INGRESO_INVALIDA");
        }

        /// <summary>
        /// Indica que la fecha de vencimiento no es válida respecto a la fecha de ingreso.
        /// </summary>
        public static ProductoException VencimientoInvalido(DateTime vencimiento, DateTime fechaIngreso)
        {
            return new ProductoException(
                $"La fecha de vencimiento ({vencimiento:dd/MM/yyyy}) no puede ser anterior a la fecha de ingreso ({fechaIngreso:dd/MM/yyyy}).",
                "VENCIMIENTO_INVALIDO");
        }

        /// <summary>
        /// Indica que la cantidad inicial de stock no es válida.
        /// </summary>
        public static ProductoException CantidadInvalida(int cantidad)
        {
            return new ProductoException($"La cantidad inicial ({cantidad}) no puede ser negativa.", "CANTIDAD_INVALIDA");
        }

        /// <summary>
        /// Indica que el tipo de stock es requerido cuando se especifica cantidad.
        /// </summary>
        public static ProductoException TipoStockRequerido()
        {
            return new ProductoException("El tipo de stock es requerido cuando se especifica una cantidad inicial.", "TIPO_STOCK_REQUERIDO");
        }

        /// <summary>
        /// Indica que ya existe un producto con ese identificador o nombre.
        /// </summary>
        public static ProductoException ProductoDuplicado(string nombre)
        {
            return new ProductoException($"Ya existe un producto con el nombre '{nombre}'.", "PRODUCTO_DUPLICADO");
        }

        /// <summary>
        /// Indica que el producto no fue encontrado.
        /// </summary>
        public static ProductoException ProductoNoEncontrado(Guid idProducto)
        {
            return new ProductoException($"No se encontró el producto con ID {idProducto}.", "PRODUCTO_NO_ENCONTRADO");
        }
    }
}
