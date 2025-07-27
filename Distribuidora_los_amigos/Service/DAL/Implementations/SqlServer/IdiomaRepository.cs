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
    internal static class LanguageRepository
    {
        // Obtener las rutas desde el App.config
        private static string LanguagePath = ConfigurationManager.AppSettings["IdiomaPath"];
        private static readonly string UserIdiomaConfigPath = ConfigurationManager.AppSettings["UserIdiomaConfigPath"];



        // Método para traducir una clave basada en el idioma actual
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

        public static void WriteKey(string key)
        {
            string language = Thread.CurrentThread.CurrentUICulture.Name;

        }

        public static List<string> GetLanguages()
        {
            return new List<string>();
        }



        // Método para guardar el idioma seleccionado en un archivo de configuración
        public static void SaveUserLanguage(string languageCode)
        {
            using (StreamWriter writer = new StreamWriter(UserIdiomaConfigPath, false))  // Sobrescribe el archivo
            {
                writer.WriteLine(languageCode);  // Guarda el código del idioma
            }
        }

        // Método para cargar el idioma desde el archivo de configuración
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
