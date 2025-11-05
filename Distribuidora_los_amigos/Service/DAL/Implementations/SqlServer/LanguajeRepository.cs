using System;
using System.IO;
using System.Resources;
using System.Globalization;
using System.Collections.Generic;

namespace Service.DAL.Implementations
{
    /// <summary>
    /// Proporciona traducciones básicas y persistencia de idioma seleccionado para el usuario.
    /// </summary>
    public static class LenguajeRepository // ✅ Nombre correcto
    {
        private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "language.config");
        
        /// <summary>
        /// Devuelve la traducción correspondiente a la clave indicada según el idioma activo.
        /// </summary>
        /// <param name="key">Identificador del texto a traducir.</param>
        /// <returns>Cadena traducida o la clave original si no existe traducción.</returns>
        public static string Translate(string key)
        {
            string currentLanguage = LoadUserLanguage();
            
            // Las traducciones ahora se manejan en los archivos de texto
            // Este método se mantiene por compatibilidad
            return key;
        }

        /// <summary>
        /// Persiste el código de idioma seleccionado por el usuario en el archivo de configuración.
        /// </summary>
        /// <param name="languageCode">Código de cultura a guardar.</param>
        public static void SaveUserLanguage(string languageCode)
        {
            try
            {
                File.WriteAllText(ConfigFilePath, languageCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving language: {ex.Message}");
            }
        }

        /// <summary>
        /// Recupera el idioma almacenado en disco o retorna el español por defecto.
        /// </summary>
        /// <returns>Código de cultura a utilizar.</returns>
        public static string LoadUserLanguage()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    return File.ReadAllText(ConfigFilePath).Trim();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading language: {ex.Message}");
            }
            
            return "es-ES"; // Idioma por defecto
        }
    }
}