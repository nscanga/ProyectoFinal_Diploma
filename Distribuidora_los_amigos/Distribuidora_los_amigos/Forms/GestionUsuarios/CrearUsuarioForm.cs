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
using Service.Facade;
using Service.ManegerEx;
using Services.Facade;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    public partial class CrearUsuarioForm : Form, IIdiomaObserver
    {
        /// <summary>
        /// Inicializa el formulario de creación de usuarios y configura eventos auxiliares.
        /// </summary>
        public CrearUsuarioForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosed += CrearUsuarioForm_FormClosed;
            this.KeyPreview = true;
            this.KeyDown += CrearUsuarioForm_KeyDown;
            
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
        /// Valida los campos obligatorios y registra un nuevo usuario mediante el servicio correspondiente.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonCreateUser_Click(object sender, EventArgs e)
        {
            string username = textBoxUserName.Text;
            string password = textBoxPassword.Text;
            string email = textBoxEmail.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {

                string messageKey = "Debe ingresar un nombre de usuario y una contraseña.";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage);

                return;
            }
            try
            {
                UserService.Register(username, password, email);
                string messageKey = "Usuario creado correctamente.";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoggerService.WriteLog($"El usuario {SesionService.UsuarioLogueado.UserName} creó al usuario {username}.", System.Diagnostics.TraceLevel.Info);

            }
            catch (SqlException ex)
            {
                string errorMessage = ErrorHandler.FormatSqlException(ex, username);

                ErrorHandler.HandleSqlException(ex, username);
                LoggerService.WriteException(ex);

                LoggerService.WriteLog($"Error al crear usuario {username} por {SesionService.UsuarioLogueado.UserName}.{errorMessage} ", System.Diagnostics.TraceLevel.Error);

            }
            catch (Exception ex)
            {

                string messageKey = "Error inesperado al crear usuario ";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage + " " + username + " " + ex.Message);
                ErrorHandler.HandleGeneralException(ex);
            }



            // Limpiar los TextBox
            textBoxUserName.Clear();
            textBoxPassword.Clear();
            textBoxEmail.Clear();
        }

        /// <summary>
        /// Traduce la clave de mensaje utilizando el servicio de idiomas.
        /// </summary>
        /// <param name="messageKey">Clave a traducir.</param>
        /// <returns>Mensaje traducido.</returns>
        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }

        /// <summary>
        /// Registra el cierre del formulario para auditoría.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento de cierre.</param>
        private void CrearUsuarioForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Desuscribirse del servicio de idiomas
            IdiomaService.Unsubscribe(this);
            
            // Registrar el cierre del formulario
            LoggerService.WriteLog($"Formulario '{this.Text}' cerrado.", System.Diagnostics.TraceLevel.Info);
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void CrearUsuarioForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaCrearUsuario();
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
