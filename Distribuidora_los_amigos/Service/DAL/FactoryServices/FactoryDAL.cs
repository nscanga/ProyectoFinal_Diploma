using Service.DAL.Contracts;
using Service.DAL.Implementations.SqlServer.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.FactoryServices
{
    internal class FactoryDAL
    {
        private static IUsuarioRepository _UsuarioRepository;
        private static readonly int backendType;
        private static readonly object _lock = new object();

        static FactoryDAL()
        {
            backendType = int.Parse(ConfigurationManager.AppSettings["BackendType"]);
        }

        public static IUsuarioRepository UsuarioRepository
        {
            get
            {
                if (_UsuarioRepository == null)
                {
                    lock (_lock)
                    {
                        if (_UsuarioRepository == null)
                        {
                            switch ((BackendType)backendType)
                            {
                                case BackendType.SqlServer:
                                    _UsuarioRepository = new UsuarioRepository();
                                    break;
                                default:
                                    throw new NotSupportedException("Backend no soportado.");
                            }
                        }
                    }
                }
                return _UsuarioRepository;
            }
        }
    }
    internal enum BackendType
    {
        SqlServer = 1

    }
}
