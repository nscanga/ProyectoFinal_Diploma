using Service.DAL.Contracts;
using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Context
{
    //Implemento una clase de contexto que utilice la estrategia de logging: Se selecciona o configura la estrategia que se usará
    public class LoggerContext
    {
        private ILoggerStrategy _loggerStrategy;

        // Permitir cambiar la estrategia en tiempo de ejecución
        public void SetLoggerStrategy(ILoggerStrategy strategy)
        {
            _loggerStrategy = strategy;
        }

        public void WriteLog(Log log, Exception ex = null)
        {
            _loggerStrategy.WriteLog(log, ex);
        }
    }
}
