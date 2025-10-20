using BLL.Commands;
using DOMAIN;
using DAL.Contracts;
using System;

namespace BLL.Commands.Producto
{
    /// <summary>
    /// Comando para crear un producto con stock inicial
    /// </summary>
    public class CrearProductoConStockCommand : BaseCommand
    {
        private readonly DOMAIN.Producto _producto;
        private readonly int _cantidadInicial;
        private readonly string _tipoStock;
        private readonly IUnitOfWork _unitOfWork;
        private bool _wasExecuted = false;

        public CrearProductoConStockCommand(
            DOMAIN.Producto producto, 
            int cantidadInicial, 
            string tipoStock,
            IUnitOfWork unitOfWork)
        {
            _producto = producto ?? throw new ArgumentNullException(nameof(producto));
            _cantidadInicial = cantidadInicial;
            _tipoStock = tipoStock;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public override bool CanExecute()
        {
            return _producto != null && 
                   !string.IsNullOrWhiteSpace(_producto.Nombre) &&
                   _cantidadInicial >= 0 &&
                   !string.IsNullOrWhiteSpace(_tipoStock);
        }

        public override void Execute()
        {
            if (!CanExecute())
            {
                throw new InvalidOperationException("El comando no puede ejecutarse en el estado actual.");
            }

            try
            {
                _unitOfWork.BeginTransaction();

                // Verificar si el producto ya existe
                var productoExistente = _unitOfWork.ProductoRepository.GetById(_producto.IdProducto);
                if (productoExistente != null)
                {
                    throw new InvalidOperationException("El producto ya existe en la base de datos.");
                }

                // Crear el producto
                _unitOfWork.ProductoRepository.Add(_producto);

                // Crear el stock inicial
                var nuevoStock = new Stock
                {
                    IdStock = Guid.NewGuid(),
                    IdProducto = _producto.IdProducto,
                    Cantidad = _cantidadInicial,
                    Tipo = _tipoStock
                };

                _unitOfWork.StockRepository.Add(nuevoStock);

                _unitOfWork.Commit();
                _wasExecuted = true;
                
                OnCommandExecuted();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                OnCommandFailed(ex);
                throw;
            }
        }

        public override void Undo()
        {
            if (!_wasExecuted)
            {
                throw new InvalidOperationException("No se puede deshacer un comando que no ha sido ejecutado.");
            }

            try
            {
                _unitOfWork.BeginTransaction();
                
                // Eliminar el stock
                _unitOfWork.StockRepository.EliminarStockPorProducto(_producto.IdProducto);
                
                // Eliminar el producto
                _unitOfWork.ProductoRepository.Remove(_producto.IdProducto);
                
                _unitOfWork.Commit();
                _wasExecuted = false;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                OnCommandFailed(ex);
                throw;
            }
        }
    }
}