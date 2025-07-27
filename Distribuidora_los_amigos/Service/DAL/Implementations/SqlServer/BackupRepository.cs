using Service.DAL.Contracts;
using Service.DOMAIN.BackUp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Implementations
{
    public class BackupRepository : IBackupRepository
    {
        public void BackupDatabase(string connectionString, string backupPath)
        {
            // Extrae el nombre de la base de datos del connectionString
            var builder = new SqlConnectionStringBuilder(connectionString);
            string databaseName = builder.InitialCatalog;

            string formattedDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            // Crea el nombre del archivo de backup
            string backupFileName = $"{databaseName}_{formattedDate}.bak";
            string fullBackupPath = Path.Combine(backupPath, backupFileName);

            // Ejecuta el backup de la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"BACKUP DATABASE [{databaseName}] TO DISK = '{fullBackupPath}' WITH INIT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}