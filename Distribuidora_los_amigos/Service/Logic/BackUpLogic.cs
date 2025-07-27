using Service.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Logic
{
    public class BackUpLogic
    {
        private readonly IBackupRepository _backupRepository;

        public BackUpLogic(IBackupRepository backupRepository)
        {
            _backupRepository = backupRepository;
        }

        public void PerformBackup(string selectedPath)
        {
            //string backupPath1 = ConfigurationManager.AppSettings["BackupPath1"];
            //string backupPath2 = ConfigurationManager.AppSettings["BackupPath2"];
            //string backupPath3 = ConfigurationManager.AppSettings["BackupPath3"];


            string databaseName1 = ConfigurationManager.AppSettings["Database1Name"];
            string databaseName2 = ConfigurationManager.AppSettings["Database2Name"];
            string databaseName3 = ConfigurationManager.AppSettings["Database3Name"];

            string connectionString1 = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;
            string connectionString2 = ConfigurationManager.ConnectionStrings["MiConexion2"].ConnectionString;
            string connectionString3 = ConfigurationManager.ConnectionStrings["LogDatabase"].ConnectionString;

            string backupPath1 = Path.Combine(selectedPath, $"{databaseName1}");
            string backupPath2 = Path.Combine(selectedPath, $"{databaseName2}");
            string backupPath3 = Path.Combine(selectedPath, $"{databaseName3}");
            // Realizar backup de cada base de datos
            _backupRepository.BackupDatabase(connectionString1, backupPath1);
            _backupRepository.BackupDatabase(connectionString2, backupPath2);
            _backupRepository.BackupDatabase(connectionString3, backupPath3);
        }


    }
}
