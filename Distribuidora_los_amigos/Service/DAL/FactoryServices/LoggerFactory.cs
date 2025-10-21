using Service.DAL.Contracts;
using Service.DAL.Implementations.Strategy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.FactoryServices
{
    /// <summary>
    /// Fabrica que crea estrategias de logging basadas en la configuración de la aplicación.
    /// </summary>
    public static class LoggerFactory
    {
        /// <summary>
        /// Resuelve la estrategia de logging definida en la configuración (archivo o base de datos).
        /// </summary>
        /// <returns>Implementación concreta de <see cref="ILoggerStrategy"/>.</returns>
        public static ILoggerStrategy CreateLogger()
        {
            string loggerType = ConfigurationManager.AppSettings["LoggerType"]; // "file" o "database"

            switch (loggerType.ToLower())
            {
                case "file":
                    return new FileLoggerStrategy();
                case "database":
                    return new DatabaseLoggerStrategy();
                default:
                    throw new NotSupportedException("base de datos no implementada ");
            }
        }
    }
}
