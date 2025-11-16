using System;

namespace BLL.Exceptions
{
    /// <summary>
    /// Excepción específica para errores relacionados con stock.
    /// </summary>
    public class StockException : BusinessException
    {
        /// <summary>
        /// Crea una nueva excepción de stock con el mensaje indicado.
        /// </summary>
        /// <param name="message">Descripción del error.</param>
        /// <param name="errorCode">Código de error opcional.</param>
        public StockException(string message, string errorCode = null) : base(message, errorCode) { }

        /// <summary>
        /// Construye una excepción que indica que no hay stock disponible.
        /// </summary>
        public static StockException StockInsuficiente(Guid productoId, int cantidadRequerida, int cantidadDisponible)
        {
            return new StockException(
                $"Stock insuficiente para el producto ID {productoId}. Requerido: {cantidadRequerida}, Disponible: {cantidadDisponible}",
                "STOCK_INSUFICIENTE");
        }

        /// <summary>
        /// Construye una excepción que indica que el stock está por debajo del mínimo.
        /// </summary>
        public static StockException StockBajoMinimo(Guid productoId, int stockActual, int stockMinimo)
        {
            return new StockException(
                $"El stock del producto ID {productoId} está por debajo del mínimo. Actual: {stockActual}, Mínimo: {stockMinimo}",
                "STOCK_BAJO_MINIMO");
        }

        /// <summary>
        /// Construye una excepción que indica que no existe registro de stock para el producto.
        /// </summary>
        public static StockException StockNoExiste(Guid productoId)
        {
            return new StockException(
                $"No existe registro de stock para el producto con ID {productoId}.",
                "STOCK_NO_EXISTE");
        }
    }
}
