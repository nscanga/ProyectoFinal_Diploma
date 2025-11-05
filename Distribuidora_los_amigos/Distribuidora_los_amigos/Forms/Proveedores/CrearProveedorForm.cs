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
    public partial class CrearProveedorForm : Form, IIdiomaObserver
    {
        private readonly ProveedorService _proveedorService;

        /// <summary>
        /// Inicializa el formulario de creación de proveedor.
        /// </summary>
        public CrearProveedorForm()
        {
            InitializeComponent();
            _proveedorService = new ProveedorService();

            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);

            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);

            // Configurar ayuda F1
            this.KeyPreview = true;
            this.KeyDown += CrearProveedorForm_KeyDown;
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        private void CrearProveedorForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaCrearProveedor();
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
        /// Construye un proveedor a partir de la entrada del usuario y lo registra en la base de datos.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonCrearProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                Proveedor nuevoProveedor = new Proveedor()
                {
                    IdProveedor = Guid.NewGuid(),
                    Nombre = textBoxNombreProveedor.Text.Trim(),
                    Direccion = textBoxMDireccionProveedor.Text.Trim(),
                    Email = textBoxEmailProveedor.Text.Trim(),
                    Telefono = textBoxTelefonoProveedor.Text.Trim(),
                    Categoria = comboBoxCategoriaProveedor.Text.Trim(),
                    Activo = checkBoxProveedor.Checked
                };

                _proveedorService.CrearProveedor(nuevoProveedor);

                string successKey = "ProveedorCreadoExito";
                string translatedSuccess = IdiomaService.Translate(successKey);
                string successTitleKey = "Éxito";
                string translatedSuccessTitle = IdiomaService.Translate(successTitleKey);
                MessageBox.Show(translatedSuccess, translatedSuccessTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                string errorKey = "ErrorCrearProveedor";
                string translatedError = IdiomaService.Translate(errorKey) + ": " + ex.Message;
                string errorTitleKey = "Error";
                string translatedErrorTitle = IdiomaService.Translate(errorTitleKey);
                MessageBox.Show(translatedError, translatedErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
