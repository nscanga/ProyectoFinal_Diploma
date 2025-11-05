using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Service.Facade;
using Services.Facade;
using Service.DAL.Contracts;

namespace Distribuidora_los_amigos.Forms.RecuperarPassword
{
    public partial class RecuperarPasswordForm : Form, IIdiomaObserver
    {
        private string _currentUsername = "";

        /// <summary>
        /// Inicializa el formulario de recuperación de contraseña.
        /// </summary>
        public RecuperarPasswordForm()
        {
            InitializeComponent();

            // Configurar ayuda F1 - DEBE IR DESPUÉS DE InitializeComponent
            this.KeyPreview = true;
            this.KeyDown += RecuperarPasswordForm_KeyDown;

            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);

            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);
        }

        /// <summary>
        /// Procesa las teclas de comando del formulario, capturando F1 para mostrar ayuda.
        /// Este método es más efectivo que KeyDown para formularios modales.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F1)
            {
                try
                {
                    ManualService manualService = new ManualService();
                    // Pasar 'this' como owner - crucial para formularios modales
                    manualService.AbrirAyudaRecuperoPass(this);
                    return true; // Indica que la tecla fue procesada
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al abrir la ayuda: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoggerService.WriteException(ex);
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        private void RecuperarPasswordForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    // Pasar 'this' como owner - crucial para formularios modales
                    manualService.AbrirAyudaRecuperoPass(this);
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ayuda: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Suscribe el formulario al servicio de idioma y traduce los controles al cargar.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void RecuperarPasswordForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Suscribirse a los cambios de idioma
                IdiomaService.Subscribe(this);
                
                // Traducir los controles del formulario
                IdiomaService.TranslateForm(this);
            }
            catch (Exception ex)
            {
                string errorKey = "ErrorCargarFormulario";
                string translatedError = IdiomaService.Translate(errorKey) + ": " + ex.Message;
                string errorTitleKey = "Error";
                string translatedErrorTitle = IdiomaService.Translate(errorTitleKey);
                MessageBox.Show(translatedError, translatedErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Reaplica la traducción del formulario cuando cambia el idioma activo.
        /// </summary>
        public void UpdateIdioma()
        {
            // Actualizar la interfaz cuando cambie el idioma
            IdiomaService.TranslateForm(this);
            this.Refresh();
        }

        /// <summary>
        /// Cancela la suscripción al servicio de idioma al cerrar el formulario.
        /// </summary>
        /// <param name="e">Argumentos del evento de cierre.</param>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            IdiomaService.Unsubscribe(this);
            base.OnFormClosed(e);
        }

        /// <summary>
        /// Solicita y envía el token de recuperación, habilitando el panel de cambio de contraseña.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void btnEnviarToken_Click(object sender, EventArgs e)
        {
            try
            {
                string username = textBoxUsuario.Text.Trim();

                if (string.IsNullOrEmpty(username))
                {
                    string messageKey = "IngreseNombreUsuario";
                    string translatedMessage = IdiomaService.Translate(messageKey);
                    string titleKey = "Advertencia";
                    string translatedTitle = IdiomaService.Translate(titleKey);
                    MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Deshabilitar el botón mientras se procesa
                btnEnviarToken.Enabled = false;
                btnEnviarToken.Text = IdiomaService.Translate("Enviando");

                // Generar y enviar el token
                Service.Facade.RecuperoPassService.GenerarYEnviarMailRecuperacion(username);

                _currentUsername = username;

                // Mostrar mensaje de éxito
                string successMessage = IdiomaService.Translate("TokenEnviadoExito");
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
                string errorMessage = IdiomaService.Translate("ErrorEnviarToken") + ": " + ex.Message;
                lblMensaje.Text = errorMessage;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                lblMensaje.Visible = true;

                LoggerService.WriteLog($"Error al enviar token de recuperación: {ex.Message}", System.Diagnostics.TraceLevel.Error);
                LoggerService.WriteException(ex);
            }
            finally
            {
                btnEnviarToken.Enabled = true;
                btnEnviarToken.Text = IdiomaService.Translate("Enviar_código_de_recuperación");
            }
        }

        /// <summary>
        /// Valida la información ingresada y solicita el cambio de contraseña con el token recibido.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void btnCambiarPassword_Click(object sender, EventArgs e)
        {
            try
            {
                string token = textBoxToken.Text.Trim();
                string nuevaPassword = textBoxNuevaPassword.Text;
                string confirmarPassword = textBoxConfirmarPassword.Text;

                if (string.IsNullOrEmpty(token))
                {
                    string messageKey = "IngreseCodigoRecuperacion";
                    string translatedMessage = IdiomaService.Translate(messageKey);
                    string titleKey = "Advertencia";
                    string translatedTitle = IdiomaService.Translate(titleKey);
                    MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(nuevaPassword) || string.IsNullOrEmpty(confirmarPassword))
                {
                    string messageKey = "CompleteCamposContraseña";
                    string translatedMessage = IdiomaService.Translate(messageKey);
                    string titleKey = "Advertencia";
                    string translatedTitle = IdiomaService.Translate(titleKey);
                    MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (nuevaPassword != confirmarPassword)
                {
                    string messageKey = "ContraseñasNoCoinciden";
                    string translatedMessage = IdiomaService.Translate(messageKey);
                    string titleKey = "Error";
                    string translatedTitle = IdiomaService.Translate(titleKey);
                    MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (nuevaPassword.Length < 6)
                {
                    string messageKey = "ContraseñaMinimo6Caracteres";
                    string translatedMessage = IdiomaService.Translate(messageKey);
                    string titleKey = "Advertencia";
                    string translatedTitle = IdiomaService.Translate(titleKey);
                    MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Deshabilitar botón mientras se procesa
                btnCambiarPassword.Enabled = false;
                btnCambiarPassword.Text = IdiomaService.Translate("Cambiando");

                // Cambiar contraseña
                bool success = Service.Facade.RecuperoPassService.ChangePassword(_currentUsername, nuevaPassword, token, confirmarPassword);

                if (success)
                {
                    string successMessage = IdiomaService.Translate("ContraseñaCambiadaExito");
                    string successTitleKey = "Éxito";
                    string translatedSuccessTitle = IdiomaService.Translate(successTitleKey);
                    MessageBox.Show(successMessage, translatedSuccessTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Cerrar formulario
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = IdiomaService.Translate("ErrorCambiarContraseña") + ": " + ex.Message;
                string errorTitleKey = "Error";
                string translatedErrorTitle = IdiomaService.Translate(errorTitleKey);
                MessageBox.Show(errorMessage, translatedErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                LoggerService.WriteLog($"Error al cambiar contraseña: {ex.Message}", System.Diagnostics.TraceLevel.Error);
                LoggerService.WriteException(ex);
            }
            finally
            {
                btnCambiarPassword.Enabled = true;
                btnCambiarPassword.Text = IdiomaService.Translate("Cambiar_contraseña");
            }
        }

        /// <summary>
        /// Cierra el formulario cancelando el proceso de recuperación.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}