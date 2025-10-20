using System;
using System.Windows.Forms;
using Service.Facade;
using Service.DAL.Contracts;
using Services.Facade;

namespace Distribuidora_los_amigos.Forms.RecuperarPassword
{
    public partial class RecuperarPasswordForm : Form, IIdiomaObserver
    {
        private string _currentUsername = "";

        public RecuperarPasswordForm()
        {
            InitializeComponent();
        }

        private void RecuperarPasswordForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Suscribirse a los cambios de idioma
                IdiomaService.Subscribe(this);
                
                // Traducir los controles del formulario
                IdiomaService.TranslateForm(this);
                
                LoggerService.WriteLog($"Formulario de recuperaci�n de contrase�a abierto.", System.Diagnostics.TraceLevel.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el formulario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        public void UpdateIdioma()
        {
            // Actualizar la interfaz cuando cambie el idioma
            IdiomaService.TranslateForm(this);
            this.Refresh();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            IdiomaService.Unsubscribe(this);
            LoggerService.WriteLog($"Formulario de recuperaci�n de contrase�a cerrado.", System.Diagnostics.TraceLevel.Info);
            base.OnFormClosed(e);
        }

        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }

        private void btnEnviarToken_Click(object sender, EventArgs e)
        {
            try
            {
                string username = textBoxUsuario.Text.Trim();

                if (string.IsNullOrEmpty(username))
                {
                    string messageKey = "Por favor, ingrese su nombre de usuario.";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    MessageBox.Show(translatedMessage, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Deshabilitar el bot�n mientras se procesa
                btnEnviarToken.Enabled = false;
                btnEnviarToken.Text = TranslateMessageKey("Enviando...");

                // Generar y enviar el token
                Service.Facade.RecuperoPassService.GenerarYEnviarMailRecuperacion(username);

                _currentUsername = username;

                // Mostrar mensaje de �xito
                string successMessage = TranslateMessageKey("Se ha enviado un c�digo de recuperaci�n a su correo electr�nico. El c�digo expira en 10 minutos.");
                lblMensaje.Text = successMessage;
                lblMensaje.ForeColor = System.Drawing.Color.Green;
                lblMensaje.Visible = true;

                // Mostrar panel para ingresar token y nueva contrase�a
                panelToken.Visible = true;

                // Deshabilitar campos de usuario y bot�n
                textBoxUsuario.Enabled = false;

                LoggerService.WriteLog($"Token de recuperaci�n enviado para usuario: {username}", System.Diagnostics.TraceLevel.Info);
            }
            catch (Exception ex)
            {
                string errorMessage = TranslateMessageKey("Error al enviar el c�digo de recuperaci�n: ") + ex.Message;
                lblMensaje.Text = errorMessage;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                lblMensaje.Visible = true;

                LoggerService.WriteLog($"Error al enviar token de recuperaci�n: {ex.Message}", System.Diagnostics.TraceLevel.Error);
                LoggerService.WriteException(ex);
            }
            finally
            {
                btnEnviarToken.Enabled = true;
                btnEnviarToken.Text = TranslateMessageKey("Enviar c�digo de recuperaci�n");
            }
        }

        private void btnCambiarPassword_Click(object sender, EventArgs e)
        {
            try
            {
                string token = textBoxToken.Text.Trim();
                string nuevaPassword = textBoxNuevaPassword.Text;
                string confirmarPassword = textBoxConfirmarPassword.Text;

                if (string.IsNullOrEmpty(token))
                {
                    string messageKey = "Por favor, ingrese el c�digo de recuperaci�n.";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    MessageBox.Show(translatedMessage, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(nuevaPassword) || string.IsNullOrEmpty(confirmarPassword))
                {
                    string messageKey = "Por favor, complete todos los campos de contrase�a.";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    MessageBox.Show(translatedMessage, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (nuevaPassword != confirmarPassword)
                {
                    string messageKey = "Las contrase�as no coinciden.";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    MessageBox.Show(translatedMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (nuevaPassword.Length < 6)
                {
                    string messageKey = "La contrase�a debe tener al menos 6 caracteres.";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    MessageBox.Show(translatedMessage, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Deshabilitar bot�n mientras se procesa
                btnCambiarPassword.Enabled = false;
                btnCambiarPassword.Text = TranslateMessageKey("Cambiando...");

                // Cambiar contrase�a
                bool success = Service.Facade.RecuperoPassService.ChangePassword(_currentUsername, nuevaPassword, token, confirmarPassword);

                if (success)
                {
                    string successMessage = TranslateMessageKey("Contrase�a cambiada exitosamente. Puede cerrar esta ventana e iniciar sesi�n con su nueva contrase�a.");
                    MessageBox.Show(successMessage, "�xito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoggerService.WriteLog($"Contrase�a cambiada exitosamente para usuario: {_currentUsername}", System.Diagnostics.TraceLevel.Info);

                    // Cerrar formulario
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = TranslateMessageKey("Error al cambiar la contrase�a: ") + ex.Message;
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                LoggerService.WriteLog($"Error al cambiar contrase�a: {ex.Message}", System.Diagnostics.TraceLevel.Error);
                LoggerService.WriteException(ex);
            }
            finally
            {
                btnCambiarPassword.Enabled = true;
                btnCambiarPassword.Text = TranslateMessageKey("Cambiar contrase�a");
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}