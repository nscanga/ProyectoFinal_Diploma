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
using Service.Facade;
using Service.ManegerEx;
using Services.Facade;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    public partial class CrearUsuarioForm : Form
    {
        public CrearUsuarioForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosed += CrearUsuarioForm_FormClosed;
            this.KeyPreview = true;
            this.KeyDown += CrearUsuarioForm_KeyDown;
        }

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

        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }

        private void CrearUsuarioForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Registrar el cierre del formulario
            LoggerService.WriteLog($"Formulario '{this.Text}' cerrado.", System.Diagnostics.TraceLevel.Info);
        }

        private void CrearUsuarioForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {

                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaCrearUsuario();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Error");
                LoggerService.WriteException(ex);
            }

        }
    }
}
