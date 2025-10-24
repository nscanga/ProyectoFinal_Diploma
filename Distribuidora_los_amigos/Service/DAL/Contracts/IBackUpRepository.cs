using Service.DOMAIN.BackUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Contracts
{
    /// <summary>
    /// Define la funcionalidad necesaria para generar copias de seguridad.
    /// </summary>
    public interface IBackupRepository
    {
        /// <summary>
        /// Realiza una copia de respaldo de la base indicada hacia la ubicación solicitada.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión de origen.</param>
        /// <param name="backupPath">Ruta destino para el archivo generado.</param>
        void BackupDatabase(string connectionString, string backupPath);

        /// <summary>
        /// Restaura una base de datos desde un archivo de backup especificado.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión de la base de datos a restaurar.</param>
        /// <param name="backupFilePath">Ruta del archivo .bak a utilizar.</param>
        void RestoreDatabase(string connectionString, string backupFilePath);
    }
}
