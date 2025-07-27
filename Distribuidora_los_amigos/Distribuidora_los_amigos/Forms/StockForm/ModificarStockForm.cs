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

namespace Distribuidora_los_amigos.Forms.StockForm
{
    public partial class ModificarStockForm : Form
    {
        private readonly StockService _stockService;
        private Stock _stock;

        public ModificarStockForm(Stock stock)
        {
            InitializeComponent();
            _stockService = new StockService();
            _stock = stock;

            // Cargar los datos en los campos
            numericUpDownStock.Value = _stock.Cantidad;
            comboBoxTipoStock.Text = _stock.Tipo;
        }

        private void btnGuardarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que la cantidad no sea negativa
                int nuevaCantidad = (int)numericUpDownStock.Value;
                if (nuevaCantidad < 0)
                {
                    MessageBox.Show("La cantidad de stock no puede ser negativa.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Asignar los nuevos valores al stock
                _stock.Cantidad = nuevaCantidad;
                _stock.Tipo = comboBoxTipoStock.Text;

                // Actualizar en la base de datos
                _stockService.ModificarStock(_stock.IdProducto, _stock.Cantidad);

                MessageBox.Show("Stock modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el stock: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
