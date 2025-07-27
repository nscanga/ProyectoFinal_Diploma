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
    public static class LoggerFactory
    {
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
