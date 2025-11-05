using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DOMAIN;
using Service.DAL.Contracts;
using Service.Facade;
using Services.Facade;

namespace Distribuidora_los_amigos.Forms.Clientes
{
    public partial class CrearClienteForm : Form, IIdiomaObserver
    {
        private readonly ClienteService _clienteService;

        /// <summary>
        /// Inicializa el formulario de creación de clientes y prepara el servicio asociado.
        /// </summary>
        public CrearClienteForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _clienteService = new ClienteService();
            
            // Habilitar captura de teclas para F1
            this.KeyPreview = true;
            this.KeyDown += CrearClienteForm_KeyDown;
            
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
        /// Valida los datos ingresados y registra un nuevo cliente en el sistema.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonGuardarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                // 🆕 VALIDACIONES ANTES DE CREAR
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    string messageKey = "El nombre es obligatorio.";
                    string translatedMessage = IdiomaService.Translate(messageKey);
                    string titleKey = "Error";
                    string translatedTitle = IdiomaService.Translate(titleKey);
                    MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox1.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    string messageKey = "El email es obligatorio.";
                    string translatedMessage = IdiomaService.Translate(messageKey);
                    string titleKey = "Error";
                    string translatedTitle = IdiomaService.Translate(titleKey);
                    MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox3.Focus();
                    return;
                }

                // 🔧 VALIDAR QUE EMAIL NO SEA TELÉFONO
                if (System.Text.RegularExpressions.Regex.IsMatch(textBox3.Text.Trim(), @"^\d{8,15}$"))
                {
                    string messageKey = "Ha ingresado un número de teléfono en el campo Email.\nPor favor ingrese un email válido (ejemplo: nombre@empresa.com).";
                    string translatedMessage = IdiomaService.Translate(messageKey);
                    string titleKey = "Error de Formato";
                    string translatedTitle = IdiomaService.Translate(titleKey);
                    MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox3.Focus();
                    textBox3.SelectAll();
                    return;
                }

                // 🔧 VALIDAR FORMATO DE EMAIL
                if (!textBox3.Text.Contains("@"))
                {
                    string messageKey = "El email debe contener '@'.\nFormato correcto: nombre@empresa.com";
                    string translatedMessage = IdiomaService.Translate(messageKey);
                    string titleKey = "Error de Formato";
                    string translatedTitle = IdiomaService.Translate(titleKey);
                    MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox3.Focus();
                    return;
                }

                // 🔧 VALIDAR QUE TELÉFONO NO SEA EMAIL
                if (textBox4.Text.Contains("@"))
                {
                    string messageKey = "Ha ingresado un email en el campo Teléfono.\nPor favor ingrese solo números.";
                    string translatedMessage = IdiomaService.Translate(messageKey);
                    string titleKey = "Error de Formato";
                    string translatedTitle = IdiomaService.Translate(titleKey);
                    MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox4.Focus();
                    textBox4.SelectAll();
                    return;
                }

                Cliente cliente = new Cliente()
                {
                    IdCliente = Guid.NewGuid(),
                    Nombre = textBox1.Text.Trim(),
                    Direccion = textBox2.Text.Trim(),
                    Email = textBox3.Text.Trim(),
                    Telefono = textBox4.Text.Trim(),
                    CUIT = textBox5.Text.Trim(),
                    Activo = checkBox1.Checked
                };

                _clienteService.CrearCliente(cliente);
                
                string successMessageKey = "Cliente creado correctamente.";
                string translatedSuccessMessage = IdiomaService.Translate(successMessageKey);
                string successTitleKey = "Éxito";
                string translatedSuccessTitle = IdiomaService.Translate(successTitleKey);
                MessageBox.Show(translatedSuccessMessage, translatedSuccessTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar los campos
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                checkBox1.Checked = false;
            }
            catch (Exception ex)
            {
                string errorTitleKey = "Error";
                string translatedErrorTitle = IdiomaService.Translate(errorTitleKey);
                MessageBox.Show(translatedErrorTitle + ": " + ex.Message, translatedErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Desuscribirse del servicio de idiomas al cerrar el formulario.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            IdiomaService.Unsubscribe(this);
            base.OnFormClosing(e);
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void CrearClienteForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaCrearCliente();
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
    }
}
