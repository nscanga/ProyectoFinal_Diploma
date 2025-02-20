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


        public ProductoService()
        {
            _productoRepository = FactoryDAL.SqlProductoRepository;
            _stockRepository = FactoryDAL.SqlStockRepository; 

        }

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

        public void ModificarProducto(Producto producto)
        {
            ValidarProducto(producto);
            _productoRepository.Update(producto);
        }

        public void DeshabilitarProducto(Guid idProducto)
        {
            _productoRepository.Disable(idProducto);
        }

        public List<Producto> ObtenerTodosProductos()
        {
            return _productoRepository.GetAll();
        }

        public Producto ObtenerProductoPorId(Guid idProducto)
        {
            return _productoRepository.GetById(idProducto);
        }

        public void EliminarProducto(Guid idProducto)
        {
            // Primero eliminar el stock relacionado
            _stockRepository.EliminarStockPorProducto(idProducto);

            // Luego eliminar el producto
            _productoRepository.Remove(idProducto);
        }

        public List<Producto> ObtenerProductosPorCategoria(string categoria)
        {
            return _productoRepository.GetByCategoria(categoria);
        }

    }
}
