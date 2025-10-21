using System;
using System.Collections.Generic;
using BLL.Exceptions;
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
            ValidarProducto(producto);
            var productoExistente = _productoRepository.GetById(producto.IdProducto);

            // Insertar el producto en la base de datos
            if (productoExistente == null)
            {
                _productoRepository.Add(producto);
            }
            else
            {
                throw new Exception("El producto ya existe en la base de datos.");
            }
            // Insertar el stock inicial para el producto recién creado
            Stock nuevoStock = new Stock
            {
                IdStock = Guid.NewGuid(),
                IdProducto = producto.IdProducto, // Relación con el producto
                Cantidad = cantidadInicial, // Cantidad inicial
                Tipo = tipoStock // Tipo de stock
            };

            // Insertar el stock en la base de datos
            _stockRepository.Add(nuevoStock);
        }


        /// <summary>
        /// Comprueba que el producto tenga datos válidos antes de persistirlo o modificarlo.
        /// </summary>
        /// <param name="producto">Producto a validar.</param>
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
            ValidarProducto(producto);
            _productoRepository.Update(producto);
        }

        /// <summary>
        /// Marca un producto como inactivo sin eliminarlo físicamente del sistema.
        /// </summary>
        /// <param name="idProducto">Identificador del producto a deshabilitar.</param>
        public void DeshabilitarProducto(Guid idProducto)
        {
            _productoRepository.Disable(idProducto);
        }

        /// <summary>
        /// Obtiene todos los productos registrados en el repositorio.
        /// </summary>
        /// <returns>Listado completo de productos.</returns>
        public List<Producto> ObtenerTodosProductos()
        {
            return _productoRepository.GetAll();
        }

        /// <summary>
        /// Recupera un producto específico a partir de su identificador.
        /// </summary>
        /// <param name="idProducto">Identificador único del producto.</param>
        /// <returns>Producto encontrado o null si no existe.</returns>
        public Producto ObtenerProductoPorId(Guid idProducto)
        {
            return _productoRepository.GetById(idProducto);
        }

        /// <summary>
        /// Obtiene el precio actual registrado para el producto indicado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        /// <returns>Precio del producto o 0 si no se encuentra.</returns>
        public decimal ObtenerPrecioProducto(Guid idProducto)
        {
            Producto producto = _productoRepository.GetById(idProducto);
            return producto != null ? producto.Precio : 0;
        }


        /// <summary>
        /// Elimina un producto y sus registros de stock asociados del sistema.
        /// </summary>
        /// <param name="idProducto">Identificador del producto a eliminar.</param>
        public void EliminarProducto(Guid idProducto)
        {
            // Primero eliminar el stock relacionado
            _stockRepository.EliminarStockPorProducto(idProducto);

            // Luego eliminar el producto
            _productoRepository.Remove(idProducto);
        }

        /// <summary>
        /// Devuelve los productos cuya categoría coincide con la indicada.
        /// </summary>
        /// <param name="categoria">Nombre de la categoría a filtrar.</param>
        /// <returns>Listado de productos pertenecientes a la categoría.</returns>
        public List<Producto> ObtenerProductosPorCategoria(string categoria)
        {
            return _productoRepository.GetByCategoria(categoria);
        }

    }
}
