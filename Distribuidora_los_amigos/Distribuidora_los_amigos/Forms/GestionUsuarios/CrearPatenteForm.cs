using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Service.DOMAIN;
using Service.Facade;
using Services.Facade;

namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    public partial class CrearPatenteForm : Form
    {
        public CrearPatenteForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.KeyDown += CrearPatenteForm_KeyDown;
        }

        private void CrearPatenteForm_Load(object sender, EventArgs e)
        {
            try
            {
                IdiomaService.TranslateForm(this);
                comboBoxAccessType.DataSource = Enum.GetValues(typeof(TipoAcceso));
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Validar los campos
            string nombre = textBoxName.Text.Trim();
            string dataKey = textBoxDataKey.Text.Trim();
            TipoAcceso tipoAcceso = (TipoAcceso)comboBoxAccessType.SelectedItem;

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(dataKey))
            {
                string messageKey = "Por favor, ingrese un nombre y la datakey.";
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
                    DataKey = dataKey,
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

        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }

        private void CrearPatenteForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {

                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaCrearPatente();
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
