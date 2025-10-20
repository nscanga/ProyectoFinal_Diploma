using DAL.Contracts;
using DAL.Contratcs;
using DAL.Factory;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL.Implementations.SqlServer
{
    /// <summary>
    /// Implementación del patrón Unit of Work para SQL Server
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

        public SqlUnitOfWork()
        {
            _connectionString = ConfigurationManager.AppSettings["MiConexion"];
            _connection = new SqlConnection(_connectionString);
        }

        // Propiedades de repositorios con lazy loading
        public IProductoRepository ProductoRepository => 
            _productoRepository ?? (_productoRepository = FactoryDAL.SqlProductoRepository);

        public IStockRepository StockRepository => 
            _stockRepository ?? (_stockRepository = FactoryDAL.SqlStockRepository);

        public IProveedorRepository ProveedorRepository => 
            _proveedorRepository ?? (_proveedorRepository = FactoryDAL.SqlProveedorRepository);

        public IPedidoRepository PedidoRepository => 
            _pedidoRepository ?? (_pedidoRepository = FactoryDAL.SqlPedidoRepository);

        public IClienteRepository ClienteRepository => 
            _clienteRepository ?? (_clienteRepository = FactoryDAL.SqlClienteRepository);

        public IDetallePedidoRepository DetallePedidoRepository => 
            _detallePedidoRepository ?? (_detallePedidoRepository = FactoryDAL.SqlDetallePedidoRepository);

        public IEstadoPedidoRepository EstadoPedidoRepository => 
            _estadoPedidoRepository ?? (_estadoPedidoRepository = FactoryDAL.SqlEstadoPedidoRepository);

        public void BeginTransaction()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            _transaction = _connection.BeginTransaction();
        }

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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