using Service.DAL.Contracts;
using Service.DAL.Implementations;
using Service.Logic;
using Services.Facade;
using System;

namespace Service.Facade
{
    /// <summary>
    /// Fachada que ejecuta respaldos y expone rutas configuradas para su almacenamiento.
    /// </summary>
    public class BackupService
    {
        /// <summary>
        /// Ejecuta el proceso de backup en la ruta especificada registrando el resultado.
        /// </summary>
        /// <param name="selectedPath">Directorio donde se guardará el respaldo.</param>
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

        /// <summary>
        /// Ejecuta el proceso de restauración desde la carpeta especificada.
        /// </summary>
        /// <param name="backupFolderPath">Carpeta que contiene los archivos .bak a restaurar.</param>
        public static void ExecuteRestore(string backupFolderPath)
        {
            try
            {
                // ✅ Validar entrada
                if (string.IsNullOrWhiteSpace(backupFolderPath))
                {
                    throw new ArgumentException("Debe seleccionar una carpeta válida con los archivos de backup.");
                }

                // ✅ Crear una instancia del repositorio de backup
                IBackupRepository backupRepository = new BackupRepository();

                // ✅ Crear una instancia de BackupLogic con el repositorio
                BackUpLogic backupLogic = new BackUpLogic(backupRepository);

                // ✅ Ejecutar la restauración
                backupLogic.PerformRestore(backupFolderPath);

                // ✅ Registrar éxito en el log
                LoggerService.WriteLog($"Restauración completada exitosamente desde: {backupFolderPath}", System.Diagnostics.TraceLevel.Info);
            }
            catch (Exception ex)
            {
                // ✅ Registrar error
                LoggerService.WriteException(ex);
                throw new Exception($"Error al ejecutar la restauración: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene las rutas disponibles para almacenar respaldos desde la configuración.
        /// </summary>
        /// <returns>Listado de rutas configuradas.</returns>
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
