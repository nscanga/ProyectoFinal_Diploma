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

namespace Distribuidora_los_amigos.Forms.Productos
{
    public partial class ModificarProductoForm : Form
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
                    MessageBox.Show("Error: El precio debe ser un número positivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar que la categoría no esté vacía
                if (string.IsNullOrWhiteSpace(comboBox2.Text))
                {
                    MessageBox.Show("Error: Debes seleccionar una categoría.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener las fechas de ingreso y vencimiento
                DateTime fechaIngreso = dateTimePicker1.Value.Date;
                DateTime? fechaVencimiento = dateTimePicker2.Checked ? (DateTime?)dateTimePicker2.Value.Date : null;

                // Validar que la fecha de vencimiento no sea menor a la fecha de ingreso
                if (fechaVencimiento.HasValue && fechaVencimiento < fechaIngreso)
                {
                    MessageBox.Show("Error: La fecha de vencimiento no puede ser anterior a la fecha de ingreso.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("Producto modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
