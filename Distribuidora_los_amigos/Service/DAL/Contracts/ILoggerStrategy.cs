using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Contracts
{
    public interface ILoggerStrategy
    {
        void WriteLog(Log log, Exception ex = null);
    }
}
