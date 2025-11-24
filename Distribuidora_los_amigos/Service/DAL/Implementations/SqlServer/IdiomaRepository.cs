using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.DAL.Implementations
{
    /// <summary>
    /// Maneja la lectura y escritura de archivos de idioma configurables por usuario.
    /// </summary>
    internal static class LanguageRepository
    {
        /// <summary>
        /// Resuelve rutas relativas basándose en la ubicación del ejecutable.
        /// </summary>
        /// <param name="path">Ruta desde el App.config</param>
        /// <returns>Ruta absoluta resuelta</returns>
        private static string ResolvePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            // Si ya es absoluta, devolverla tal cual
            if (Path.IsPathRooted(path))
                return path;

            // Si es relativa, combinarla con la carpeta del ejecutable
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(Path.Combine(baseDirectory, path));
        }

        // Obtener las rutas desde el App.config y resolver rutas relativas
        private static string LanguagePath
        {
            get
            {
                string path = ConfigurationManager.AppSettings["IdiomaPath"];
                return ResolvePath(path);
            }
        }

        private static string UserIdiomaConfigPath
        {
            get
            {
                string path = ConfigurationManager.AppSettings["UserIdiomaConfigPath"];
                return ResolvePath(path);
            }
        }

        /// <summary>
        /// Traduce una clave textual utilizando el archivo correspondiente al idioma actual.
        /// </summary>
        /// <param name="key">Clave de la traducción buscada.</param>
        /// <returns>Cadena traducida encontrada.</returns>
        public static string Translate(string key)
        {
            string language = Thread.CurrentThread.CurrentUICulture.Name;
            string fileName = Path.Combine(LanguagePath, $"language.{language}");

            if (!File.Exists(fileName))
            {
                throw new Exception($"No se encontró el archivo de idioma para {language} en: {fileName}");
            }

            using (StreamReader reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;
                    
                    string[] columns = line.Split('=');
                    if (columns.Length >= 2 && columns[0].Trim().ToLower() == key.ToLower())
                    {
                        return columns[1].Trim();
                    }
                }
            }

            throw new Exception($"No se encontró la palabra {key} en el archivo de idioma {fileName}");
        }

        /// <summary>
        /// Punto de extensión para registrar nuevas claves de idioma (sin implementación actual).
        /// </summary>
        /// <param name="key">Clave que se podría persistir.</param>
        public static void WriteKey(string key)
        {
            string language = Thread.CurrentThread.CurrentUICulture.Name;
        }

        /// <summary>
        /// Obtiene la lista de idiomas disponibles (pendiente de implementación).
        /// </summary>
        /// <returns>Lista vacía hasta completar la lógica.</returns>
        public static List<string> GetLanguages()
        {
            return new List<string>();
        }

        /// <summary>
        /// Guarda el idioma preferido por el usuario en el archivo de configuración correspondiente.
        /// </summary>
        /// <param name="languageCode">Código de idioma a almacenar.</param>
        public static void SaveUserLanguage(string languageCode)
        {
            try
            {
                string filePath = UserIdiomaConfigPath;
                
                // Asegurar que el directorio exista
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Guardar el idioma
                File.WriteAllText(filePath, languageCode);
            }
            catch (UnauthorizedAccessException)
            {
                // Si no tiene permisos en esa carpeta, intentar guardar en AppData del usuario
                string appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "DistribuidoraLosAmigos",
                    "user_language.config"
                );
                
                string directory = Path.GetDirectoryName(appDataPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                File.WriteAllText(appDataPath, languageCode);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar el idioma del usuario: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Carga el idioma almacenado para el usuario o retorna español como valor predeterminado.
        /// </summary>
        /// <returns>Código de idioma configurado.</returns>
        public static string LoadUserLanguage()
        {
            try
            {
                string filePath = UserIdiomaConfigPath;
                
                // Intentar leer del archivo configurado
                if (File.Exists(filePath))
                {
                    string languageCode = File.ReadAllText(filePath).Trim();
                    if (!string.IsNullOrEmpty(languageCode))
                    {
                        return languageCode;
                    }
                }
                
                // Si no existe, intentar leer del AppData
                string appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "DistribuidoraLosAmigos",
                    "user_language.config"
                );
                
                if (File.Exists(appDataPath))
                {
                    string languageCode = File.ReadAllText(appDataPath).Trim();
                    if (!string.IsNullOrEmpty(languageCode))
                    {
                        return languageCode;
                    }
                }
            }
            catch (Exception)
            {
                // Si hay cualquier error, continuar con idioma por defecto
            }

            // Si no existe el archivo o hay error, retorna "es-ES" como idioma predeterminado
            return "es-ES";
        }
    }
}