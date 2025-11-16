using System;
using System.Collections.Generic;
using BLL.Exceptions;
using BLL.Helpers;
using DAL.Contratcs;
using DAL.Factory;
using DOMAIN;

namespace BLL
{
    public class ProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IStockRepository _stockRepository;


        /// <summary>
        /// Inicializa el servicio preparando los repositorios necesarios para gestionar productos y stock.
        /// </summary>
        public ProductoService()
        {
            _productoRepository = FactoryDAL.SqlProductoRepository;
            _stockRepository = FactoryDAL.SqlStockRepository;

        }

        /// <summary>
        /// Crea un nuevo producto y registra su stock inicial asociado al tipo indicado.
        /// </summary>
        /// <param name="producto">Entidad del producto a persistir.</param>
        /// <param name="cantidadInicial">Cantidad de stock con la que se inicializará el producto.</param>
        /// <param name="tipoStock">Descripción del tipo de stock (por ejemplo, unidad o caja).</param>
        public void CrearProducto(Producto producto, int cantidadInicial, string tipoStock)
        {
            try
            {
                // 🆕 Validaciones completas de negocio
                ValidarProductoCompleto(producto, cantidadInicial, tipoStock);
                
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    var productoExistente = _productoRepository.GetById(producto.IdProducto);

                    // Insertar el producto en la base de datos
                    if (productoExistente == null)
                    {
                        _productoRepository.Add(producto);
                    }
                    else
                    {
                        throw ProductoException.ProductoDuplicado(producto.Nombre);
                    }
                    
                    // Insertar el stock inicial para el producto recién creado
                    Stock nuevoStock = new Stock
                    {
                        IdStock = Guid.NewGuid(),
                        IdProducto = producto.IdProducto,
                        Cantidad = cantidadInicial,
                        Tipo = tipoStock
                    };

                    _stockRepository.Add(nuevoStock);
                }, "Error al crear producto");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"❌ No se puede crear producto sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Valida completamente un producto antes de crearlo o modificarlo.
        /// Incluye validaciones de datos básicos, fechas y stock inicial.
        /// </summary>
        /// <param name="producto">Producto a validar.</param>
        /// <param name="cantidadInicial">Cantidad inicial de stock (solo para creación).</param>
        /// <param name="tipoStock">Tipo de stock (solo para creación).</param>
        private void ValidarProductoCompleto(Producto producto, int cantidadInicial = 0, string tipoStock = null)
        {
            // Validar que el producto no sea nulo
            if (producto == null)
                throw ProductoException.ProductoNulo();

            // Validar nombre
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw ProductoException.NombreRequerido();

            // Validar categoría
            if (string.IsNullOrWhiteSpace(producto.Categoria))
                throw ProductoException.CategoriaRequerida();

            // Validar precio
            if (producto.Precio <= 0)
                throw ProductoException.PrecioInvalido(producto.Precio);

            // Validar fecha de ingreso
            if (producto.FechaIngreso == default)
                throw ProductoException.FechaIngresoRequerida();

            // Validar que la fecha de ingreso no sea futura
            if (producto.FechaIngreso > DateTime.Now)
                throw ProductoException.FechaIngresoInvalida(producto.FechaIngreso);

            // Validar fecha de vencimiento (si está presente)
            if (producto.Vencimiento.HasValue)
            {
                if (producto.Vencimiento.Value < producto.FechaIngreso)
                    throw ProductoException.VencimientoInvalido(producto.Vencimiento.Value, producto.FechaIngreso);
            }

            // Validar cantidad inicial (solo para creación)
            if (cantidadInicial < 0)
                throw ProductoException.CantidadInvalida(cantidadInicial);

            // Validar tipo de stock (solo para creación cuando se especifica cantidad)
            if (cantidadInicial > 0 && string.IsNullOrWhiteSpace(tipoStock))
                throw ProductoException.TipoStockRequerido();
        }

