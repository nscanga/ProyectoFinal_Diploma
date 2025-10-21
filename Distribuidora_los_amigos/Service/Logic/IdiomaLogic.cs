using Service.DAL.Implementations;
using Service.Facade;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service.Logic
{
    /// <summary>
    /// Agrupa utilidades para traducir controles e interactuar con la configuración de idiomas.
    /// </summary>
    internal static class IdiomaLogic
    {
        /// <summary>
        /// Traduce una clave textual usando el repositorio de idiomas configurado.
        /// </summary>
        /// <param name="key">Clave de traducción.</param>
        /// <returns>Texto traducido o la clave original en caso de error.</returns>
        public static string Translate(string key)
        {
            try
            {
                // ✅ Usar tu LanguageRepository existente que lee archivos de texto
                return LanguageRepository.Translate(key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error translating key '{key}': {ex.Message}");
                return key; // Retorna la clave original si no se encuentra
            }
        }

        /// <summary>
        /// Recorre recursivamente los controles traduciendo sus textos según la etiqueta configurada.
        /// </summary>
        /// <param name="control">Control raíz del formulario a traducir.</param>
        public static void TranslateForm(Control control)
        {
            try
            {
                foreach (Control ctrl in control.Controls)
                {
                    if (ctrl.Tag != null && !string.IsNullOrEmpty(ctrl.Tag.ToString()))
                    {
                        ctrl.Text = Translate(ctrl.Tag.ToString());
                    }

                    if (ctrl.HasChildren)
                    {
                        TranslateForm(ctrl);
                    }

                    if (ctrl is MenuStrip menuStrip)
                    {
                        foreach (ToolStripMenuItem item in menuStrip.Items)
                        {
                            TranslateMenuItem(item);
                        }
                    }
                }

                if (control is MenuStrip menuStripDirect)
                {
                    foreach (ToolStripMenuItem item in menuStripDirect.Items)
                    {
                        TranslateMenuItem(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error translating form: {ex.Message}");
            }
        }

        /// <summary>
        /// Aplica la traducción a un ítem de menú y a todos sus descendientes.
        /// </summary>
        /// <param name="menuItem">Elemento de menú a traducir.</param>
        private static void TranslateMenuItem(ToolStripMenuItem menuItem)
        {
            try
            {
                if (menuItem.Tag != null && !string.IsNullOrEmpty(menuItem.Tag.ToString()))
                {
                    menuItem.Text = Translate(menuItem.Tag.ToString());
                }

                foreach (ToolStripItem subItem in menuItem.DropDownItems)
                {
                    if (subItem is ToolStripMenuItem subMenuItem)
                    {
                        TranslateMenuItem(subMenuItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error translating menu item: {ex.Message}");
            }
        }

        /// <summary>
        /// Guarda el idioma seleccionado por el usuario.
        /// </summary>
        /// <param name="languageCode">Código de cultura a persistir.</param>
        public static void SaveUserLanguage(string languageCode)
        {
            try
            {
                // ✅ Usar tu LanguageRepository existente
                LanguageRepository.SaveUserLanguage(languageCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving language: {ex.Message}");
            }
        }

        /// <summary>
        /// Recupera el idioma previamente almacenado o retorna el valor por defecto.
        /// </summary>
        /// <returns>Código de idioma a utilizar.</returns>
        public static string LoadUserLanguage()
        {
            try
            {
                // ✅ Usar tu LanguageRepository existente
                return LanguageRepository.LoadUserLanguage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading language: {ex.Message}");
                return "es-ES";
            }
        }
    }
}
