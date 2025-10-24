using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Service.DAL.Contracts;
using Service.DAL.Implementations.SqlServer.Helpers;
using Service.DOMAIN;
using Service.Facade;
using Services.Facade;

namespace Distribuidora_los_amigos
{

    public partial class LoginForm : Form , IIdiomaObserver // Implemento la interface del observer
    {
        /// <summary>
        /// Inicializa el formulario de inicio de sesión configurando eventos y posición inicial.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();
            this.FormClosed += LoginForm_FormClosed;
            this.KeyPreview = true;
            this.KeyDown += LoginForm_KeyDown;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// Configura la suscripción a idiomas, crea un usuario administrador por defecto y traduce la interfaz al cargar.
        /// </summary>
        /// <param name="sender">Origen del evento de carga.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
                InicializadorDeIdioma();
                // Suscribirse a los cambios de idioma
                IdiomaService.Subscribe(this);

                // Cargar el idioma guardado y aplicarlo
                string currentLanguage = IdiomaService.LoadUserLanguage();

                // ✅ Verificar si existen usuarios en el sistema
                List<Usuario> usuarios = UserService.GetAllUsuarios();

                if (usuarios.Count == 0)
                {
                    string messageKey = "No hay usuarios en el sistema. Se creará un usuario administrador por defecto.\n\nUsuario: admin\nContraseña: Admin123!";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    MessageBox.Show(translatedMessage, "Inicialización del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    try
                    {
                        // ✅ Crear usuario administrador usando el servicio (esto aplicará el hash MD5 correctamente)
                        var usuarioAdmin = new Usuario
                        {
                            UserName = "admin"
                        };

                        UserService.Register("admin", "Admin123!", "admin@sistema.com");

                        LoggerService.WriteLog("Usuario administrador por defecto creado exitosamente.", System.Diagnostics.TraceLevel.Info);
                        
                        string successKey = "Usuario administrador creado exitosamente.\n\nUsuario: admin\nContraseña: Admin123!\n\n⚠️ Por favor, cambie esta contraseña después del primer inicio de sesión.";
                        string successMessage = TranslateMessageKey(successKey);
                        MessageBox.Show(successMessage, "Sistema Inicializado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        LoggerService.WriteLog($"Error al crear usuario administrador por defecto: {ex.Message}", System.Diagnostics.TraceLevel.Error);
                        LoggerService.WriteException(ex);
                        
                        string errorKey = "Error crítico: No se pudo crear el usuario administrador por defecto. La aplicación no puede continuar.";
                        string errorMessage = TranslateMessageKey(errorKey);
                        MessageBox.Show(errorMessage + "\n\n" + ex.Message, "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                        Application.Exit();
                        return;
                    }
                }

                // Establecer la cultura del hilo principal al idioma guardado
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLanguage);

                // Traducir los controles del formulario
                IdiomaService.TranslateForm(this);

                LoggerService.WriteLog($"Formulario '{this.Text}' abierto.", System.Diagnostics.TraceLevel.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Error");
                LoggerService.WriteException(ex);
            }
        }
        /// <summary>
        /// Libera la suscripción al servicio de idioma y registra el cierre del formulario.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento de cierre.</param>
        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            IdiomaService.Unsubscribe(this);
            // Registrar el cierre del formulario
            LoggerService.WriteLog($"Formulario '{this.Text}' cerrado.", System.Diagnostics.TraceLevel.Info);
        }

        /// <summary>
        /// Actualiza los textos del formulario cuando se detecta un cambio de idioma.
        /// </summary>
        public void UpdateIdioma()
        {
            // Actualizar la interfaz cuando cambie el idioma
            IdiomaService.TranslateForm(this);
            this.Refresh();
        }


        /// <summary>
        /// Maneja el cambio manual de idioma guardando la selección y retraduciendo la interfaz.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {


            Dictionary<string, string> languageMap = new Dictionary<string, string>()
             {
              { "Español", "es-ES" },
              { "Inglés", "en-US" },
             { "Portugués", "pt-PT" }
             };

            string selectedItem = listBox1.SelectedItem.ToString();
            string selectedLanguage = languageMap.ContainsKey(selectedItem) ? languageMap[selectedItem] : "en-US"; // Predeterminado a Inglés si no se encuentra

            // Guardar el nuevo idioma seleccionado
            IdiomaService.SaveUserLanguage(selectedLanguage);

            // Cambiar la cultura del hilo principal al nuevo idioma
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);

