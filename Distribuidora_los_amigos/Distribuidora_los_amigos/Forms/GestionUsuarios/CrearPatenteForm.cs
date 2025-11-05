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
    public partial class CrearPatenteForm : Form, IIdiomaObserver
    {
        /// <summary>
        /// Inicializa el formulario de creación de patentes configurando eventos básicos.
        /// </summary>
        public CrearPatenteForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosed += CrearPatenteForm_FormClosed;
            this.KeyPreview = true;
            this.KeyDown += CrearPatenteForm_KeyDown;
            
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
        /// Traduce la clave recibida utilizando el servicio de idiomas.
        /// </summary>
        /// <param name="messageKey">Clave a traducir.</param>
        /// <returns>Mensaje traducido.</returns>
        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }

        /// <summary>
        /// Habilita la ayuda contextual del formulario al presionar F1.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void CrearPatenteForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {

                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaCrearPatente();
                    e.Handled = true; // Prevenir propagación del evento
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Error");
                LoggerService.WriteException(ex);
            }

        }

        /// <summary>
        /// Registra el cierre del formulario para auditoría.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento de cierre.</param>
        private void CrearPatenteForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Desuscribirse del servicio de idiomas
            IdiomaService.Unsubscribe(this);
            
            // Registrar cuando el formulario se cierra
            LoggerService.WriteLog($"Formulario '{this.Text}' cerrado.", System.Diagnostics.TraceLevel.Info);
        }

        /// <summary>
        /// Traduce la interfaz y llena el combo con los tipos de acceso disponibles.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void CrearPatenteForm_Load(object sender, EventArgs e)
        {
            try
            {
                IdiomaService.TranslateForm(this);
                comboBoxTipoAcceso.DataSource = Enum.GetValues(typeof(TipoAcceso));
                LoggerService.WriteLog($"Formulario '{this.Text}' abierto.", System.Diagnostics.TraceLevel.Info);
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al cargar el formulario '{this.Text}': {ex.Message}", System.Diagnostics.TraceLevel.Error);

                string messageKey = "Ocurrió un error al cargar el formulario:";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Valida los datos ingresados y crea una nueva patente mediante el servicio de usuarios.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Validar los campos
            string nombre = textBoxNombre.Text.Trim();
            TipoAcceso tipoAcceso = (TipoAcceso)comboBoxTipoAcceso.SelectedItem;

            if (string.IsNullOrEmpty(nombre))
            {
                string messageKey = "Por favor, ingrese un nombre.";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage);
                return;
            }
            try
            {
                // Crear la nueva patente
                Patente nuevaPatente = new Patente
                {
                    Id = Guid.NewGuid(),
                    Nombre = nombre,
                    DataKey = nombre, // Usar el nombre como DataKey
                    TipoAcceso = tipoAcceso
                };

                // Llamar al servicio para guardar la patente
                UserService.CreatePatente(nuevaPatente);


                string messageKey = "Patente creada con éxito.";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage);
                LoggerService.WriteLog($"Patente creada con éxito: {nombre}. Usuario: {SesionService.UsuarioLogueado.UserName}", System.Diagnostics.TraceLevel.Info);
                this.Close();
            }
            catch (Exception ex)
            {
                string messageKey = "Ocurrió un error al crear la patente";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteLog($"Error al crear patente. Excepción: {ex.Message}. Usuario: {SesionService.UsuarioLogueado.UserName}", System.Diagnostics.TraceLevel.Error);
            }
        }
    }
}
