using DAL.Contracts;
using DAL.Contratcs;
using DAL.Factory;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementations.SqlServer
{
    /// <summary>
    /// Implementación del patrón Unit of Work para SQL Server.
    /// </summary>
    public class SqlUnitOfWork : IUnitOfWork
    {
        private readonly string _connectionString;
        private SqlConnection _connection;
        private SqlTransaction _transaction;
        private bool _disposed = false;

        // Lazy loading de repositorios
        private IProductoRepository _productoRepository;
        private IStockRepository _stockRepository;
        private IProveedorRepository _proveedorRepository;
        private IPedidoRepository _pedidoRepository;
        private IClienteRepository _clienteRepository;
        private IDetallePedidoRepository _detallePedidoRepository;
        private IEstadoPedidoRepository _estadoPedidoRepository;

        /// <summary>
        /// Inicializa una instancia del Unit of Work configurando la conexión SQL Server.
        /// </summary>
        public SqlUnitOfWork()
        {
            _connectionString = ConfigurationManager.AppSettings["MiConexion"];
            _connection = new SqlConnection(_connectionString);
        }

        // Propiedades de repositorios con lazy loading
        /// <summary>
        /// Obtiene el repositorio de productos administrado por la unidad de trabajo.
        /// </summary>
        public IProductoRepository ProductoRepository =>
            _productoRepository ?? (_productoRepository = FactoryDAL.SqlProductoRepository);

        /// <summary>
        /// Obtiene el repositorio de stock administrado por la unidad de trabajo.
        /// </summary>
        public IStockRepository StockRepository =>
            _stockRepository ?? (_stockRepository = FactoryDAL.SqlStockRepository);

        /// <summary>
        /// Obtiene el repositorio de proveedores administrado por la unidad de trabajo.
        /// </summary>
        public IProveedorRepository ProveedorRepository =>
            _proveedorRepository ?? (_proveedorRepository = FactoryDAL.SqlProveedorRepository);

        /// <summary>
        /// Obtiene el repositorio de pedidos administrado por la unidad de trabajo.
        /// </summary>
        public IPedidoRepository PedidoRepository =>
            _pedidoRepository ?? (_pedidoRepository = FactoryDAL.SqlPedidoRepository);

        /// <summary>
        /// Obtiene el repositorio de clientes administrado por la unidad de trabajo.
        /// </summary>
        public IClienteRepository ClienteRepository =>
            _clienteRepository ?? (_clienteRepository = FactoryDAL.SqlClienteRepository);

        /// <summary>
        /// Obtiene el repositorio de detalles de pedido administrado por la unidad de trabajo.
        /// </summary>
        public IDetallePedidoRepository DetallePedidoRepository =>
            _detallePedidoRepository ?? (_detallePedidoRepository = FactoryDAL.SqlDetallePedidoRepository);

        /// <summary>
        /// Obtiene el repositorio de estados de pedido administrado por la unidad de trabajo.
        /// </summary>
        public IEstadoPedidoRepository EstadoPedidoRepository =>
            _estadoPedidoRepository ?? (_estadoPedidoRepository = FactoryDAL.SqlEstadoPedidoRepository);

        /// <summary>
        /// Inicia una transacción sobre la conexión subyacente.
        /// </summary>
        public void BeginTransaction()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            _transaction = _connection.BeginTransaction();
        }

        /// <summary>
        /// Confirma los cambios realizados durante la transacción activa.
        /// </summary>
        public void Commit()
        {
            try
            {
                _transaction?.Commit();
            }
            catch
            {
                _transaction?.Rollback();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        /// Revierte los cambios realizados en la transacción activa.
        /// </summary>
        public void Rollback()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        /// Persiste los cambios ejecutando un ciclo de transacción implícito si fuera necesario.
        /// </summary>
        public void SaveChanges()
        {
            // En este contexto, SaveChanges podría hacer commit automático
            // si no hay transacción explícita
            if (_transaction == null)
            {
                BeginTransaction();
                Commit();
            }
        }

        /// <summary>
        /// Libera los recursos administrados por la unidad de trabajo.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Patrón de liberación de recursos para cerrar conexiones y transacciones.
        /// </summary>
        /// <param name="disposing">Indica si se liberan recursos administrados.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
                _disposed = true;
            }
        }
    }
}
