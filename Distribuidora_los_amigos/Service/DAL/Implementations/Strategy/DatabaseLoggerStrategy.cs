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
    public class DatabaseLoggerStrategy : ILoggerStrategy
    {
        public void WriteLog(Log log, Exception ex = null)
        {
            LoggerRepository.WriteLogToDatabase(log, ex);
        }
    }
}
