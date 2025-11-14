using System;

namespace BLL.Exceptions
{
    /// <summary>
    /// Excepción específica para errores relacionados con pedidos.
    /// </summary>
    public class PedidoException : BusinessException
    {
        /// <summary>
        /// Crea una nueva excepción de pedido con el mensaje indicado.
        /// </summary>
        /// <param name="message">Descripción del error.</param>
        /// <param name="errorCode">Código de error opcional.</param>
        public PedidoException(string message, string errorCode = null) : base(message, errorCode) { }

        /// <summary>
        /// Construye una excepción que indica que el pedido no fue encontrado.
        /// </summary>
        public static PedidoException PedidoNoEncontrado(int pedidoId)
        {
            return new PedidoException($"No se encontró el pedido con ID: {pedidoId}", "PEDIDO_NOT_FOUND");
        }

        /// <summary>
        /// Construye una excepción que indica que el pedido tiene stock insuficiente.
        /// </summary>
        public static PedidoException StockInsuficiente(string productoNombre)
        {
            return new PedidoException($"Stock insuficiente para el producto: {productoNombre}", "STOCK_INSUFICIENTE");
        }

        /// <summary>
        /// Construye una excepción que indica que el pedido no puede ser modificado.
        /// </summary>
        public static PedidoException PedidoNoModificable(string estado)
        {
            return new PedidoException($"No se puede modificar un pedido en estado: {estado}", "PEDIDO_NO_MODIFICABLE");
        }
    }
}
