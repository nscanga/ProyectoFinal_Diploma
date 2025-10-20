using Service.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Service.Logic
{
    public class BackUpLogic
    {
        private readonly IBackupRepository _backupRepository;

        public BackUpLogic(IBackupRepository backupRepository)
        {
            _backupRepository = backupRepository ?? throw new ArgumentNullException(nameof(backupRepository));
        }

        public void PerformBackup(string selectedPath)
        {
            try
            {
                // ✅ Validar que la ruta seleccionada no esté vacía
                if (string.IsNullOrWhiteSpace(selectedPath))
                {
                    throw new ArgumentException("Debe seleccionar una ruta válida para el backup.");
                }

                // ✅ Crear el directorio si no existe
                if (!Directory.Exists(selectedPath))
                {
                    Directory.CreateDirectory(selectedPath);
                }

                // ✅ Obtener nombres de bases de datos
                string databaseName1 = ConfigurationManager.AppSettings["Database1Name"];
                string databaseName2 = ConfigurationManager.AppSettings["Database2Name"];
                string databaseName3 = ConfigurationManager.AppSettings["Database3Name"];

                // ✅ Obtener cadenas de conexión
                string connectionString1 = ConfigurationManager.ConnectionStrings["MiConexion"]?.ConnectionString;
                string connectionString2 = ConfigurationManager.ConnectionStrings["MiConexion2"]?.ConnectionString;
                string connectionString3 = ConfigurationManager.ConnectionStrings["LogDatabase"]?.ConnectionString;

                // ✅ Validar conexiones
                if (string.IsNullOrEmpty(connectionString1))
                    throw new ConfigurationErrorsException("No se encontró la cadena de conexión 'MiConexion'.");
                if (string.IsNullOrEmpty(connectionString2))
                    throw new ConfigurationErrorsException("No se encontró la cadena de conexión 'MiConexion2'.");
                if (string.IsNullOrEmpty(connectionString3))
                    throw new ConfigurationErrorsException("No se encontró la cadena de conexión 'LogDatabase'.");

                // ✅ Construir rutas de backup
                string backupPath1 = Path.Combine(selectedPath, $"{databaseName1}.bak");
                string backupPath2 = Path.Combine(selectedPath, $"{databaseName2}.bak");
                string backupPath3 = Path.Combine(selectedPath, $"{databaseName3}.bak");

                // ✅ Realizar backup de cada base de datos
                _backupRepository.BackupDatabase(connectionString1, backupPath1);
                _backupRepository.BackupDatabase(connectionString2, backupPath2);
                _backupRepository.BackupDatabase(connectionString3, backupPath3);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al ejecutar el backup: {ex.Message}", ex);
            }
        }

        public List<string> GetAvailableBackupPaths()
        {
            List<string> paths = new List<string>();
            
            string path1 = ConfigurationManager.AppSettings["BackupPath1"];
            string path2 = ConfigurationManager.AppSettings["BackupPath2"];
            string path3 = ConfigurationManager.AppSettings["BackupPath3"];

            if (!string.IsNullOrEmpty(path1)) paths.Add(path1);
            if (!string.IsNullOrEmpty(path2)) paths.Add(path2);
            if (!string.IsNullOrEmpty(path3)) paths.Add(path3);

            return paths;
        }
    }
}
