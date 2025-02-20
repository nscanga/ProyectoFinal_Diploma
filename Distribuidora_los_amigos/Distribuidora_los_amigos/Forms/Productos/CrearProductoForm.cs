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
        public CrearProductoForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _productoService = new ProductoService();
            this.KeyPreview = true;
            //this.KeyDown += AgregarProductoForm_KeyDown;
        }

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

                // Obtener la cantidad inicial de stock y el tipo
                int cantidadStock = int.Parse(numericUpDownStock.Text);
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
                    IdProducto = Guid.NewGuid(), // Generar un nuevo GUID
                    Nombre = textBoxNombreProducto.Text.Trim(),
                    Categoria = comboBoxCrearProducto.Text.Trim(),
                    Precio = precio,
                    FechaIngreso = fechaIngreso,
                    Vencimiento = fechaVencimiento,
                    Activo = true
                };

                MessageBox.Show("Producto y stock creados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Guardar el producto
                _productoService.CrearProducto(producto, cantidadStock, tipoStock);
                LoggerService.WriteLog($"Producto creado: {producto.Nombre}, Categoría: {producto.Categoria}, Cantidad inicial: {cantidadStock}", System.Diagnostics.TraceLevel.Info);

                string messageKey = "Producto creado correctamente.";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage);

                // Limpiar los campos
                textBoxNombreProducto.Text = "";
                comboBoxCrearProducto.SelectedIndex = -1;
                numericUpDownPrecioProducto.Value = 0;
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
                numericUpDownStock.Value = 0;
                comboBoxTipoStock.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }


        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }

        //private void AgregarProductoForm_KeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        if (e.KeyCode == Keys.F1)
        //        {
        //            ManualService manualService = new ManualService();
        //            manualService.AbrirAyudaAltaProducto();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error " + ex.Message, "Error");
        //        LoggerService.WriteException(ex);
        //    }
        //}

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
