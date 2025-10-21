using Service.DAL.Contracts;
using Service.DAL.Implementations.SqlServer;
using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Implementations.Strategy
{
    /// <summary>
    /// Estrategia de logging que persiste los eventos en la base de datos.
    /// </summary>
    public class DatabaseLoggerStrategy : ILoggerStrategy
    {
        /// <summary>
        /// Registra la información recibida utilizando el repositorio de bitácora en base de datos.
        /// </summary>
        /// <param name="log">Datos principales del evento.</param>
        /// <param name="ex">Excepción asociada para almacenar el detalle, si existe.</param>
        public void WriteLog(Log log, Exception ex = null)
        {
            LoggerRepository.WriteLogToDatabase(log, ex);
        }
    }
}
