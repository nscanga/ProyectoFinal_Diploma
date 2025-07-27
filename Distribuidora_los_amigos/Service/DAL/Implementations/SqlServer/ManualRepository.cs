using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service.DAL.Implementations.SqlServer
{
    public class ManualRepository
    {
        private string helpFilePath;

        public ManualRepository(string languageCode)
        {
            // Inicializa el helpFilePath basado en el idioma
            helpFilePath = GetHelpFilePath(languageCode);
        }

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

        public void AbrirAyudaAltaPaciente()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "2");
        }
        public void AbrirAyudaDeshPaciente()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "4");
        }
        public void AbrirAyudaModPaciente()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "3");
        }
        public void AbrirAyudaVerPaciente()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "5");
        }




        public void AbrirAyudaAltaProfesional()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "7");
        }

        public void AbrirAyudaDeshProfesional()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "9");
        }

        public void AbrirAyudaModProfesional()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "8");
        }

        public void AbrirAyudaVerProfesional()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "10");
        }

        public void AbrirAyudaAltaTratamiento()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "12");
        }
        public void AbrirAyudaAltaEtapa()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "13");
        }


        public void AbrirAyudaAltaCita()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "15");
        }
        public void AbrirAyudaRegistroCita()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "16");
        }

        public void AbrirAyudaListarCitas()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "17");
        }







        public void AbrirAyudaHistorial()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "18");
        }
        public void AbrirAyudaHorarios()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "19");
        }
        public void AbrirAyudaMain()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "20");
        }



        public void AbrirAyudaLogin()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "31");
        }
        public void AbrirAyudaCambiarPass()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "33");
        }
        public void AbrirAyudaRecuperoPass()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "32");
        }




        public void AbrirAyudaAsignarRol()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "24");
        }
        public void AbrirAyudaBackUp()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "22");
        }
        public void AbrirAyudaBitacora()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "25");
        }
        public void AbrirAyudaCrearPatente()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "27");
        }
        public void AbrirAyudaCrearRol()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "26");
        }
        public void AbrirAyudaCrearUsuario()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "23");
        }
        public void AbrirAyudaModUsuario()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "28");
        }
        public void AbrirAyudaMostrarUsuario()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "29");
        }
    }
}

