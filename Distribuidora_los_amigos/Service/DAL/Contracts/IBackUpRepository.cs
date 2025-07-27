using Service.DOMAIN.BackUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Contracts
{
    public interface IBackupRepository
    {
     void BackupDatabase(string connectionString, string backupPath);
    }
}
