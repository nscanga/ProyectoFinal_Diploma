using Service.DAL.Implementations;
using Service.DAL.Implementations.SqlServer;
using Service.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service.Logic
{
    internal static class IdiomaLogic
    {
        public static string Translate(string key)
        {
            try
            {
                // Simplemente llamar a la DAO para traducir la clave
                return LanguageRepository.Translate(key);
            }
            catch (Exception ex)
            {
                // Registrar cualquier excepción, si es necesario
                Console.WriteLine(ex.Message);
                //LoggerService.WriteException(ex);
            }

            return key; // Si ocurre un error, se retorna la clave original
        }

        public static void TranslateForm(Control control)
        {
            foreach (Control ctrl in control.Controls)
            {
                if (ctrl.Tag != null && !string.IsNullOrEmpty(ctrl.Tag.ToString()))
                {
                    ctrl.Text = Translate(ctrl.Tag.ToString());
                }

                // Traducir recursivamente subcontroles
                if (ctrl.HasChildren)
                {
                    TranslateForm(ctrl);
                }

                // Verificar si es un MenuStrip
                if (ctrl is MenuStrip menuStrip)
                {
                    foreach (ToolStripMenuItem item in menuStrip.Items)
                    {
                        TranslateMenuItem(item);
                    }
                }
                if (ctrl is MessageBox)
                {
                    string Message = ctrl.Tag.ToString();
                    if (!string.IsNullOrEmpty(Message))
                    {
                        Message = Translate(Message);
                        MessageBox.Show(Message);
                    }
                }
            }

            // Si el control en sí es un MenuStrip
            if (control is MenuStrip menuStripDirect)
            {
                foreach (ToolStripMenuItem item in menuStripDirect.Items)
                {
                    TranslateMenuItem(item);
                }
            }
        }

        private static void TranslateMenuItem(ToolStripMenuItem menuItem)
        {
            if (menuItem.Tag != null && !string.IsNullOrEmpty(menuItem.Tag.ToString()))
            {
                menuItem.Text = Translate(menuItem.Tag.ToString());
            }

            // Traducir recursivamente subitems
            foreach (ToolStripItem subItem in menuItem.DropDownItems)
            {
                if (subItem is ToolStripMenuItem subMenuItem)
                {
                    TranslateMenuItem(subMenuItem);
                }
            }
        }
        public static void ShowTranslatedMessage(string messageKey)
        {
            string translatedMessage = Translate(messageKey);
            
        }
        // Guardar el idioma seleccionado llamando a la capa de acceso a datos (DAO)
        public static void SaveUserLanguage(string languageCode)
        {
            LanguageRepository.SaveUserLanguage(languageCode);
           
        }

        // Cargar el idioma seleccionado desde la capa de acceso a datos (DAO)
        public static string LoadUserLanguage()
        {
            return LanguageRepository.LoadUserLanguage();
        }



    }
}