        /// <summary>
        /// Comprueba que el producto tenga datos válidos antes de persistirlo o modificarlo.
        /// NOTA: Método legacy mantenido por compatibilidad. Usar ValidarProductoCompleto() en su lugar.
        /// </summary>
        /// <param name="producto">Producto a validar.</param>
        [Obsolete("Usar ValidarProductoCompleto() que incluye validaciones más robustas")]
        private void ValidarProducto(Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre) ||
                string.IsNullOrWhiteSpace(producto.Categoria))
            {
                throw ProductoException.CamposInvalidos();
            }

            if (producto.Precio <= 0)
            {
                throw new ArgumentException("El precio debe ser mayor que 0.");
            }

            if (producto.FechaIngreso == default)
            {
                throw new ArgumentException("La fecha de ingreso es obligatoria.");
            }

            if (producto.Vencimiento != null && producto.Vencimiento < producto.FechaIngreso)
            {
                throw new ArgumentException("La fecha de vencimiento no puede ser anterior a la fecha de ingreso.");
            }
        }

        /// <summary>
        /// Actualiza los datos de un producto existente luego de validarlo.
        /// </summary>
        /// <param name="producto">Producto con los cambios a aplicar.</param>
        public void ModificarProducto(Producto producto)
        {
            try
            {
                // 🆕 Usar la validación completa (sin cantidadInicial ni tipoStock)
                ValidarProductoCompleto(producto);
                
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    _productoRepository.Update(producto);
                }, "Error al modificar producto");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"❌ No se puede modificar producto sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Marca un producto como inactivo sin eliminarlo físicamente del sistema.
        /// </summary>
        /// <param name="idProducto">Identificador del producto a deshabilitar.</param>
        public void DeshabilitarProducto(Guid idProducto)
        {
            try
            {
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    _productoRepository.Disable(idProducto);
                }, "Error al deshabilitar producto");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"❌ No se puede deshabilitar producto sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los productos registrados en el repositorio.
        /// </summary>
        /// <returns>Listado completo de productos.</returns>
        public List<Producto> ObtenerTodosProductos()
        {
            try
            {
                return ExceptionMapper.ExecuteWithMapping(() =>
                {
                    return _productoRepository.GetAll();
                }, "Error al obtener productos");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    Console.WriteLine($"⚠️ Error de conexión al obtener productos.");
                }
                throw;
            }
        }

        /// <summary>
        /// Recupera un producto específico a partir de su identificador.
        /// </summary>
        /// <param name="idProducto">Identificador único del producto.</param>
        /// <returns>Producto encontrado o null si no existe.</returns>
        public Producto ObtenerProductoPorId(Guid idProducto)
        {
            try
            {
                return ExceptionMapper.ExecuteWithMapping(() =>
                {
                    return _productoRepository.GetById(idProducto);
                }, $"Error al obtener producto {idProducto}");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    Console.WriteLine($"⚠️ Error de conexión al obtener producto.");
                }
                throw;
            }
        }

        /// <summary>
        /// Obtiene el precio actual registrado para el producto indicado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        /// <returns>Precio del producto o 0 si no se encuentra.</returns>
        public decimal ObtenerPrecioProducto(Guid idProducto)
        {
            try
            {
                return ExceptionMapper.ExecuteWithMapping(() =>
                {
                    Producto producto = _productoRepository.GetById(idProducto);
                    return producto != null ? producto.Precio : 0;
                }, $"Error al obtener precio del producto {idProducto}");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    Console.WriteLine($"⚠️ Error de conexión al obtener precio del producto.");
                }
                throw;
            }
        }


        /// <summary>
        /// Elimina un producto y sus registros de stock asociados del sistema.
        /// </summary>
        /// <param name="idProducto">Identificador del producto a eliminar.</param>
        public void EliminarProducto(Guid idProducto)
        {
            try
            {
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    // Primero eliminar el stock relacionado
                    _stockRepository.EliminarStockPorProducto(idProducto);

                    // Luego eliminar el producto
                    _productoRepository.Remove(idProducto);
                }, "Error al eliminar producto");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"❌ No se puede eliminar producto sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Devuelve los productos cuya categoría coincide con la indicada.
        /// </summary>
        /// <param name="categoria">Nombre de la categoría a filtrar.</param>
        /// <returns>Listado de productos pertenecientes a la categoría.</returns>
        public List<Producto> ObtenerProductosPorCategoria(string categoria)
        {
            try
            {
                return ExceptionMapper.ExecuteWithMapping(() =>
                {
                    return _productoRepository.GetByCategoria(categoria);
                }, $"Error al obtener productos de categoría {categoria}");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    Console.WriteLine($"⚠️ Error de conexión al obtener productos por categoría.");
                }
                throw;
            }
        }

    }
}
