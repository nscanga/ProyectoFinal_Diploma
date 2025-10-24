using Service.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Service.Logic
{
    /// <summary>
    /// Orquesta la generación de respaldos para las distintas bases de datos del sistema.
    /// </summary>
    public class BackUpLogic
    {
        private readonly IBackupRepository _backupRepository;

        /// <summary>
        /// Inicializa la lógica de backup con el repositorio a utilizar.
        /// </summary>
        /// <param name="backupRepository">Repositorio encargado de ejecutar el respaldo.</param>
        public BackUpLogic(IBackupRepository backupRepository)
        {
            _backupRepository = backupRepository ?? throw new ArgumentNullException(nameof(backupRepository));
        }

        /// <summary>
        /// Ejecuta la copia de seguridad de todas las bases configuradas hacia la ruta indicada.
        /// </summary>
        /// <param name="selectedPath">Directorio destino para almacenar los archivos .bak.</param>
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

        /// <summary>
        /// Restaura todas las bases de datos del sistema desde los archivos de backup especificados.
        /// </summary>
        /// <param name="backupFolderPath">Carpeta que contiene los archivos .bak a restaurar.</param>
        public void PerformRestore(string backupFolderPath)
        {
            try
            {
                // ✅ Validar que la ruta existe
                if (string.IsNullOrWhiteSpace(backupFolderPath))
                {
                    throw new ArgumentException("Debe seleccionar una carpeta válida con los archivos de backup.");
                }

                if (!Directory.Exists(backupFolderPath))
                {
                    throw new DirectoryNotFoundException($"No se encontró la carpeta: {backupFolderPath}");
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

                // ✅ Buscar archivos de backup (intentar con diferentes nombres posibles)
                string backupFile1 = FindBackupFile(backupFolderPath, databaseName1);
                string backupFile2 = FindBackupFile(backupFolderPath, databaseName2);
                string backupFile3 = FindBackupFile(backupFolderPath, databaseName3);

                // ✅ Validar que existan los archivos
                if (string.IsNullOrEmpty(backupFile1))
                    throw new FileNotFoundException($"No se encontró el archivo de backup para la base de datos: {databaseName1}");
                if (string.IsNullOrEmpty(backupFile2))
                    throw new FileNotFoundException($"No se encontró el archivo de backup para la base de datos: {databaseName2}");
                if (string.IsNullOrEmpty(backupFile3))
                    throw new FileNotFoundException($"No se encontró el archivo de backup para la base de datos: {databaseName3}");

                // ✅ Restaurar cada base de datos
                _backupRepository.RestoreDatabase(connectionString1, backupFile1);
                _backupRepository.RestoreDatabase(connectionString2, backupFile2);
                _backupRepository.RestoreDatabase(connectionString3, backupFile3);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al ejecutar la restauración: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Busca un archivo de backup en la carpeta especificada que coincida con el nombre de la base de datos.
        /// </summary>
        /// <param name="folderPath">Carpeta donde buscar.</param>
        /// <param name="databaseName">Nombre de la base de datos.</param>
        /// <returns>Ruta completa del archivo encontrado o null si no existe.</returns>
        private string FindBackupFile(string folderPath, string databaseName)
        {
            // Buscar primero el archivo con el nombre exacto
            string exactMatch = Path.Combine(folderPath, $"{databaseName}.bak");
            if (File.Exists(exactMatch))
                return exactMatch;

            // Buscar archivos que contengan el nombre de la base de datos
            string[] files = Directory.GetFiles(folderPath, $"{databaseName}*.bak");
            if (files.Length > 0)
            {
                // Retornar el más reciente si hay varios
                Array.Sort(files);
                return files[files.Length - 1];
            }

            return null;
        }

        /// <summary>
        /// Obtiene las rutas de respaldo sugeridas desde la configuración.
        /// </summary>
        /// <returns>Lista de directorios configurados.</returns>
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
