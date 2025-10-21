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
    /// Estrategia de logging orientada a guardar eventos en archivos de texto.
    /// </summary>
    public class FileLoggerStrategy : ILoggerStrategy
    {
        /// <summary>
        /// Persiste el log recibido en un archivo mediante el repositorio de logger.
        /// </summary>
        /// <param name="log">Evento a registrar.</param>
        /// <param name="ex">Excepción a detallar, cuando existe.</param>
        public void WriteLog(Log log, Exception ex = null)
        {
            LoggerRepository.WriteLogToFile(log, ex);
        }
    }
}
