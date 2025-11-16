using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Exceptions;
using BLL.Helpers;
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
        private const string EMAIL_ADMINISTRADOR = "nicolas.scanga@hotmail.com"; // Configurar email del admin

        /// <summary>
        /// Inicializa el servicio resolviendo los repositorios de stock y productos necesarios.
        /// </summary>
        public StockService()
        {
            _stockRepository = FactoryDAL.SqlStockRepository;
            _productoRepository = FactoryDAL.SqlProductoRepository;
        }

        /// <summary>
        /// Agrega un nuevo registro de stock al repositorio.
        /// </summary>
        /// <param name="stock">Entidad de stock que se almacenará.</param>
        public void AgregarStock(Stock stock)
        {
            try
            {
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    _stockRepository.Add(stock);
                }, "Error al agregar stock");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"❌ No se puede agregar stock sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Modifica la cantidad del stock asociado a un producto y verifica si necesita notificación.
        /// </summary>
        public void ModificarStock(Guid idProducto, int cantidad)
        {
            try
            {
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    Stock stock = ObtenerStockPorProducto(idProducto);
                    if (stock != null)
                    {
                        stock.Cantidad += cantidad;
                        _stockRepository.Update(stock);
                        
                        // Verificar si el stock es bajo
                        VerificarYNotificarStockBajo(idProducto, stock);
                    }
                }, "Error al modificar stock");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"❌ No se puede modificar stock sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro de stock específico.
        /// </summary>
        /// <param name="idStock">Identificador del stock a remover.</param>
        public void EliminarStock(Guid idStock)
        {
            try
            {
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    _stockRepository.Remove(idStock);
                }, "Error al eliminar stock");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"❌ No se puede eliminar stock sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Aumenta la cantidad de stock disponible para un producto.
        /// </summary>
        public void AumentarStock(Guid idProducto, int cantidad)
        {
            try
            {
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    Stock stock = _stockRepository.GetByProducto(idProducto).FirstOrDefault();
                    if (stock != null)
                    {
                        stock.Cantidad += cantidad;
                        _stockRepository.Update(stock);
                        
                        // Verificar si el stock sigue bajo después del aumento
                        VerificarYNotificarStockBajo(idProducto, stock);
                    }
                }, "Error al aumentar stock");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"❌ No se puede aumentar stock sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Disminuye la cantidad de stock disponible para un producto y notifica si es necesario.
        /// </summary>
        public void DisminuirStock(Guid idProducto, int cantidad)
        {
            try
            {
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    Stock stock = _stockRepository.GetByProducto(idProducto).FirstOrDefault();
                    if (stock != null)
                    {
                        if (stock.Cantidad < cantidad)
                        {
                            throw StockException.StockInsuficiente(idProducto, cantidad, stock.Cantidad);
                        }
                        
                        stock.Cantidad -= cantidad;
                        _stockRepository.Update(stock);
                        
                        // Verificar si el stock quedó bajo después de disminuir
                        VerificarYNotificarStockBajo(idProducto, stock);
                    }
                }, "Error al disminuir stock");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"❌ No se puede disminuir stock sin conexión.");
                    throw new StockException("No se puede actualizar el stock: Sin conexión a la base de datos");
                }
                throw;
            }
        }

        /// <summary>
        /// Recupera todos los registros de stock disponibles.
        /// </summary>
        /// <returns>Listado completo de stock.</returns>
        public List<Stock> ObtenerStock()
        {
            try
            {
                return ExceptionMapper.ExecuteWithMapping(() =>
                {
                    return _stockRepository.GetAll();
                }, "Error al obtener stocks");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    Console.WriteLine($"⚠️ Error de conexión al obtener stocks.");
                }
                throw;
            }
        }

        /// <summary>
        /// Obtiene el stock con información completa del producto para mostrar en la UI
        /// </summary>
        public List<StockDTO> ObtenerStockConDetalles()
        {
            try
            {
                return ExceptionMapper.ExecuteWithMapping(() =>
                {
                    List<StockDTO> listaStockDTO = new List<StockDTO>();
                    
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
                    return listaStockDTO;
                }, "Error al obtener stock con detalles");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    Console.WriteLine($"⚠️ Error de conexión al obtener stock con detalles.");
                }
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener stock con detalles: {ex.Message}");
                LoggerService.WriteException(ex);
                return new List<StockDTO>();
            }
        }

        /// <summary>
        /// Devuelve el stock asociado a un producto (toma el primer resultado encontrado).
        /// </summary>
        public Stock ObtenerStockPorProducto(Guid idProducto)
        {
            try
            {
                return ExceptionMapper.ExecuteWithMapping(() =>
                {
                    return _stockRepository.GetByProducto(idProducto).FirstOrDefault();
                }, $"Error al obtener stock del producto {idProducto}");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    Console.WriteLine($"⚠️ Error de conexión al obtener stock por producto.");
                }
                throw;
            }
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
            try
            {
                return ExceptionMapper.ExecuteWithMapping(() =>
                {
                    return _stockRepository.GetAll()
                        .Where(s => s.Cantidad < STOCK_MINIMO)
                        .ToList();
                }, "Error al obtener productos con stock bajo");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    Console.WriteLine($"⚠️ Error de conexión al obtener productos con stock bajo.");
                }
                throw;
            }
        }
    }
}
