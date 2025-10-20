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
        public LoginForm()
        {
            InitializeComponent();
            this.FormClosed += LoginForm_FormClosed;
            this.KeyPreview = true;
            this.KeyDown += LoginForm_KeyDown;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
                //string hash = CryptographyService.HashMd5("admin123");
                //MessageBox.Show("Hash generado: " + hash);
                InicializadorDeIdioma();
                // Suscribirse a los cambios de idioma
                IdiomaService.Subscribe(this);

                // Cargar el idioma guardado y aplicarlo
                string currentLanguage = IdiomaService.LoadUserLanguage();


                IUsuarioRepository usuarioRepo = new UsuarioRepository();

                if (usuarioRepo.GetAll().Count == 0)
                {
                    MessageBox.Show("No hay usuarios creados. Se creará un usuario administrador por defecto.", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var usuarioAdmin = new Usuario
                    {
                        IdUsuario = Guid.NewGuid(),
                        UserName = "admin",
                        Password = "admin123",
                        Email = "admin@admin.com",
                        Estado = "Habilitado",
                        Lenguaje = "es-ES"
                    };

                    usuarioRepo.CreateUsuario(usuarioAdmin);
                }


                // Establecer la cultura del hilo principal al idioma guardado
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLanguage);

                // Traducir los controles del formulario
                IdiomaService.TranslateForm(this);

                // Inicializar el ComboBox con las opciones de idioma

                LoggerService.WriteLog($"Formulario '{this.Text}' abierto.", System.Diagnostics.TraceLevel.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Error");
                LoggerService.WriteException(ex);
            }
        }
        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            IdiomaService.Unsubscribe(this);
            // Registrar el cierre del formulario
            LoggerService.WriteLog($"Formulario '{this.Text}' cerrado.", System.Diagnostics.TraceLevel.Info);
        }

        public void UpdateIdioma()
        {
            // Actualizar la interfaz cuando cambie el idioma
            IdiomaService.TranslateForm(this);
            this.Refresh();
        }


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

        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }
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

        private void okButton_Click(object sender, EventArgs e)
        {

        }

        private void lblClear_Click(object sender, EventArgs e)
        {
            textBoxUser.Clear();    
            textBoxPass.Clear();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }

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
