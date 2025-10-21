using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Contracts
{
    /// <summary>
    /// Estrategia de persistencia para eventos de log.
    /// </summary>
    public interface ILoggerStrategy
    {
        /// <summary>
        /// Registra un evento de log en el destino definido por la estrategia.
        /// </summary>
        /// <param name="log">Instancia con los datos del evento.</param>
        /// <param name="ex">Excepción opcional asociada.</param>
        void WriteLog(Log log, Exception ex = null);
    }
}
