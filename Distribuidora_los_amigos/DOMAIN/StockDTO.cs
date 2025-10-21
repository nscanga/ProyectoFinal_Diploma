using System;

namespace DOMAIN
{
    /// <summary>
    /// DTO para mostrar información combinada de Stock y Producto
    /// </summary>
    public class StockDTO
    {
        public Guid IdStock { get; set; }
        public Guid IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Categoria { get; set; }
        public int Cantidad { get; set; }
        public string Tipo { get; set; }
        public decimal PrecioUnitario { get; set; }
        public bool Activo { get; set; }

        /// <summary>
        /// Permite instanciar el DTO antes de completar los datos combinados de stock y producto.
        /// </summary>
        public StockDTO()
        {
        }
    }
}
