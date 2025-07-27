using Service.DAL.Implementations.SqlServer;
using Service.Logic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Facade
{
    public class ManualService
    {
        private ManualRepository manualRepository;

        public ManualService()
        {
            // Obtener el código de idioma de la configuración o de otro lugar
            string languageCode = IdiomaLogic.LoadUserLanguage();
            manualRepository = new ManualRepository(languageCode);
        }
        public void AbrirAyudaAltaPaciente()
        {
            manualRepository.AbrirAyudaAltaPaciente();
        }

        public void AbrirAyudaModPaciente()
        {
            manualRepository.AbrirAyudaModPaciente();
        }

        public void AbrirAyudaDeshPaciente()
        {
            manualRepository.AbrirAyudaDeshPaciente();
        }
        public void AbrirAyudaVerPaciente()
        {
            manualRepository.AbrirAyudaVerPaciente();
        }


        public void AbrirAyudaAltaProfesional()
        {
            manualRepository.AbrirAyudaAltaProfesional();
        }

        public void AbrirAyudaModProfesional()
        {
            manualRepository.AbrirAyudaModProfesional();
        }

        public void AbrirAyudaDeshProfesional()
        {
            manualRepository.AbrirAyudaDeshProfesional();
        }

        public void AbrirAyudaVerProfesional()
        {
            manualRepository.AbrirAyudaVerProfesional();
        }



        public void AbrirAyudaAltaTratamiento()
        {
            manualRepository.AbrirAyudaAltaTratamiento();
        }

        public void AbrirAyudaAltaEtapa()
        {
            manualRepository.AbrirAyudaAltaEtapa();
        }






        public void AbrirAyudaAltaCita()
        {
            manualRepository.AbrirAyudaAltaCita();
        }

        public void AbrirAyudaRegistroCita()
        {
            manualRepository.AbrirAyudaRegistroCita();
        }

        public void AbrirAyudaListarCitas()
        {
            manualRepository.AbrirAyudaListarCitas();
        }



        public void AbrirAyudaHistorial()
        {
            manualRepository.AbrirAyudaHistorial();
        }
        public void AbrirAyudaHorarios()
        {
            manualRepository.AbrirAyudaHorarios();
        }
        public void AbrirAyudaMain()
        {
            manualRepository.AbrirAyudaMain();
        }







        public void AbrirAyudaLogin()
        {
            manualRepository.AbrirAyudaLogin();
        }

        public void AbrirAyudaCambiarPass()
        {
            manualRepository.AbrirAyudaCambiarPass();
        }

        public void AbrirAyudaRecuperoPass()
        {
            manualRepository.AbrirAyudaRecuperoPass();
        }

        public void AbrirAyudaAsignarRol()
        {
            manualRepository.AbrirAyudaAsignarRol();
        }

        public void AbrirAyudaBackUp()
        {
            manualRepository.AbrirAyudaBackUp();
        }

        public void AbrirAyudaBitacora()
        {
            manualRepository.AbrirAyudaBitacora();
        }

        public void AbrirAyudaCrearPatente()
        {
            manualRepository.AbrirAyudaCrearPatente();
        }

        public void AbrirAyudaCrearRol()
        {
            manualRepository.AbrirAyudaCrearRol();
        }

        public void AbrirAyudaCrearUsuario()
        {
            manualRepository.AbrirAyudaCrearUsuario();
        }

        public void AbrirAyudaModUsuario()
        {
            manualRepository.AbrirAyudaModUsuario();
        }

        public void AbrirAyudaMostrarUsuario()
        {
            manualRepository.AbrirAyudaMostrarUsuario();
        }

    }

}
