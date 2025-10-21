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
    /// <summary>
    /// Servicio que expone métodos para abrir secciones del manual contextual según el idioma.
    /// </summary>
    public class ManualService
    {
        private ManualRepository manualRepository;

        /// <summary>
        /// Inicializa el servicio cargando el repositorio correspondiente al idioma del usuario.
        /// </summary>
        public ManualService()
        {
            // Obtener el código de idioma de la configuración o de otro lugar
            string languageCode = IdiomaLogic.LoadUserLanguage();
            manualRepository = new ManualRepository(languageCode);
        }
        /// <summary>
        /// Abre la sección de ayuda para el alta de pacientes.
        /// </summary>
        public void AbrirAyudaAltaPaciente()
        {
            manualRepository.AbrirAyudaAltaPaciente();
        }

        /// <summary>
        /// Muestra la ayuda para modificar pacientes.
        /// </summary>
        public void AbrirAyudaModPaciente()
        {
            manualRepository.AbrirAyudaModPaciente();
        }

        /// <summary>
        /// Abre la ayuda para deshabilitar pacientes.
        /// </summary>
        public void AbrirAyudaDeshPaciente()
        {
            manualRepository.AbrirAyudaDeshPaciente();
        }
        /// <summary>
        /// Muestra la ayuda sobre visualización de pacientes.
        /// </summary>
        public void AbrirAyudaVerPaciente()
        {
            manualRepository.AbrirAyudaVerPaciente();
        }


        /// <summary>
        /// Abre la guía del alta de profesionales.
        /// </summary>
        public void AbrirAyudaAltaProfesional()
        {
            manualRepository.AbrirAyudaAltaProfesional();
        }

        /// <summary>
        /// Presenta la ayuda para modificar profesionales.
        /// </summary>
        public void AbrirAyudaModProfesional()
        {
            manualRepository.AbrirAyudaModProfesional();
        }

        /// <summary>
        /// Muestra la ayuda para deshabilitar profesionales.
        /// </summary>
        public void AbrirAyudaDeshProfesional()
        {
            manualRepository.AbrirAyudaDeshProfesional();
        }

        /// <summary>
        /// Abre la sección de ayuda para ver profesionales.
        /// </summary>
        public void AbrirAyudaVerProfesional()
        {
            manualRepository.AbrirAyudaVerProfesional();
        }



        /// <summary>
        /// Abre la ayuda para crear tratamientos.
        /// </summary>
        public void AbrirAyudaAltaTratamiento()
        {
            manualRepository.AbrirAyudaAltaTratamiento();
        }

        /// <summary>
        /// Muestra la guía para crear etapas de tratamiento.
        /// </summary>
        public void AbrirAyudaAltaEtapa()
        {
            manualRepository.AbrirAyudaAltaEtapa();
        }






        /// <summary>
        /// Abre la ayuda del alta de citas.
        /// </summary>
        public void AbrirAyudaAltaCita()
        {
            manualRepository.AbrirAyudaAltaCita();
        }

        /// <summary>
        /// Muestra la ayuda del registro de citas.
        /// </summary>
        public void AbrirAyudaRegistroCita()
        {
            manualRepository.AbrirAyudaRegistroCita();
        }

        /// <summary>
        /// Despliega la ayuda para listar citas.
        /// </summary>
        public void AbrirAyudaListarCitas()
        {
            manualRepository.AbrirAyudaListarCitas();
        }



        /// <summary>
        /// Abre la ayuda del historial de atenciones.
        /// </summary>
        public void AbrirAyudaHistorial()
        {
            manualRepository.AbrirAyudaHistorial();
        }
        /// <summary>
        /// Muestra la ayuda para gestionar horarios.
        /// </summary>
        public void AbrirAyudaHorarios()
        {
            manualRepository.AbrirAyudaHorarios();
        }
        /// <summary>
        /// Despliega la ayuda de la pantalla principal.
        /// </summary>
        public void AbrirAyudaMain()
        {
            manualRepository.AbrirAyudaMain();
        }







        /// <summary>
        /// Abre la ayuda para el inicio de sesión.
        /// </summary>
        public void AbrirAyudaLogin()
        {
            manualRepository.AbrirAyudaLogin();
        }

        /// <summary>
        /// Muestra la ayuda para cambiar contraseñas.
        /// </summary>
        public void AbrirAyudaCambiarPass()
        {
            manualRepository.AbrirAyudaCambiarPass();
        }

        /// <summary>
        /// Despliega la guía de recuperación de contraseña.
        /// </summary>
        public void AbrirAyudaRecuperoPass()
        {
            manualRepository.AbrirAyudaRecuperoPass();
        }

        /// <summary>
        /// Abre la ayuda para asignar roles.
        /// </summary>
        public void AbrirAyudaAsignarRol()
        {
            manualRepository.AbrirAyudaAsignarRol();
        }

        /// <summary>
        /// Muestra la ayuda sobre generación de respaldos.
        /// </summary>
        public void AbrirAyudaBackUp()
        {
            manualRepository.AbrirAyudaBackUp();
        }

        /// <summary>
        /// Presenta la ayuda para consultar bitácoras.
        /// </summary>
        public void AbrirAyudaBitacora()
        {
            manualRepository.AbrirAyudaBitacora();
        }

        /// <summary>
        /// Abre la ayuda para crear patentes.
        /// </summary>
        public void AbrirAyudaCrearPatente()
        {
            manualRepository.AbrirAyudaCrearPatente();
        }

        /// <summary>
        /// Muestra la ayuda para crear roles.
        /// </summary>
        public void AbrirAyudaCrearRol()
        {
            manualRepository.AbrirAyudaCrearRol();
        }

        /// <summary>
        /// Despliega la ayuda para crear usuarios.
        /// </summary>
        public void AbrirAyudaCrearUsuario()
        {
            manualRepository.AbrirAyudaCrearUsuario();
        }

        /// <summary>
        /// Presenta la ayuda para modificar usuarios.
        /// </summary>
        public void AbrirAyudaModUsuario()
        {
            manualRepository.AbrirAyudaModUsuario();
        }

        /// <summary>
        /// Abre la ayuda para visualizar usuarios registrados.
        /// </summary>
        public void AbrirAyudaMostrarUsuario()
        {
            manualRepository.AbrirAyudaMostrarUsuario();
        }

    }

}
