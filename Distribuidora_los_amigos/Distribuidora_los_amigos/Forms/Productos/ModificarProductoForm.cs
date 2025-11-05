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

namespace Distribuidora_los_amigos.Forms.Productos
{
    public partial class ModificarProductoForm : Form, IIdiomaObserver
    {
        private readonly ProductoService _productoService;
        private Producto _producto;

        /// <summary>
        /// Constructor por defecto requerido por el diseñador.
        /// </summary>
        public ModificarProductoForm()
        {
        }

        /// <summary>
        /// Inicializa el formulario cargando los datos del producto a modificar.
        /// </summary>
        /// <param name="producto">Producto seleccionado para edición.</param>
        public ModificarProductoForm(Producto producto)
        {
            InitializeComponent();
            _productoService = new ProductoService();
            _producto = producto;

            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);
            
            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);

            // Configurar ayuda F1
            this.KeyPreview = true;
            this.KeyDown += ModificarProductoForm_KeyDown;

            // Cargar los datos en los campos del formulario
            textBoxNombreProducto.Text = _producto.Nombre;
            comboBox2.Text = _producto.Categoria;
            numericUpDownPrecioProducto.Value = _producto.Precio;

            // Cargar fechas
            dateTimePicker1.Value = _producto.FechaIngreso;
            if (_producto.Vencimiento.HasValue)
            {
                Disponible.Checked = true;
                dateTimePicker2.Value = _producto.Vencimiento.Value;
            }
            else
            {
                Disponible.Checked = false;
                dateTimePicker2.Enabled = false;
            }
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        private void ModificarProductoForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaModificarProducto();
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
        /// Valida la información ingresada y actualiza el producto en la base de datos.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void btnGuardarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que el precio sea un número válido
                if (!decimal.TryParse(numericUpDownPrecioProducto.Text, out decimal precio) || precio <= 0)
                {
                    string errorMessage = IdiomaService.Translate("Error: El precio debe ser un número positivo.");
                    string errorTitle = IdiomaService.Translate("Error");
                    MessageBox.Show(errorMessage, errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar que la categoría no esté vacía
                if (string.IsNullOrWhiteSpace(comboBox2.Text))
                {
                    string errorMessage = IdiomaService.Translate("Error: Debes seleccionar una categoría.");
                    string errorTitle = IdiomaService.Translate("Error");
                    MessageBox.Show(errorMessage, errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener las fechas de ingreso y vencimiento
                DateTime fechaIngreso = dateTimePicker1.Value.Date;
                DateTime? fechaVencimiento = dateTimePicker2.Checked ? (DateTime?)dateTimePicker2.Value.Date : null;

                // Validar que la fecha de vencimiento no sea menor a la fecha de ingreso
                if (fechaVencimiento.HasValue && fechaVencimiento < fechaIngreso)
                {
                    string errorMessage = IdiomaService.Translate("Error: La fecha de vencimiento no puede ser anterior a la fecha de ingreso.");
                    string errorTitle = IdiomaService.Translate("Error");
                    MessageBox.Show(errorMessage, errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Actualizar los valores del producto
                _producto.Nombre = textBoxNombreProducto.Text.Trim();
                _producto.Categoria = comboBox2.Text.Trim();
                _producto.Precio = precio;
                _producto.FechaIngreso = fechaIngreso;
                _producto.Vencimiento = fechaVencimiento;

                // Guardar los cambios
                _productoService.ModificarProducto(_producto);
                string successMessage = IdiomaService.Translate("Producto modificado correctamente.");
                string successTitle = IdiomaService.Translate("Éxito");
                MessageBox.Show(successMessage, successTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                string errorTitle = IdiomaService.Translate("Error");
                MessageBox.Show("Error al modificar el producto: " + ex.Message, errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Placeholder del evento Load para futuras inicializaciones.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void ModificarProductoForm_Load(object sender, EventArgs e)
        {

        }
    }
}
