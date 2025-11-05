using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Service.DAL.Contracts;
using Service.DOMAIN;
using Service.Facade;
using Service.ManegerEx;
using Services.Facade;

namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    public partial class ModificarUsuarioForm : Form, IIdiomaObserver
    {
        /// <summary>
        /// Inicializa el formulario de administración de usuarios estableciendo eventos y título.
        /// </summary>
        public ModificarUsuarioForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Modificar Usuario";
            this.FormClosed += ModificarUsuarioForm_FormClosed;
            this.KeyPreview = true;
            this.KeyDown += ModificarUsuarioForm_KeyDown;
            
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
        /// Traduce el formulario y carga los usuarios disponibles al momento de abrir la vista.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void ModificarUsuarioForm_Load(object sender, EventArgs e)
        {
            LoggerService.WriteLog($"Formulario '{this.Text}' abierto.", System.Diagnostics.TraceLevel.Info);
            try
            {
                // Obtener la lista de usuarios y cargarla en el ComboBox
                List<Usuario> usuarios = UserService.GetAllUsuarios();
                cbUsuarios.DataSource = usuarios;
                cbUsuarios.DisplayMember = "UserName";
                cbUsuarios.ValueMember = "IdUsuario";
            }
            catch (SqlException ex)
            {
                string messageKey = "Error al cargar usuarios";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage + ex.Message);
                ErrorHandler.HandleSqlException(ex, translatedMessage);

            }
        }

        /// <summary>
        /// Registra el cierre del formulario de modificación de usuarios.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento de cierre.</param>
        private void ModificarUsuarioForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Desuscribirse del servicio de idiomas
            IdiomaService.Unsubscribe(this);
            
            // Registrar el cierre del formulario
            LoggerService.WriteLog($"Formulario '{this.Text}' cerrado.", System.Diagnostics.TraceLevel.Info);
        }

        /// <summary>
        /// Confirma con el usuario y habilita la cuenta seleccionada.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonEnable_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbUsuarios.SelectedValue != null)
                {
                    var selectedUserId = (Guid)cbUsuarios.SelectedValue;
                    var selectedUserName = ((Usuario)cbUsuarios.SelectedItem).UserName;

                    string messageKey = "¿Está seguro que desea habilitar este usuario?";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    string messageKey1 = "Confirmar habilitación";
                    string translatedMessage1 = TranslateMessageKey(messageKey1);

                    var result = MessageBox.Show(translatedMessage, translatedMessage1, MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        //UserService.EnabledUsuario((Guid)cbUsuarios.SelectedValue);

                        string successMessageKey = "Usuario habilitado correctamente.";
                        string translatedSuccessMessage = TranslateMessageKey(successMessageKey);
                        MessageBox.Show(translatedSuccessMessage);


                        LoggerService.WriteLog($"El Usuario {SesionService.UsuarioLogueado.UserName} Habilito al usuario {selectedUserName}  ", System.Diagnostics.TraceLevel.Info);



                    }

                }
                else
                {
                    string messageKey = "Por favor, seleccione un usuario.";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    MessageBox.Show(translatedMessage);
                }
            }
            catch (SqlException ex)
            {
                string messageKey = "Ocurrió un error al habilitar el usuario";
                string translatedMessage = TranslateMessageKey(messageKey);
                ErrorHandler.HandleSqlException(ex, translatedMessage);

                LoggerService.WriteLog($"Ocurrio un error al tratar de habilitar el usuario por {SesionService.UsuarioLogueado.UserName}   ", System.Diagnostics.TraceLevel.Error);



            }
        }

        /// <summary>
        /// Solicita confirmación y deshabilita el usuario elegido.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonDisable_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbUsuarios.SelectedValue != null)
                {
                    // Confirmar deshabilitación
                    string messageKey = "¿Está seguro que desea deshabilitar este usuario?";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    string messageKey1 = "Confirmar deshabilitación";
                    string translatedMessage1 = TranslateMessageKey(messageKey1);
                    var result = MessageBox.Show(translatedMessage, translatedMessage1, MessageBoxButtons.YesNo);
                    var selectedUserName = ((Usuario)cbUsuarios.SelectedItem).UserName;
                    if (result == DialogResult.Yes)
                    {
                        // Llamar al servicio para deshabilitar el usuario
                        UserService.DisableUser((Guid)cbUsuarios.SelectedValue);

                        string successMessageKey = "Usuario deshabilitado correctamente.";
                        string translatedSuccessMessage = TranslateMessageKey(successMessageKey);
                        MessageBox.Show(translatedSuccessMessage);


                        LoggerService.WriteLog($"El Usuario {SesionService.UsuarioLogueado.UserName} deshabilito al usuario {selectedUserName}  ", System.Diagnostics.TraceLevel.Info);


                    }
                }
                else
                {
                    string messageKey = "Por favor, seleccione un usuario.";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    MessageBox.Show(translatedMessage);
                }
            }
            catch (SqlException ex)
            {
                string messageKey = "Ocurrió un error al deshabilitar el usuario";
                string translatedMessage = TranslateMessageKey(messageKey);
                ErrorHandler.HandleSqlException(ex, translatedMessage);

                LoggerService.WriteLog($"Ocurrio un error al tratar de deshabilitar el usuario por {SesionService.UsuarioLogueado.UserName}   ", System.Diagnostics.TraceLevel.Error);

            }

        }
        /// <summary>
        /// Traduce una clave textual utilizando el servicio de idiomas.
        /// </summary>
        /// <param name="messageKey">Clave a traducir.</param>
        /// <returns>Mensaje traducido.</returns>
        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }

        /// <summary>
        /// Abre la ayuda del formulario cuando el usuario presiona F1.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void ModificarUsuarioForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {

                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaModUsuario();
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
