using Service.DAL.Contracts;
using Service.DAL.Implementations;
using Service.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service.Facade
{
    public class BackupService
    {
        public static void ExecuteBackup(string selectedPath)
        {
            // Crear una instancia del repositorio de backup
            IBackupRepository backupRepository = new BackupRepository();

            // Crear una instancia de BackupLogic con el repositorio
            BackUpLogic backupLogic = new BackUpLogic(backupRepository);

            // Ejecutar el backup
            try
            {
                backupLogic.PerformBackup(selectedPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Error");
                //LoggerService.WriteException(ex);
                throw;
            }
        }

    }

}
