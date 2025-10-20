using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Contracts;
using DAL.Contratcs;
using DAL.Factory;
using DOMAIN;
using Service.Facade;
using Services.Facade;

namespace BLL
{
    public class StockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IProductoRepository _productoRepository;
        private const int STOCK_MINIMO = 10; // Umbral para notificación
        private const string EMAIL_ADMINISTRADOR = "distribuidoralosamigos@gmail.com"; // Configurar email del admin

        public StockService()
        {
            _stockRepository = FactoryDAL.SqlStockRepository;
            _productoRepository = FactoryDAL.SqlProductoRepository;
        }

        public void AgregarStock(Stock stock)
        {
            _stockRepository.Add(stock);
        }

        /// <summary>
        /// Modifica la cantidad del stock asociado a un producto y verifica si necesita notificación.
        /// </summary>
        public void ModificarStock(Guid idProducto, int cantidad)
        {
            Stock stock = ObtenerStockPorProducto(idProducto);
            if (stock != null)
            {
                stock.Cantidad += cantidad;
                _stockRepository.Update(stock);
                
                // Verificar si el stock es bajo
                VerificarYNotificarStockBajo(idProducto, stock);
            }
        }

        public void EliminarStock(Guid idStock)
        {
            _stockRepository.Remove(idStock);
        }

        /// <summary>
        /// Aumenta la cantidad de stock disponible para un producto.
        /// </summary>
        public void AumentarStock(Guid idProducto, int cantidad)
        {
            Stock stock = _stockRepository.GetByProducto(idProducto).FirstOrDefault();
            if (stock != null)
            {
                stock.Cantidad += cantidad;
                _stockRepository.Update(stock);
                
                // Verificar si el stock sigue bajo después del aumento
                VerificarYNotificarStockBajo(idProducto, stock);
            }
        }

        /// <summary>
        /// Disminuye la cantidad de stock disponible para un producto y notifica si es necesario.
        /// </summary>
        public void DisminuirStock(Guid idProducto, int cantidad)
        {
            Stock stock = _stockRepository.GetByProducto(idProducto).FirstOrDefault();
            if (stock != null && stock.Cantidad >= cantidad)
            {
                stock.Cantidad -= cantidad;
                _stockRepository.Update(stock);
                
                // Verificar si el stock quedó bajo después de disminuir
                VerificarYNotificarStockBajo(idProducto, stock);
            }
        }

        public List<Stock> ObtenerStock()
        {
            return _stockRepository.GetAll();
        }

        /// <summary>
        /// Obtiene el stock con información completa del producto para mostrar en la UI
        /// </summary>
        public List<StockDTO> ObtenerStockConDetalles()
        {
            List<StockDTO> listaStockDTO = new List<StockDTO>();
            
            try
            {
                // Obtener todos los stocks
                List<Stock> stocks = _stockRepository.GetAll();
                
                // Obtener todos los productos
                List<Producto> productos = _productoRepository.GetAll();
                
                // Combinar la información usando LINQ
                var stockConDetalles = from stock in stocks
                                       join producto in productos 
                                       on stock.IdProducto equals producto.IdProducto
                                       select new StockDTO
                                       {
                                           IdStock = stock.IdStock,
                                           IdProducto = stock.IdProducto,
                                           NombreProducto = producto.Nombre,
                                           Categoria = producto.Categoria,
                                           Cantidad = stock.Cantidad,
                                           Tipo = stock.Tipo,
                                           PrecioUnitario = producto.Precio,
                                           Activo = stock.Activo
                                       };
                
                listaStockDTO = stockConDetalles.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener stock con detalles: {ex.Message}");
                LoggerService.WriteException(ex);
            }
            
            return listaStockDTO;
        }

        /// <summary>
        /// Devuelve el stock asociado a un producto (toma el primer resultado encontrado).
        /// </summary>
        public Stock ObtenerStockPorProducto(Guid idProducto)
        {
            return _stockRepository.GetByProducto(idProducto).FirstOrDefault();
        }

        /// <summary>
        /// Verifica si el stock está bajo el umbral y envía notificación por email.
        /// </summary>
        private void VerificarYNotificarStockBajo(Guid idProducto, Stock stock)
        {
            try
            {
                if (stock.Cantidad < STOCK_MINIMO)
                {
                    // Obtener información del producto
                    Producto producto = _productoRepository.GetById(idProducto);
                    
                    if (producto != null)
                    {
                        Console.WriteLine($"⚠️ ALERTA: Stock bajo detectado para '{producto.Nombre}' - Cantidad: {stock.Cantidad}");
                        
                        // Enviar notificación por email
                        EmailService.EnviarNotificacionStockBajo(producto, stock, EMAIL_ADMINISTRADOR);
                        
                        Console.WriteLine($"✅ Notificación de stock bajo enviada para '{producto.Nombre}'");
                    }
                }
            }
            catch (Exception ex)
            {
                // Registrar el error pero no detener el proceso
                Console.WriteLine($"❌ Error al verificar/notificar stock bajo: {ex.Message}");
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Obtiene una lista de productos con stock bajo (menor al umbral).
        /// </summary>
        public List<Stock> ObtenerProductosConStockBajo()
        {
            return _stockRepository.GetAll()
                .Where(s => s.Cantidad < STOCK_MINIMO)
                .ToList();
        }
    }
}
