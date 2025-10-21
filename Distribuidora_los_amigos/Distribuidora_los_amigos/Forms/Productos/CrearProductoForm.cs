using System;
using System.Collections;
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

namespace Distribuidora_los_amigos.Forms.Productos
{
    public partial class CrearProductoForm : Form
    {
        private readonly ProductoService _productoService;
        
        /// <summary>
        /// Inicializa el formulario de creación de productos cargando catálogos auxiliares.
        /// </summary>
        public CrearProductoForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _productoService = new ProductoService();
            this.KeyPreview = true;
            
            // Cargar datos en los ComboBox
            CargarCategorias();
            CargarTiposStock();
        }

        /// <summary>
        /// Rellena el combo de categorías disponibles para los productos.
        /// </summary>
        private void CargarCategorias()
        {
            comboBoxCrearProducto.Items.Clear();
            comboBoxCrearProducto.Items.Add("Pollo Fresco");
            comboBoxCrearProducto.Items.Add("Pollo Congelado");
            comboBoxCrearProducto.Items.Add("Menudencias");
            comboBoxCrearProducto.Items.Add("Embutidos de Pollo");
            comboBoxCrearProducto.Items.Add("Huevos");
            comboBoxCrearProducto.Items.Add("Productos Elaborados");
            comboBoxCrearProducto.Items.Add("Condimentos y Salsas");
            comboBoxCrearProducto.Items.Add("Insumos");
            
            comboBoxCrearProducto.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Inicializa las opciones de unidad de medida para el stock.
        /// </summary>
        private void CargarTiposStock()
        {
            comboBoxTipoStock.Items.Clear();
            comboBoxTipoStock.Items.Add("Unidad");
            comboBoxTipoStock.Items.Add("Kilogramo");
            comboBoxTipoStock.Items.Add("Caja");
            comboBoxTipoStock.Items.Add("Bandeja");
            comboBoxTipoStock.Items.Add("Maple");
            comboBoxTipoStock.Items.Add("Docena");
            comboBoxTipoStock.Items.Add("Paquete");
            comboBoxTipoStock.Items.Add("Litro");
            
            comboBoxTipoStock.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Valida los datos ingresados y crea un nuevo producto con su stock inicial.
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
                if (string.IsNullOrWhiteSpace(comboBoxCrearProducto.Text))
                {
                    MessageBox.Show("Error: Debes seleccionar una categoría.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar que el tipo de stock no esté vacío
                if (string.IsNullOrWhiteSpace(comboBoxTipoStock.Text))
                {
                    MessageBox.Show("Error: Debes seleccionar un tipo de stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener la cantidad inicial de stock y el tipo
                int cantidadStock = (int)numericUpDownStock.Value;
                string tipoStock = comboBoxTipoStock.Text;

                // Obtener las fechas de ingreso y vencimiento
                DateTime fechaIngreso = dateTimePicker1.Value.Date;
                DateTime? fechaVencimiento = dateTimePicker2.Checked ? (DateTime?)dateTimePicker2.Value.Date : null;

                // Validar que la fecha de vencimiento no sea menor a la fecha de ingreso
                if (fechaVencimiento.HasValue && fechaVencimiento < fechaIngreso)
                {
                    MessageBox.Show("Error: La fecha de vencimiento no puede ser anterior a la fecha de ingreso.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Producto producto = new Producto()
                {
                    IdProducto = Guid.NewGuid(),
                    Nombre = textBoxNombreProducto.Text.Trim(),
                    Categoria = comboBoxCrearProducto.Text.Trim(),
                    Precio = precio,
                    FechaIngreso = fechaIngreso,
                    Vencimiento = fechaVencimiento,
                    Activo = true
                };

                // Guardar el producto
                _productoService.CrearProducto(producto, cantidadStock, tipoStock);
                
                LoggerService.WriteLog($"Producto creado: {producto.Nombre}, Categoría: {producto.Categoria}, Cantidad inicial: {cantidadStock}", System.Diagnostics.TraceLevel.Info);

                string messageKey = "Producto creado correctamente.";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar los campos
                textBoxNombreProducto.Clear();
                comboBoxCrearProducto.SelectedIndex = -1;
                numericUpDownPrecioProducto.Value = 0;
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
                dateTimePicker2.Checked = false;
                numericUpDownStock.Value = 0;
                comboBoxTipoStock.SelectedIndex = -1;
                
                textBoxNombreProducto.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Traduce la clave textual usando el servicio de idiomas.
        /// </summary>
        /// <param name="messageKey">Clave a traducir.</param>
        /// <returns>Mensaje localizado.</returns>
        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }

        /// <summary>
        /// Placeholder para reaccionar a cambios de categoría seleccionada.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
