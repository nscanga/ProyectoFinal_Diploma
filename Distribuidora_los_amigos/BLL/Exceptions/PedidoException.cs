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

        /// <summary>
        /// Indica que se intentó procesar un pedido nulo.
        /// </summary>
        public static PedidoException PedidoNulo()
        {
            return new PedidoException("El pedido no puede ser nulo.", "PEDIDO_NULO");
        }

        /// <summary>
        /// Indica que el pedido debe tener un cliente asignado.
        /// </summary>
        public static PedidoException ClienteRequerido()
        {
            return new PedidoException("El pedido debe estar asociado a un cliente válido.", "CLIENTE_REQUERIDO");
        }

        /// <summary>
        /// Indica que el cliente especificado no existe en la base de datos.
        /// </summary>
        public static PedidoException ClienteNoExiste(Guid idCliente)
        {
            return new PedidoException($"El cliente con ID {idCliente} no existe.", "CLIENTE_NO_EXISTE");
        }

        /// <summary>
        /// Indica que el pedido debe tener un estado asignado.
        /// </summary>
        public static PedidoException EstadoRequerido()
        {
            return new PedidoException("El pedido debe tener un estado válido.", "ESTADO_REQUERIDO");
        }

        /// <summary>
        /// Indica que el estado especificado no existe en el catálogo.
        /// </summary>
        public static PedidoException EstadoNoExiste(Guid idEstado)
        {
            return new PedidoException($"El estado con ID {idEstado} no existe.", "ESTADO_NO_EXISTE");
        }

        /// <summary>
        /// Indica que la fecha del pedido no es válida.
        /// </summary>
        public static PedidoException FechaInvalida(DateTime fecha)
        {
            return new PedidoException($"La fecha del pedido ({fecha:dd/MM/yyyy}) no puede ser en el futuro.", "FECHA_INVALIDA");
        }

        /// <summary>
        /// Indica que el pedido no contiene productos.
        /// </summary>
        public static PedidoException PedidoVacio()
        {
            return new PedidoException("El pedido debe contener al menos un producto.", "PEDIDO_VACIO");
        }

        /// <summary>
        /// Indica que la cantidad especificada no es válida.
        /// </summary>
        public static PedidoException CantidadInvalida(int cantidad)
        {
            return new PedidoException($"La cantidad ({cantidad}) debe ser mayor a cero.", "CANTIDAD_INVALIDA");
        }

        /// <summary>
        /// Indica que el precio no es válido.
        /// </summary>
        public static PedidoException PrecioInvalido(decimal precio)
        {
            return new PedidoException($"El precio ({precio:C2}) no puede ser negativo.", "PRECIO_INVALIDO");
        }

        /// <summary>
        /// Indica que el producto es requerido en el detalle del pedido.
        /// </summary>
        public static PedidoException ProductoRequerido()
        {
            return new PedidoException("Cada detalle del pedido debe tener un producto asignado.", "PRODUCTO_REQUERIDO");
        }
    }
}
