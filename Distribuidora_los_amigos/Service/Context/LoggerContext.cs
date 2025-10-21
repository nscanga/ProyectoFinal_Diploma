using Service.DAL.Contracts;
using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Context
{
    /// <summary>
    /// Contexto responsable de ejecutar la estrategia de logging configurada para persistir registros.
    /// </summary>
    public class LoggerContext
    {
        private ILoggerStrategy _loggerStrategy;

        /// <summary>
        /// Permite modificar la estrategia de logging que se utilizará en tiempo de ejecución.
        /// </summary>
        /// <param name="strategy">Implementación concreta que determina el destino del log.</param>
        public void SetLoggerStrategy(ILoggerStrategy strategy)
        {
            _loggerStrategy = strategy;
        }

        /// <summary>
        /// Registra un evento usando la estrategia configurada e información opcional de una excepción.
        /// </summary>
        /// <param name="log">Datos del evento a almacenar.</param>
        /// <param name="ex">Excepción asociada al evento, si corresponde.</param>
        public void WriteLog(Log log, Exception ex = null)
        {
            _loggerStrategy.WriteLog(log, ex);
        }
    }
}
