using Service.DAL.Contracts;
using System;
using System.Data.SqlClient;
using System.IO;

namespace Service.DAL.Implementations
{
    /// <summary>
    /// Gestiona la creación de copias de seguridad de la base de datos y su almacenamiento local.
    /// </summary>
    public class BackupRepository : IBackupRepository
    {
        /// <summary>
        /// Ejecuta un backup completo de la base de datos y coloca el archivo resultante en la carpeta indicada.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión hacia la instancia de SQL Server.</param>
        /// <param name="backupPath">Ruta destino solicitada por el usuario.</param>
        public void BackupDatabase(string connectionString, string backupPath)
        {
            SqlConnection connection = null;
            string sqlServerBackupPath = null;
            
            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                string databaseName = builder.InitialCatalog;

                if (string.IsNullOrEmpty(databaseName))
                {
                    throw new InvalidOperationException("El nombre de la base de datos no puede estar vacío.");
                }

                // ✅ Obtener la carpeta de backup de SQL Server
                string sqlBackupFolder = GetSqlServerBackupFolder(connectionString);
                
                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string backupFileName = $"{databaseName}_{formattedDate}.bak";
                
                // ✅ IMPORTANTE: SQL Server en Linux usa rutas Linux
                // Ruta donde SQL Server creará el backup (formato Linux)
                sqlServerBackupPath = $"{sqlBackupFolder}/{backupFileName}";
                
                // ✅ Ruta final donde el usuario quiere el backup (formato Windows)
                string userBackupDirectory = Path.GetDirectoryName(backupPath);
                string userBackupPath = Path.Combine(userBackupDirectory, backupFileName);
                
                // Crear directorio del usuario si no existe
                if (!Directory.Exists(userBackupDirectory))
                {
                    Directory.CreateDirectory(userBackupDirectory);
                }

                connection = new SqlConnection(connectionString);
                connection.Open();

                // ✅ Hacer backup en la carpeta de SQL Server Linux
                string backupQuery = $@"
                    BACKUP DATABASE [{databaseName}] 
                    TO DISK = N'{sqlServerBackupPath}' 
                    WITH INIT, FORMAT, COMPRESSION, 
                    NAME = N'{databaseName}-Full Backup',
                    STATS = 10";
                
                using (SqlCommand command = new SqlCommand(backupQuery, connection))
                {
                    command.CommandTimeout = 600;
                    command.ExecuteNonQuery();
                }

                // ✅ Ahora leer el archivo desde SQL Server y guardarlo localmente
                string readBackupQuery = $@"
                    SELECT BulkColumn 
                    FROM OPENROWSET(BULK N'{sqlServerBackupPath}', SINGLE_BLOB) AS BackupFile";
                
                using (SqlCommand readCommand = new SqlCommand(readBackupQuery, connection))
                {
                    byte[] backupData = (byte[])readCommand.ExecuteScalar();
                    
                    if (backupData != null && backupData.Length > 0)
                    {
                        // ✅ Guardar el archivo en la ubicación del usuario
                        File.WriteAllBytes(userBackupPath, backupData);
                    }
                    else
                    {
                        throw new Exception("No se pudo leer el archivo de backup desde el servidor.");
                    }
                }

                // ✅ Limpiar el archivo temporal en SQL Server
                try
                {
                    string deleteQuery = $"EXEC xp_cmdshell 'rm {sqlServerBackupPath}'";
                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }
                }
                catch
                {
                    // Ignorar si no se puede eliminar
                }

                connection.Close();
            }
            catch (SqlException sqlEx)
            {
                string errorMessage = $"Error de SQL Server al hacer backup:\n{sqlEx.Message}\n\n";
                
                if (sqlEx.Message.Contains("OPENROWSET"))
                {
                    errorMessage += "NOTA: La función OPENROWSET puede no estar habilitada.\n";
                    errorMessage += $"El backup se creó en el servidor en: {sqlServerBackupPath}\n\n";
                    errorMessage += "Contacte al administrador para obtener el archivo.";
                }
                
                throw new Exception(errorMessage, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al realizar el backup: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Obtiene la carpeta de backups configurada en la instancia de SQL Server o una ruta por defecto.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión utilizada para consultar la instancia.</param>
        /// <returns>Ruta del directorio de backups del servidor.</returns>
        private string GetSqlServerBackupFolder(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    string query = @"
                        EXEC master.dbo.xp_instance_regread 
                            N'HKEY_LOCAL_MACHINE',
                            N'Software\Microsoft\MSSQLServer\MSSQLServer',
                            N'BackupDirectory'";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read() && !reader.IsDBNull(1))
                            {
                                return reader.GetString(1);
                            }
                        }
                    }
                }
            }
            catch
            {
                // Si falla, usar ruta por defecto de Linux
            }

            // ✅ Ruta por defecto de SQL Server en Linux
            return "/var/opt/mssql/data";
        }
    }
}