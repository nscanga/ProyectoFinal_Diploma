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
    public static class LoggerService
    {
        public static void WriteLog(Log log, Exception ex = null)
        {
            LoggerLogic.writelog(log, ex);
        }

        public static void WriteLog(string message, TraceLevel level = TraceLevel.Info)
        {
            LoggerLogic.writelog(new Log(message, level));
        }

        public static void WriteException(Exception ex)
        {
            LoggerLogic.writelog(new Log(ex.Message, TraceLevel.Error), ex);
        }
    }
}
