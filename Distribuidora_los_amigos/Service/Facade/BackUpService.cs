using Service.DAL.Contracts;
using Service.DAL.Implementations;
using Service.Logic;
using Services.Facade;
using System;

namespace Service.Facade
{
    public class BackupService
    {
        public static void ExecuteBackup(string selectedPath)
        {
            try
            {
                // ✅ Validar entrada
                if (string.IsNullOrWhiteSpace(selectedPath))
                {
                    throw new ArgumentException("Debe seleccionar una ruta válida para el backup.");
                }

                // ✅ Crear una instancia del repositorio de backup
                IBackupRepository backupRepository = new BackupRepository();

                // ✅ Crear una instancia de BackupLogic con el repositorio
                BackUpLogic backupLogic = new BackUpLogic(backupRepository);

                // ✅ Ejecutar el backup
                backupLogic.PerformBackup(selectedPath);

                // ✅ Registrar éxito en el log
                LoggerService.WriteLog($"Backup completado exitosamente en: {selectedPath}", System.Diagnostics.TraceLevel.Info);
            }
            catch (Exception ex)
            {
                // ✅ Registrar error
                LoggerService.WriteException(ex);
                throw new Exception($"Error al ejecutar el backup: {ex.Message}", ex);
            }
        }

        public static System.Collections.Generic.List<string> GetAvailableBackupPaths()
        {
            try
            {
                IBackupRepository backupRepository = new BackupRepository();
                BackUpLogic backupLogic = new BackUpLogic(backupRepository);
                return backupLogic.GetAvailableBackupPaths();
            }
            catch (Exception ex)
            {
                LoggerService.WriteException(ex);
                return new System.Collections.Generic.List<string>();
            }
        }
    }
}
