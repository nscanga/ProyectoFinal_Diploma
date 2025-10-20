using System;
using System.Configuration;
using DAL.Contracts;
using DAL.Contratcs;
using DAL.Implementations.SqlServer;

namespace DAL.Factory
{
    public static class FactoryDAL
    {
        // Objeto de bloqueo para garantizar la seguridad en entornos multi-hilo
        private static readonly object _lock = new object();

        // Repositorios privados
        private static IProductoRepository _productoRepository;
        private static IStockRepository _stockRepository;
        private static IProveedorRepository _proveedorRepository;
        private static IPedidoRepository _pedidoRepository;
        private static IClienteRepository _clienteRepository;
        private static IDetallePedidoRepository _detallePedidoRepository;
        private static IEstadoPedidoRepository _estadoPedidoRepository;

        // Variables de configuración
        private static readonly int backendType;
        private static readonly string connectionString;

        static FactoryDAL()
        {
            backendType = int.Parse(ConfigurationManager.AppSettings["BackendType"]);
            connectionString = ConfigurationManager.AppSettings["MiConexion"];
        }

        /// <summary>
        /// Obtiene el repositorio de productos
        /// </summary>
        public static IProductoRepository SqlProductoRepository
        {
            get
            {
                if (_productoRepository == null)
                {
                    lock (_lock)
                    {
                        if (_productoRepository == null)
                        {
                            switch ((BackendType)backendType)
                            {
                                case BackendType.SqlServer:
                                    _productoRepository = new SqlProductoRepository();
                                    break;
                                default:
                                    throw new NotSupportedException("Backend no soportado.");
                            }
                        }
                    }
                }
                return _productoRepository;
            }
        }

        /// <summary>
        /// Obtiene el repositorio de stock
        /// </summary>
        public static IStockRepository SqlStockRepository
        {
            get
            {
                if (_stockRepository == null)
                {
                    lock (_lock)
                    {
                        if (_stockRepository == null)
                        {
                            switch ((BackendType)backendType)
                            {
                                case BackendType.SqlServer:
                                    _stockRepository = new SqlStockRepository();
                                    break;
                                default:
                                    throw new NotSupportedException("Backend no soportado.");
                            }
                        }
                    }
                }
                return _stockRepository;
            }
        }

        /// <summary>
        /// Obtiene el repositorio de proveedores
        /// </summary>
        public static IProveedorRepository SqlProveedorRepository
        {
            get
            {
                if (_proveedorRepository == null)
                {
                    lock (_lock)
                    {
                        if (_proveedorRepository == null)
                        {
                            switch ((BackendType)backendType)
                            {
                                case BackendType.SqlServer:
                                    _proveedorRepository = new SqlProveedorRepository();
                                    break;
                                default:
                                    throw new NotSupportedException("Backend no soportado.");
                            }
                        }
                    }
                }
                return _proveedorRepository;
            }
        }

        /// <summary>
        /// Obtiene el repositorio de pedidos
        /// </summary>
        public static IPedidoRepository SqlPedidoRepository
        {
            get
            {
                if (_pedidoRepository == null)
                {
                    lock (_lock)
                    {
                        if (_pedidoRepository == null)
                        {
                            switch ((BackendType)backendType)
                            {
                                case BackendType.SqlServer:
                                    _pedidoRepository = new SqlPedidoRepository();
                                    break;
                                default:
                                    throw new NotSupportedException("Backend no soportado.");
                            }
                        }
                    }
                }
                return _pedidoRepository;
            }
        }

        public static IClienteRepository SqlClienteRepository
        {
            get
            {
                if (_clienteRepository == null)
                {
                    lock (_lock)
                    {
                        if (_clienteRepository == null)
                        {
                            switch ((BackendType)backendType)
                            {
                                case BackendType.SqlServer:
                                    _clienteRepository = new SqlClienteRepository();
                                    break;
                                default:
                                    throw new NotSupportedException("Backend no soportado.");
                            }
                        }
                    }
                }
                return _clienteRepository;
            }
        }

        public static IDetallePedidoRepository SqlDetallePedidoRepository
        {
            get
            {
                if (_detallePedidoRepository == null)
                {
                    lock (_lock)
                    {
                        if (_detallePedidoRepository == null)
                        {
                            switch ((BackendType)backendType)
                            {
                                case BackendType.SqlServer:
                                    _detallePedidoRepository = new SqlDetallePedidoRepository();
                                    break;
                                default:
                                    throw new NotSupportedException("Backend no soportado.");
                            }
                        }
                    }
                }
                return _detallePedidoRepository;
            }
        }

        public static IEstadoPedidoRepository SqlEstadoPedidoRepository
        {
            get
            {
                if (_estadoPedidoRepository == null)
                {
                    lock (_lock)
                    {
                        if (_estadoPedidoRepository == null)
                        {
                            switch ((BackendType)backendType)
                            {
                                case BackendType.SqlServer:
                                    _estadoPedidoRepository = new SqlEstadoPedidoRepository();
                                    break;
                                default:
                                    throw new NotSupportedException("Backend no soportado.");
                            }
                        }
                    }
                }
                return _estadoPedidoRepository;
            }
        }

        /// <summary>
        /// Método genérico para crear repositorios con reflection
        /// </summary>
        /// <typeparam name="TInterface">Tipo de interfaz del repositorio</typeparam>
        /// <typeparam name="TImplementation">Tipo de implementación concreta</typeparam>
        /// <returns>Instancia del repositorio</returns>
        private static TInterface CreateRepository<TInterface, TImplementation>()
            where TImplementation : class, TInterface, new()
        {
            switch ((BackendType)backendType)
            {
                case BackendType.SqlServer:
                    return new TImplementation();
                default:
                    throw new NotSupportedException($"Backend {backendType} no soportado para {typeof(TInterface).Name}.");
            }
        }

        /// <summary>
        /// Método thread-safe genérico para obtener repositorios
        /// </summary>
        /// <typeparam name="TInterface">Tipo de interfaz del repositorio</typeparam>
        /// <typeparam name="TImplementation">Tipo de implementación concreta</typeparam>
        /// <param name="repository">Referencia al campo del repositorio</param>
        /// <returns>Instancia del repositorio</returns>
        private static TInterface GetOrCreateRepository<TInterface, TImplementation>(ref TInterface repository)
            where TImplementation : class, TInterface, new()
        {
            if (repository == null)
            {
                lock (_lock)
                {
                    if (repository == null)
                    {
                        repository = CreateRepository<TInterface, TImplementation>();
                    }
                }
            }
            return repository;
        }

        /// <summary>
        /// Enumera los tipos de backend disponibles
        /// </summary>
        internal enum BackendType
        {
            SqlServer = 1,
            // Futuras implementaciones: Oracle = 2, MySQL = 3, etc.
        }
    }
}
