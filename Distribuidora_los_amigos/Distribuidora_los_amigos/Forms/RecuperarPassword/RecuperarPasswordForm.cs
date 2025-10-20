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
                
                LoggerService.WriteLog($"Formulario de recuperación de contraseña abierto.", System.Diagnostics.TraceLevel.Info);
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
            LoggerService.WriteLog($"Formulario de recuperación de contraseña cerrado.", System.Diagnostics.TraceLevel.Info);
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

                // Deshabilitar el botón mientras se procesa
                btnEnviarToken.Enabled = false;
                btnEnviarToken.Text = TranslateMessageKey("Enviando...");

                // Generar y enviar el token
                Service.Facade.RecuperoPassService.GenerarYEnviarMailRecuperacion(username);

                _currentUsername = username;

                // Mostrar mensaje de éxito
                string successMessage = TranslateMessageKey("Se ha enviado un código de recuperación a su correo electrónico. El código expira en 10 minutos.");
                lblMensaje.Text = successMessage;
                lblMensaje.ForeColor = System.Drawing.Color.Green;
                lblMensaje.Visible = true;

                // Mostrar panel para ingresar token y nueva contraseña
                panelToken.Visible = true;

                // Deshabilitar campos de usuario y botón
                textBoxUsuario.Enabled = false;

                LoggerService.WriteLog($"Token de recuperación enviado para usuario: {username}", System.Diagnostics.TraceLevel.Info);
            }
            catch (Exception ex)
            {
                string errorMessage = TranslateMessageKey("Error al enviar el código de recuperación: ") + ex.Message;
                lblMensaje.Text = errorMessage;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                lblMensaje.Visible = true;

                LoggerService.WriteLog($"Error al enviar token de recuperación: {ex.Message}", System.Diagnostics.TraceLevel.Error);
                LoggerService.WriteException(ex);
            }
            finally
            {
                btnEnviarToken.Enabled = true;
                btnEnviarToken.Text = TranslateMessageKey("Enviar código de recuperación");
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
                    string messageKey = "Por favor, ingrese el código de recuperación.";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    MessageBox.Show(translatedMessage, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(nuevaPassword) || string.IsNullOrEmpty(confirmarPassword))
                {
                    string messageKey = "Por favor, complete todos los campos de contraseña.";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    MessageBox.Show(translatedMessage, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (nuevaPassword != confirmarPassword)
                {
                    string messageKey = "Las contraseñas no coinciden.";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    MessageBox.Show(translatedMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (nuevaPassword.Length < 6)
                {
                    string messageKey = "La contraseña debe tener al menos 6 caracteres.";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    MessageBox.Show(translatedMessage, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Deshabilitar botón mientras se procesa
                btnCambiarPassword.Enabled = false;
                btnCambiarPassword.Text = TranslateMessageKey("Cambiando...");

                // Cambiar contraseña
                bool success = Service.Facade.RecuperoPassService.ChangePassword(_currentUsername, nuevaPassword, token, confirmarPassword);

                if (success)
                {
                    string successMessage = TranslateMessageKey("Contraseña cambiada exitosamente. Puede cerrar esta ventana e iniciar sesión con su nueva contraseña.");
                    MessageBox.Show(successMessage, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoggerService.WriteLog($"Contraseña cambiada exitosamente para usuario: {_currentUsername}", System.Diagnostics.TraceLevel.Info);

                    // Cerrar formulario
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = TranslateMessageKey("Error al cambiar la contraseña: ") + ex.Message;
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                LoggerService.WriteLog($"Error al cambiar contraseña: {ex.Message}", System.Diagnostics.TraceLevel.Error);
                LoggerService.WriteException(ex);
            }
            finally
            {
                btnCambiarPassword.Enabled = true;
                btnCambiarPassword.Text = TranslateMessageKey("Cambiar contraseña");
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}