using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service.DAL.Implementations.SqlServer
{
    /// <summary>
    /// Gestiona las rutas y apertura de archivos de ayuda contextual según el idioma configurado.
    /// </summary>
    public class ManualRepository
    {
        private string helpFilePath;

        /// <summary>
        /// Crea el repositorio seleccionando la ruta de ayuda acorde al idioma indicado.
        /// </summary>
        /// <param name="languageCode">Código de idioma para resolver la ruta del archivo CHM.</param>
        public ManualRepository(string languageCode)
        {
            // Inicializa el helpFilePath basado en el idioma
            helpFilePath = GetHelpFilePath(languageCode);
        }

        /// <summary>
        /// Obtiene la ruta del archivo de ayuda correspondiente al idioma solicitado.
        /// </summary>
        /// <param name="languageCode">Código de idioma configurado.</param>
        /// <returns>Ruta absoluta del archivo de ayuda.</returns>
        public static string GetHelpFilePath(string languageCode)
        {
            string key = $"HelpFilePath_{languageCode}";
            string helpFilePath = ConfigurationManager.AppSettings[key];

            // Si no existe la clave para el idioma especificado, se usa la ruta por defecto
            if (string.IsNullOrEmpty(helpFilePath))
            {
                helpFilePath = ConfigurationManager.AppSettings["HelpFilePath_Default"];
            }

            return helpFilePath;
        }

        /// <summary>
        /// Despliega la ayuda de la pantalla principal.
        /// </summary>
        public void AbrirAyudaMain()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "20");
        }

        /// <summary>
        /// Abre la ayuda correspondiente al inicio de sesión.
        /// </summary>
        public void AbrirAyudaLogin()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "31");
        }

        /// <summary>
        /// Muestra la guía para cambiar la contraseña.
        /// </summary>
        public void AbrirAyudaCambiarPass()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "33");
        }

        /// <summary>
        /// Presenta la ayuda sobre el proceso de recuperación de contraseña.
        /// </summary>
        public void AbrirAyudaRecuperoPass()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "32");
        }

        /// <summary>
        /// Despliega la ayuda para asignar roles a los usuarios.
        /// </summary>
        public void AbrirAyudaAsignarRol()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "24");
        }

        /// <summary>
        /// Abre la sección de ayuda sobre la generación de respaldos.
        /// </summary>
        public void AbrirAyudaBackUp()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "22");
        }

        /// <summary>
        /// Muestra la ayuda relativa a la consulta de la bitácora.
        /// </summary>
        public void AbrirAyudaBitacora()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "25");
        }

        /// <summary>
        /// Presenta la guía para la creación de patentes.
        /// </summary>
        public void AbrirAyudaCrearPatente()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "27");
        }

        /// <summary>
        /// Despliega la ayuda para crear roles.
        /// </summary>
        public void AbrirAyudaCrearRol()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "26");
        }

        /// <summary>
        /// Abre la ayuda para registrar nuevos usuarios.
        /// </summary>
        public void AbrirAyudaCrearUsuario()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "23");
        }

        /// <summary>
        /// Muestra la ayuda asociada a la modificación de usuarios.
        /// </summary>
        public void AbrirAyudaModUsuario()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "28");
        }

        /// <summary>
        /// Despliega la ayuda para visualizar usuarios existentes.
        /// </summary>
        public void AbrirAyudaMostrarUsuario()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "29");
        }
    }
}

