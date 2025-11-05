using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Service.DAL.Contracts;
using Service.DOMAIN;
using Service.Facade;
using Services.Facade;

namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    public partial class AsignarRolForm : Form, IIdiomaObserver
    {
        /// <summary>
        /// Inicializa el formulario de asignación de roles configurando eventos y posición.
        /// </summary>
        public AsignarRolForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosed += AsignarRolForm_FormClosed;
            this.KeyPreview = true;
            this.KeyDown += AsignarRolForm_KeyDown;
            
            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);
            
            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);
        }

        /// <summary>
        /// Actualiza los textos del formulario cuando cambia el idioma.
        /// </summary>
        public void UpdateIdioma()
        {
            IdiomaService.TranslateForm(this);
            this.Refresh();
        }

        /// <summary>
        /// Traduce el formulario y carga usuarios y familias para su selección.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void AsignarRolForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Usamos UserService para obtener la lista de usuarios
                List<Usuario> usuarios = UserService.GetAllUsuarios(); // Método en UserService
                comboBoxUsers.DataSource = usuarios;
                comboBoxUsers.DisplayMember = "UserName";
                comboBoxUsers.ValueMember = "IdUsuario";

                // Usamos FamiliaService para obtener la lista de familias
                List<Familia> familias = FamiliaService.GetAllFamilias(); // Método en FamiliaService
                comboBoxRoles.DataSource = familias;
                comboBoxRoles.DisplayMember = "Nombre";  // Mostrar el nombre de la familia
                comboBoxRoles.ValueMember = "Id";


                LoggerService.WriteLog($"Formulario '{this.Text}' abierto.", System.Diagnostics.TraceLevel.Info);
            }
            catch (Exception ex)
            {
                // Manejar excepciones en la carga del formulario
                LoggerService.WriteLog($"Error al cargar el formulario AsignarRolForm: {ex.Message}", System.Diagnostics.TraceLevel.Error);



                string messageKey = "Ocurrió un error al cargar el formulario:";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Registra el cierre del formulario de asignación de roles.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento de cierre.</param>
        private void AsignarRolForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Desuscribirse del servicio de idiomas
            IdiomaService.Unsubscribe(this);
            
            // Registrar el cierre del formulario
            LoggerService.WriteLog($"Formulario '{this.Text}' cerrado.", System.Diagnostics.TraceLevel.Info);
        }

        /// <summary>
        /// Valida las selecciones y asigna la familia elegida al usuario seleccionado.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonAssignRole_Click(object sender, EventArgs e)
        {
            Usuario usuarioSeleccionado = (Usuario)comboBoxUsers.SelectedItem;
            Familia familiaSeleccionada = (Familia)comboBoxRoles.SelectedItem;
            if (comboBoxUsers.SelectedValue == null || comboBoxRoles.SelectedValue == null)
            {
                string messageKey = "Por favor, seleccione un usuario y una familia.";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage);


                return;
            }
            try
            {
                // Validar que se ha seleccionado un usuario y una familia


                // Obtener el IdUsuario seleccionado y la familia seleccionada
                Guid usuarioId = (Guid)comboBoxUsers.SelectedValue;
                Guid familiaId = (Guid)comboBoxRoles.SelectedValue;

                // Asignar la familia al usuario usando el servicio
                FamiliaService.AsignarFamiliaAUsuario(usuarioId, new Familia { Id = familiaId });



                // Registro en el log
                LoggerService.WriteLog($"Rol {familiaSeleccionada.Nombre} asignado al usuario {usuarioSeleccionado.UserName} exitosamente. Usuario: {SesionService.UsuarioLogueado.UserName}", System.Diagnostics.TraceLevel.Info);

                string messageKey = "Rol asignado con éxito.";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage);

            }
            catch (Exception ex)
            {
                // Manejar excepciones al asignar el rol
                LoggerService.WriteLog($"Error al asignar el rol: {familiaSeleccionada.Nombre} {ex.Message}", System.Diagnostics.TraceLevel.Error);
                string messageKey = "Ocurrió un error al asignar el rol:";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage, $"{familiaSeleccionada.Nombre}" + ex.Message);

            }

        }

        /// <summary>
        /// Traduce claves de mensaje usando el servicio de idiomas.
        /// </summary>
        /// <param name="messageKey">Clave a traducir.</param>
        /// <returns>Mensaje traducido.</returns>
        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void AsignarRolForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaAsignarRol();
                    e.Handled = true; // Prevenir propagación del evento
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ayuda: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }
    }
}
