using Service.Context;
using Service.DAL.FactoryServices;
using Service.DOMAIN;
using System;

namespace Service.Logic
{
    /// <summary>
    /// Expone un punto centralizado para registrar eventos mediante estrategias configurables.
    /// </summary>
    public static class LoggerLogic
    {
        private static LoggerContext _loggerContext = new LoggerContext();

        // inicializa con la estrategia por defecto
        /// <summary>
        /// Configura la estrategia de logging indicada en la fábrica.
        /// </summary>
        static LoggerLogic()
        {
             _loggerContext.SetLoggerStrategy(LoggerFactory.CreateLogger());
        }

        /// <summary>
        /// Escribe un registro utilizando la estrategia actual, admitiendo excepciones asociadas.
        /// </summary>
        /// <param name="log">Datos del evento a registrar.</param>
        /// <param name="ex">Excepción opcional para complementar el log.</param>
        public static void writelog(Log log, Exception ex = null)
        {
            // si es un error crítico, podrías hacer alguna acción extra aquí, como enviar una notificación

            // escribir el log utilizando la estrategia configurada
            _loggerContext.WriteLog(log, ex);
        }
    }
}
