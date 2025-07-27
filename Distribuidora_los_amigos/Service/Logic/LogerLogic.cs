using Service.Context;
using Service.DAL.FactoryServices;
using Service.DOMAIN;
using System;

namespace Service.Logic
{
    public static class LoggerLogic
    {
        private static LoggerContext _loggerContext = new LoggerContext();

        // inicializa con la estrategia por defecto
        static LoggerLogic()
        {
             _loggerContext.SetLoggerStrategy(LoggerFactory.CreateLogger());
        }

        public static void writelog(Log log, Exception ex = null)
        {
            // si es un error crítico, podrías hacer alguna acción extra aquí, como enviar una notificación

            // escribir el log utilizando la estrategia configurada
            _loggerContext.WriteLog(log, ex);
        }
    }
}
