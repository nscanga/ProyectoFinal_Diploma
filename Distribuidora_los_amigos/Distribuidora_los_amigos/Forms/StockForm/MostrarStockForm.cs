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
using Distribuidora_los_amigos.Forms.Productos;
using DOMAIN;
using Services.Facade;

namespace Distribuidora_los_amigos.Forms.StockForm
{
    public partial class MostrarStockForm : Form
    {

        private readonly StockService _stockService;
        public MostrarStockForm()
        {
            InitializeComponent();
            _stockService = new StockService();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += MostrarStockForm_Load;

        }

        private void MostrarStockForm_Load(object sender, EventArgs e)
        {
            CargarStock();
        }

        private void CargarStock()
        {
            try
            {
                List<Stock> stockList = _stockService.ObtenerStock();
                dataGridViewStock.DataSource = stockList;
                LoggerService.WriteLog($"Se cargó la lista de stock en {this.Text}", System.Diagnostics.TraceLevel.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el stock: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        private void buttonModificarStock_Click(object sender, EventArgs e)
        {
            if (dataGridViewStock.SelectedRows.Count > 0)
            {
                // Obtener el objeto Stock seleccionado
                Stock stockSeleccionado = (Stock)dataGridViewStock.SelectedRows[0].DataBoundItem;
                ModificarStockForm formModificar = new ModificarStockForm(stockSeleccionado);
                formModificar.ShowDialog();
                CargarStock(); // Refresca la lista después de modificar
            }
            else
            {
                MessageBox.Show("Seleccione un producto para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewStock.SelectedRows.Count > 0)
            {
                // Obtener el stock seleccionado
                Stock stockSeleccionado = (Stock)dataGridViewStock.SelectedRows[0].DataBoundItem;

                // Confirmación antes de eliminar
                DialogResult resultado = MessageBox.Show(
                    $"¿Estás seguro de que deseas eliminar el stock del producto: {stockSeleccionado.IdProducto}?",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (resultado == DialogResult.Yes)
                {
                    try
                    {
                        // Llamar al servicio para eliminar el stock
                        _stockService.EliminarStock(stockSeleccionado.IdStock);

                        // Log de eliminación
                        LoggerService.WriteLog($"Se eliminó el stock con ID: {stockSeleccionado.IdStock}", System.Diagnostics.TraceLevel.Info);

                        // Refrescar la lista después de la eliminación
                        CargarStock();

                        MessageBox.Show("Stock eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el stock: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LoggerService.WriteException(ex);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un stock para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonActualizarProducto_Click(object sender, EventArgs e)
        {
            CargarStock();
        }
    }
}