            // Traducir nuevamente los controles
            IdiomaService.TranslateForm(this);
            this.Refresh();
        }

        /// <summary>
        /// Inicializa la lista de idiomas disponibles y selecciona el idioma persistido.
        /// </summary>
        private void InicializadorDeIdioma()
        {
            listBox1.Items.AddRange(new string[] { "Español", "Inglés", "Portugués" });

            // Seleccionar el idioma actual
            string currentLanguage = IdiomaService.LoadUserLanguage();
            Dictionary<string, string> reverseLanguageMap = new Dictionary<string, string>()
            {
            { "es-ES", "Español" },
            { "en-US", "Inglés" },
            { "pt-PT", "Portugués" }
             };

            // Asignar el idioma seleccionado en la ListBox
            listBox1.SelectedItem = reverseLanguageMap.ContainsKey(currentLanguage) ? reverseLanguageMap[currentLanguage] : "Inglés"; // Predeterminado a Inglés
        }

        /// <summary>
        /// Traduce una clave textual utilizando el servicio de idiomas.
        /// </summary>
        /// <param name="messageKey">Clave a traducir.</param>
        /// <returns>Cadena traducida correspondiente.</returns>
        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }
        /// <summary>
        /// Abre la ayuda contextual del formulario cuando el usuario presiona F1.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento de teclado.</param>
        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {

                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaLogin();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Error");
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Placeholder del botón OK (sin implementación adicional).
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void okButton_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Limpia los campos de usuario y contraseña.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void lblClear_Click(object sender, EventArgs e)
        {
            textBoxUser.Clear();
            textBoxPass.Clear();
        }

        /// <summary>
        /// Cierra el formulario de login al hacer clic en el ícono de cierre.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Alterna la visibilidad de la contraseña según el estado del checkbox.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void checkBoxPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPass.Checked)
            {
                // Mostrar la contraseña como texto plano
                textBoxPass.UseSystemPasswordChar = false;
            }
            else
            {
                // Ocultar la contraseña con caracteres de seguridad
                textBoxPass.UseSystemPasswordChar = true;
            }
        }

        /// <summary>
        /// Valida credenciales, inicia sesión y abre el formulario principal si la autenticación es exitosa.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = textBoxUser.Text.Trim();
                string password = textBoxPass.Text;

                // Validar que el nombre de usuario y la contraseña no estén vacíos
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    string messageKey = "Por favor, ingrese su nombre de usuario y contraseña";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    MessageBox.Show(translatedMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                // Intentar iniciar sesión
                if (UserService.Login(username, password))
                {
                    Usuario user = UserService.GetUsuarioByUsername(username);
                    SesionService.UsuarioLogueado = user;

                    // Ocultar el formulario de login
                    this.Hide();

                    // Crear y mostrar el formulario principal
                    main mainForm = new main();
                    mainForm.Show();

                    // Suscribirse al evento FormClosed para cerrar el formulario de login después
                    mainForm.FormClosed += (s, args) => this.Close();
                    LoggerService.WriteLog($"Inicio de sesión exitoso para el usuario: {username}.", System.Diagnostics.TraceLevel.Info);


                }
                else
                {
                    string messageKey = "Inicio de sesión fallido. Verifique su nombre de usuario y contraseña.";
                    string translatedMessage = TranslateMessageKey(messageKey);
                    MessageBox.Show(translatedMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoggerService.WriteLog($"Inicio de sesión fallido para el usuario: {username}.", System.Diagnostics.TraceLevel.Warning);

                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales

                //LoggerService.WriteLog($"Error al intentar iniciar sesión: {ex.Message}", System.Diagnostics.TraceLevel.Error);
                //string messageKey = "Ocurrió un error al intentar iniciar sesión:";
                //string translatedMessage = TranslateMessageKey(messageKey);
                //MessageBox.Show(translatedMessage + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //LoggerService.WriteException(ex);
                Console.WriteLine("Error al iniciar sesión: " + ex.ToString()); // Imprime el error exacto en la consola
                LoggerService.WriteLog($"Error al intentar iniciar sesión: {ex.Message}", System.Diagnostics.TraceLevel.Error);

                string messageKey = "Ocurrió un error al intentar iniciar sesión:";
                string translatedMessage = TranslateMessageKey(messageKey);

                MessageBox.Show(translatedMessage + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Inicia el flujo de recuperación de contraseña mostrando el formulario correspondiente.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void label5_Click(object sender, EventArgs e)
        {
            try
            {
                // Abrir formulario de recuperación de contraseña
                var recuperarForm = new Forms.RecuperarPassword.RecuperarPasswordForm();
                recuperarForm.ShowDialog(this);
                
                LoggerService.WriteLog("Se abrió el formulario de recuperación de contraseña.", System.Diagnostics.TraceLevel.Info);
            }
            catch (Exception ex)
            {
                string messageKey = "Error al abrir el formulario de recuperación de contraseña:";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }
    }
}
