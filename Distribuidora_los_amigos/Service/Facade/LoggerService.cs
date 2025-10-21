using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Service.DOMAIN;
using Service.Logic;

namespace Services.Facade
{
    /// <summary>
    /// Simplifica el acceso a la lógica de logging desde las capas superiores.
    /// </summary>
    public static class LoggerService
    {
        /// <summary>
        /// Registra un evento utilizando un objeto de log ya construido.
        /// </summary>
        /// <param name="log">Información principal del evento.</param>
        /// <param name="ex">Excepción adicional, si existe.</param>
        public static void WriteLog(Log log, Exception ex = null)
        {
            LoggerLogic.writelog(log, ex);
        }

        /// <summary>
        /// Registra un mensaje textual con el nivel indicado.
        /// </summary>
        /// <param name="message">Mensaje descriptivo del evento.</param>
        /// <param name="level">Nivel de severidad.</param>
        public static void WriteLog(string message, TraceLevel level = TraceLevel.Info)
        {
            LoggerLogic.writelog(new Log(message, level));
        }

        /// <summary>
        /// Registra una excepción capturando su mensaje y pila de llamadas.
        /// </summary>
        /// <param name="ex">Excepción a registrar.</param>
        public static void WriteException(Exception ex)
        {
            LoggerLogic.writelog(new Log(ex.Message, TraceLevel.Error), ex);
        }
    }
}
