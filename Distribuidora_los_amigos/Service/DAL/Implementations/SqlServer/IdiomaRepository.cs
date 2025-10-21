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
        // Obtener las rutas desde el App.config
        private static string LanguagePath = ConfigurationManager.AppSettings["IdiomaPath"];
        private static readonly string UserIdiomaConfigPath = ConfigurationManager.AppSettings["UserIdiomaConfigPath"];



        /// <summary>
        /// Traduce una clave textual utilizando el archivo correspondiente al idioma actual.
        /// </summary>
        /// <param name="key">Clave de la traducción buscada.</param>
        /// <returns>Cadena traducida encontrada.</returns>
        public static string Translate(string key)
        {
            // Obtener el código de idioma actual (es-ES, en-US, etc.)
            string language = Thread.CurrentThread.CurrentUICulture.Name;

            // Construir el nombre completo del archivo basado en el idioma (language.es-ES, language.en-US)
            string fileName = Path.Combine(LanguagePath, $"language.{language}");

            // Verificar que el archivo de idioma existe
            if (!File.Exists(fileName))
            {

                throw new Exception($"No se encontró el archivo de idioma para {language}");
            }

            // Leer el archivo y buscar la clave
            using (StreamReader reader = new StreamReader(fileName))
            {

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] columns = line.Split('=');

                    if (columns[0].ToLower() == key.ToLower())
                    {
                        return columns[1]; // Retorna la traducción
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
            using (StreamWriter writer = new StreamWriter(UserIdiomaConfigPath, false))  // Sobrescribe el archivo
            {
                writer.WriteLine(languageCode);  // Guarda el código del idioma
            }
        }

        /// <summary>
        /// Carga el idioma almacenado para el usuario o retorna español como valor predeterminado.
        /// </summary>
        /// <returns>Código de idioma configurado.</returns>
        public static string LoadUserLanguage()
        {
            if (File.Exists(UserIdiomaConfigPath))
            {
                using (StreamReader reader = new StreamReader(UserIdiomaConfigPath))
                {
                    string languageCode = reader.ReadLine();
                    if (!string.IsNullOrEmpty(languageCode))
                    {
                        return languageCode;  // Retorna el idioma guardado
                    }
                }
            }

            // Si no existe el archivo o no tiene un valor, retorna "es-ES" como idioma predeterminado
            return "es-ES";
        }




    }
}
