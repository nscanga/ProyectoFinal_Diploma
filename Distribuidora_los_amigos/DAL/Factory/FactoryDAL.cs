using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contratcs;
using DAL.Implementations.SqlServer;

namespace DAL.Factory
{
    public static class FactoryDAL
    {
        // Objeto de bloqueo para asegurar que la inicialización de repositorios sea segura en un entorno multi-hilo.
        private static readonly object _lock = new object();

        // Repositorios privados para cada tipo de entidad en la base de datos.
        private static IProductoRepository _productoRepository;
        private static IStockRepository _stockRepository;
        private static IProveedorRepository _proveedorRepository;




        // Variables para leer la configuración del tipo de backend y cadena de conexión.
        private static readonly int backendType;
        private static readonly string connectionString;

        static FactoryDAL()
        {
            // Lee el tipo de backend (por ejemplo, SQL Server) desde la configuración.
            backendType = int.Parse(ConfigurationManager.AppSettings["BackendType"]);

            // Lee la cadena de conexión desde la configuración.
            connectionString = ConfigurationManager.AppSettings["MiConexion"];
        }

        /// <summary>
        /// Propiedad para acceder al repositorio de Paciente. Se utiliza el patrón Singleton para asegurarse de que solo se cree una instancia del repositorio.
        /// Dependiendo de la configuración, el repositorio se crea para el tipo de backend especificado.
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
                            // Instancia el repositorio de Paciente para el backend SQL Server.
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
        /// Propiedad para acceder al repositorio de Stock usando el patrón Singleton.
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

        internal enum BackendType
        {
            SqlServer = 1
        }

    }
}
