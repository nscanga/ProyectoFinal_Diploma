using System;

namespace DOMAIN
{
    /// <summary>
    /// DTO para mostrar información combinada de DetallePedido y Producto
    /// </summary>
    public class DetallePedidoDTO
    {
        public Guid IdDetallePedido { get; set; }
        public Guid IdPedido { get; set; }
        public Guid IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Categoria { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        public DetallePedidoDTO()
        {
        }
    }
}