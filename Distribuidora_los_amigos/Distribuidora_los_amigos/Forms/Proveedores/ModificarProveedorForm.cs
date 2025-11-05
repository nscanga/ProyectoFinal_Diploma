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
using Service.Facade;
using Services.Facade;
using Service.DAL.Contracts;

namespace Distribuidora_los_amigos.Forms.Proveedores
{
    public partial class ModificarProveedorForm : Form, IIdiomaObserver
    {
        private readonly ProveedorService _proveedorService;
        private Proveedor _proveedor;

        /// <summary>
        /// Inicializa el formulario con los datos del proveedor a modificar.
        /// </summary>
        /// <param name="proveedor">Proveedor seleccionado para edición.</param>
        public ModificarProveedorForm(Proveedor proveedor)
        {
            InitializeComponent();
            _proveedorService = new ProveedorService();
            _proveedor = proveedor;

            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);

            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);

            // Configurar ayuda F1
            this.KeyPreview = true;
            this.KeyDown += ModificarProveedorForm_KeyDown;

            // Cargar los datos en los campos del formulario
            textBox1.Text = _proveedor.Nombre;
            textBox2.Text = _proveedor.Direccion;
            textBox3.Text = _proveedor.Email;
            textBox4.Text = _proveedor.Telefono;
            comboBox1.Text = _proveedor.Categoria;
            checkBox1.Checked = _proveedor.Activo;
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        private void ModificarProveedorForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaModificarProveedor();
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
        /// Actualiza los textos del formulario cuando cambia el idioma.
        /// </summary>
        public void UpdateIdioma()
        {
            IdiomaService.TranslateForm(this);
            this.Refresh();
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
        /// Guarda las modificaciones realizadas sobre el proveedor actual.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonGuardarProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                // Actualizar los datos del proveedor con los valores ingresados
                _proveedor.Nombre = textBox1.Text.Trim();
                _proveedor.Direccion = textBox2.Text.Trim();
                _proveedor.Email = textBox3.Text.Trim();
                _proveedor.Telefono = textBox4.Text.Trim();
                _proveedor.Categoria = comboBox1.Text.Trim();
                _proveedor.Activo = checkBox1.Checked;

                // Guardar los cambios en la base de datos
                _proveedorService.ModificarProveedor(_proveedor);

                string successKey = "ProveedorModificadoExito";
                string translatedSuccess = IdiomaService.Translate(successKey);
                string successTitleKey = "Éxito";
                string translatedSuccessTitle = IdiomaService.Translate(successTitleKey);
                MessageBox.Show(translatedSuccess, translatedSuccessTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                string errorKey = "ErrorModificarProveedor";
                string translatedError = IdiomaService.Translate(errorKey) + ": " + ex.Message;
                string errorTitleKey = "Error";
                string translatedErrorTitle = IdiomaService.Translate(errorTitleKey);
                MessageBox.Show(translatedError, translatedErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
