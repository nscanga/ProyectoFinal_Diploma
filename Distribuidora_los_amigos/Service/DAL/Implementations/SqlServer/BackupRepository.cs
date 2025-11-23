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

                connection = new SqlConnection(connectionString);
                connection.Open();

                // ✅ Detectar si la edición de SQL Server soporta compresión
                bool supportsCompression = SupportsBackupCompression(connection);

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

                // ✅ Construir el comando de backup con o sin compresión según la edición
                string compressionOption = supportsCompression ? "COMPRESSION," : "";
                
                string backupQuery = $@"
                    BACKUP DATABASE [{databaseName}] 
                    TO DISK = N'{sqlServerBackupPath}' 
                    WITH INIT, FORMAT, {compressionOption}
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
        /// Restaura una base de datos desde un archivo de backup, desconectando primero todas las sesiones activas.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión hacia la instancia de SQL Server.</param>
        /// <param name="backupFilePath">Ruta completa del archivo .bak a restaurar.</param>
        public void RestoreDatabase(string connectionString, string backupFilePath)
        {
            SqlConnection connection = null;
            string sqlServerRestorePath = null;

            try
            {
                // ✅ Validar que el archivo existe
                if (!File.Exists(backupFilePath))
                {
                    throw new FileNotFoundException($"No se encontró el archivo de backup: {backupFilePath}");
                }

                var builder = new SqlConnectionStringBuilder(connectionString);
                string databaseName = builder.InitialCatalog;

                if (string.IsNullOrEmpty(databaseName))
                {
                    throw new InvalidOperationException("El nombre de la base de datos no puede estar vacío.");
                }

                // ✅ Conectar al servidor (a la base master, no a la que vamos a restaurar)
                builder.InitialCatalog = "master";
                connection = new SqlConnection(builder.ConnectionString);
                connection.Open();

                // ✅ Obtener la carpeta de backup de SQL Server
                string sqlBackupFolder = GetSqlServerBackupFolder(builder.ConnectionString);
                string backupFileName = Path.GetFileName(backupFilePath);
                
                // ✅ Determinar la ruta correcta según el sistema operativo del servidor
                // Para Windows usar backslash, para Linux usar forward slash
                bool isLinuxServer = sqlBackupFolder.StartsWith("/");
                sqlServerRestorePath = isLinuxServer 
                    ? $"{sqlBackupFolder}/{backupFileName}"
                    : Path.Combine(sqlBackupFolder, backupFileName);

                // ✅ Copiar el archivo de backup a la carpeta de SQL Server
                // Leer el archivo localmente
                byte[] backupData = File.ReadAllBytes(backupFilePath);

                // ✅ Escribir el archivo en la ubicación de SQL Server usando FileStream remoto
                // Esto se hace mediante BCP o comandos de SQL Server
                string writeFileQuery = GenerateWriteFileQuery(sqlServerRestorePath, backupData, isLinuxServer);
                
                if (!string.IsNullOrEmpty(writeFileQuery))
                {
                    using (SqlCommand writeCommand = new SqlCommand(writeFileQuery, connection))
                    {
                        writeCommand.CommandTimeout = 600;
                        writeCommand.ExecuteNonQuery();
                    }
                }
                else
                {
                    // ✅ Método alternativo: Usar BulkCopy para transferir el archivo
                    TransferBackupFileToServer(connection, backupData, sqlServerRestorePath, isLinuxServer);
                }

                // ✅ Paso 1: Poner la base de datos en modo de usuario único y cerrar todas las conexiones
                string setSingleUserQuery = $@"
                    IF EXISTS (SELECT name FROM sys.databases WHERE name = N'{databaseName}')
                    BEGIN
                        ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    END";

                using (SqlCommand command = new SqlCommand(setSingleUserQuery, connection))
                {
                    command.CommandTimeout = 600;
                    command.ExecuteNonQuery();
                }

                // ✅ Paso 2: Ejecutar el RESTORE desde la carpeta de SQL Server
                string restoreQuery = $@"
                    RESTORE DATABASE [{databaseName}] 
                    FROM DISK = N'{sqlServerRestorePath}' 
                    WITH REPLACE, 
                         RECOVERY,
                         STATS = 10";

                using (SqlCommand command = new SqlCommand(restoreQuery, connection))
                {
                    command.CommandTimeout = 600;
                    command.ExecuteNonQuery();
                }

                // ✅ Paso 3: Volver la base de datos a modo multiusuario
                string setMultiUserQuery = $@"
                    ALTER DATABASE [{databaseName}] SET MULTI_USER";

                using (SqlCommand command = new SqlCommand(setMultiUserQuery, connection))
                {
                    command.CommandTimeout = 600;
                    command.ExecuteNonQuery();
                }

                // ✅ Limpiar el archivo temporal en SQL Server
                try
                {
                    if (isLinuxServer)
                    {
                        string deleteQuery = $"EXEC xp_cmdshell 'rm {sqlServerRestorePath}'";
                        using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                        {
                            deleteCommand.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string deleteQuery = $"EXEC xp_cmdshell 'del \"{sqlServerRestorePath}\"'";
                        using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                        {
                            deleteCommand.ExecuteNonQuery();
                        }
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
                string errorMessage = $"Error de SQL Server al restaurar la base de datos:\n{sqlEx.Message}\n\n";

                if (sqlEx.Message.Contains("is being used by"))
                {
                    errorMessage += "NOTA: La base de datos está en uso. Cierre todas las conexiones activas antes de restaurar.";
                }
                else if (sqlEx.Message.Contains("Cannot open backup device") || sqlEx.Message.Contains("Operating system error 5"))
                {
                    errorMessage += "NOTA: SQL Server no tiene permisos para acceder al archivo de backup.\n";
                    errorMessage += "El sistema intentó copiar el archivo a una ubicación accesible pero falló.\n";
                    errorMessage += "Verifique los permisos o contacte al administrador del sistema.";
                }
                else if (sqlEx.Message.Contains("cannot be opened"))
                {
                    errorMessage += "NOTA: Verifique que SQL Server tenga permisos de lectura sobre el archivo de backup.";
                }

                throw new Exception(errorMessage, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al restaurar la base de datos: {ex.Message}", ex);
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
        /// Genera un query para escribir un archivo en el servidor SQL Server.
        /// </summary>
        private string GenerateWriteFileQuery(string targetPath, byte[] data, bool isLinuxServer)
        {
            // Este método es complejo y depende de xp_cmdshell estar habilitado
            // Por ahora retornamos null para usar el método alternativo
            return null;
        }

        /// <summary>
        /// Transfiere el archivo de backup al servidor SQL Server usando técnicas alternativas.
        /// </summary>
        private void TransferBackupFileToServer(SqlConnection connection, byte[] backupData, string targetPath, bool isLinuxServer)
        {
            try
            {
                // ✅ Método 1: Intentar habilitar Ole Automation Procedures temporalmente
                string enableOleQuery = @"
                    EXEC sp_configure 'show advanced options', 1;
                    RECONFIGURE;
                    EXEC sp_configure 'Ole Automation Procedures', 1;
                    RECONFIGURE;";
                
                try
                {
                    using (SqlCommand enableCmd = new SqlCommand(enableOleQuery, connection))
                    {
                        enableCmd.CommandTimeout = 60;
                        enableCmd.ExecuteNonQuery();
                    }

                    // ✅ Usar Ole Automation para escribir el archivo
                    string writeFileOleQuery = $@"
                        DECLARE @ObjectToken INT;
                        DECLARE @FileContent VARBINARY(MAX) = {GetHexString(backupData)};
                        
                        EXEC sp_OACreate 'ADODB.Stream', @ObjectToken OUTPUT;
                        EXEC sp_OASetProperty @ObjectToken, 'Type', 1;
                        EXEC sp_OAMethod @ObjectToken, 'Open';
                        EXEC sp_OAMethod @ObjectToken, 'Write', NULL, @FileContent;
                        EXEC sp_OAMethod @ObjectToken, 'SaveToFile', NULL, '{targetPath}', 2;
                        EXEC sp_OAMethod @ObjectToken, 'Close';
                        EXEC sp_OADestroy @ObjectToken;";

                    using (SqlCommand writeCmd = new SqlCommand(writeFileOleQuery, connection))
                    {
                        writeCmd.CommandTimeout = 600;
                        writeCmd.ExecuteNonQuery();
                    }
                }
                catch
                {
                    // ✅ Si falla Ole Automation, intentar con xp_cmdshell y PowerShell
                    TransferUsingPowerShell(connection, backupData, targetPath, isLinuxServer);
                }
                finally
                {
                    // Deshabilitar Ole Automation por seguridad
                    string disableOleQuery = @"
                        EXEC sp_configure 'Ole Automation Procedures', 0;
                        RECONFIGURE;";
                    
                    try
                    {
                        using (SqlCommand disableCmd = new SqlCommand(disableOleQuery, connection))
                        {
                            disableCmd.CommandTimeout = 60;
                            disableCmd.ExecuteNonQuery();
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"No se pudo transferir el archivo de backup al servidor: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Transfiere el archivo usando PowerShell vía xp_cmdshell.
        /// </summary>
        private void TransferUsingPowerShell(SqlConnection connection, byte[] backupData, string targetPath, bool isLinuxServer)
        {
            if (isLinuxServer)
            {
                throw new NotSupportedException("La transferencia automática de archivos no está soportada en SQL Server en Linux. " +
                    "Por favor, copie manualmente el archivo de backup a la carpeta de SQL Server.");
            }

            // ✅ Convertir bytes a base64 para transferir vía PowerShell
            string base64Data = Convert.ToBase64String(backupData);
            
            // ✅ Dividir en chunks para evitar límites de comando
            const int chunkSize = 8000; // caracteres por comando
            int chunks = (int)Math.Ceiling((double)base64Data.Length / chunkSize);

            // ✅ Habilitar xp_cmdshell temporalmente
            string enableXpCmd = @"
                EXEC sp_configure 'show advanced options', 1;
                RECONFIGURE;
                EXEC sp_configure 'xp_cmdshell', 1;
                RECONFIGURE;";
            
            using (SqlCommand enableCmd = new SqlCommand(enableXpCmd, connection))
            {
                enableCmd.CommandTimeout = 60;
                enableCmd.ExecuteNonQuery();
            }

            try
            {
                // ✅ Crear archivo temporal para el script PowerShell
                string tempScriptPath = Path.Combine(Path.GetTempPath(), $"restore_transfer_{Guid.NewGuid()}.ps1");
                string psScript = $@"
                    $base64 = '{base64Data}'
                    $bytes = [Convert]::FromBase64String($base64)
                    [System.IO.File]::WriteAllBytes('{targetPath}', $bytes)
                ";

                // ✅ Escribir el script localmente
                File.WriteAllText(tempScriptPath, psScript);

                // ✅ Ejecutar PowerShell para copiar el archivo
                string psCommand = $"powershell.exe -ExecutionPolicy Bypass -File \"{tempScriptPath}\"";
                string execQuery = $"EXEC xp_cmdshell '{psCommand}'";

                using (SqlCommand execCmd = new SqlCommand(execQuery, connection))
                {
                    execCmd.CommandTimeout = 600;
                    execCmd.ExecuteNonQuery();
                }

                // ✅ Limpiar script temporal
                try
                {
                    File.Delete(tempScriptPath);
                }
                catch { }
            }
            finally
            {
                // ✅ Deshabilitar xp_cmdshell por seguridad
                string disableXpCmd = @"
                    EXEC sp_configure 'xp_cmdshell', 0;
                    RECONFIGURE;";
                
                try
                {
                    using (SqlCommand disableCmd = new SqlCommand(disableXpCmd, connection))
                    {
                        disableCmd.CommandTimeout = 60;
                        disableCmd.ExecuteNonQuery();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Convierte un array de bytes a representación hexadecimal para SQL Server.
        /// </summary>
        private string GetHexString(byte[] data)
        {
            if (data == null || data.Length == 0)
                return "0x";

            return "0x" + BitConverter.ToString(data).Replace("-", "");
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

        /// <summary>
        /// Determina si la instancia de SQL Server soporta compresión de backups.
        /// SQL Server Express Edition no soporta compresión.
        /// </summary>
        /// <param name="connection">Conexión abierta a SQL Server.</param>
        /// <returns>True si soporta compresión, False en caso contrario.</returns>
        private bool SupportsBackupCompression(SqlConnection connection)
        {
            try
            {
                string query = @"
                    SELECT CASE 
                        WHEN SERVERPROPERTY('EngineEdition') = 4 THEN 0  -- Express Edition
                        WHEN SERVERPROPERTY('EngineEdition') = 2 THEN 0  -- Standard Edition (algunas versiones)
                        ELSE 1
                    END AS SupportsCompression";
                
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    return result != null && Convert.ToInt32(result) == 1;
                }
            }
            catch
            {
                // Si hay error al detectar, asumir que NO soporta compresión (más seguro)
                return false;
            }
        }
    }
}