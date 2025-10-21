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
        
        /// <summary>
        /// Inicializa el formulario de stock configurando servicios y eventos de carga.
        /// </summary>
        public MostrarStockForm()
        {
            InitializeComponent();
            _stockService = new StockService();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += MostrarStockForm_Load;
        }

        /// <summary>
        /// Carga el listado de stock y ajusta la grilla al iniciar el formulario.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void MostrarStockForm_Load(object sender, EventArgs e)
        {
            CargarStock();
            ConfigurarDataGridView();
        }

        /// <summary>
        /// Configura visibilidad, encabezados y formato de columnas para el grid de stock.
        /// </summary>
        private void ConfigurarDataGridView()
        {
            // Ocultar las columnas de IDs que no son amigables para el usuario
            if (dataGridViewStock.Columns["IdStock"] != null)
                dataGridViewStock.Columns["IdStock"].Visible = false;
            
            if (dataGridViewStock.Columns["IdProducto"] != null)
                dataGridViewStock.Columns["IdProducto"].Visible = false;

            // Configurar nombres de encabezados más amigables
            if (dataGridViewStock.Columns["NombreProducto"] != null)
                dataGridViewStock.Columns["NombreProducto"].HeaderText = "Producto";
            
            if (dataGridViewStock.Columns["Categoria"] != null)
                dataGridViewStock.Columns["Categoria"].HeaderText = "Categoría";
            
            if (dataGridViewStock.Columns["Cantidad"] != null)
                dataGridViewStock.Columns["Cantidad"].HeaderText = "Cantidad";
            
            if (dataGridViewStock.Columns["Tipo"] != null)
                dataGridViewStock.Columns["Tipo"].HeaderText = "Tipo de Stock";
            
            if (dataGridViewStock.Columns["PrecioUnitario"] != null)
            {
                dataGridViewStock.Columns["PrecioUnitario"].HeaderText = "Precio";
                dataGridViewStock.Columns["PrecioUnitario"].DefaultCellStyle.Format = "C2"; // Formato de moneda
            }
            
            if (dataGridViewStock.Columns["Activo"] != null)
                dataGridViewStock.Columns["Activo"].HeaderText = "Activo";

            // Opcional: Configurar orden de columnas
            int orden = 0;
            if (dataGridViewStock.Columns["NombreProducto"] != null)
                dataGridViewStock.Columns["NombreProducto"].DisplayIndex = orden++;
            
            if (dataGridViewStock.Columns["Categoria"] != null)
                dataGridViewStock.Columns["Categoria"].DisplayIndex = orden++;
            
            if (dataGridViewStock.Columns["Cantidad"] != null)
                dataGridViewStock.Columns["Cantidad"].DisplayIndex = orden++;
            
            if (dataGridViewStock.Columns["Tipo"] != null)
                dataGridViewStock.Columns["Tipo"].DisplayIndex = orden++;
            
            if (dataGridViewStock.Columns["PrecioUnitario"] != null)
                dataGridViewStock.Columns["PrecioUnitario"].DisplayIndex = orden++;
            
            if (dataGridViewStock.Columns["Activo"] != null)
                dataGridViewStock.Columns["Activo"].DisplayIndex = orden++;

            // Ajustar ancho de columnas
            dataGridViewStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Obtiene el stock con detalles de producto y lo vincula a la grilla.
        /// </summary>
        private void CargarStock()
        {
            try
            {
                // 🆕 USAR EL NUEVO MÉTODO CON DETALLES
                List<StockDTO> stockList = _stockService.ObtenerStockConDetalles();
                dataGridViewStock.DataSource = stockList;
                
                LoggerService.WriteLog($"Se cargó la lista de stock en {this.Text}", System.Diagnostics.TraceLevel.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el stock: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Abre el formulario de modificación para el stock seleccionado.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonModificarStock_Click(object sender, EventArgs e)
        {
            if (dataGridViewStock.SelectedRows.Count > 0)
            {
                // Obtener el StockDTO seleccionado
                StockDTO stockDTOSeleccionado = (StockDTO)dataGridViewStock.SelectedRows[0].DataBoundItem;
                
                // Crear objeto Stock a partir del DTO para pasar al formulario de modificación
                Stock stockSeleccionado = new Stock()
                {
                    IdStock = stockDTOSeleccionado.IdStock,
                    IdProducto = stockDTOSeleccionado.IdProducto,
                    Cantidad = stockDTOSeleccionado.Cantidad,
                    Tipo = stockDTOSeleccionado.Tipo,
                    Activo = stockDTOSeleccionado.Activo
                };
                
                ModificarStockForm formModificar = new ModificarStockForm(stockSeleccionado);
                formModificar.ShowDialog();
                CargarStock(); // Refresca la lista después de modificar
            }
            else
            {
                MessageBox.Show("Seleccione un producto para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Elimina el registro de stock seleccionado tras confirmación del usuario.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void Eliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewStock.SelectedRows.Count > 0)
            {
                // Obtener el stock seleccionado
                StockDTO stockDTOSeleccionado = (StockDTO)dataGridViewStock.SelectedRows[0].DataBoundItem;

                // Confirmación antes de eliminar (ahora muestra el nombre del producto)
                DialogResult resultado = MessageBox.Show(
                    $"¿Estás seguro de que deseas eliminar el stock del producto: {stockDTOSeleccionado.NombreProducto}?",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (resultado == DialogResult.Yes)
                {
                    try
                    {
                        // Llamar al servicio para eliminar el stock
                        _stockService.EliminarStock(stockDTOSeleccionado.IdStock);

                        // Log de eliminación
                        LoggerService.WriteLog($"Se eliminó el stock con ID: {stockDTOSeleccionado.IdStock} - Producto: {stockDTOSeleccionado.NombreProducto}", System.Diagnostics.TraceLevel.Info);

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

        /// <summary>
        /// Refresca la lista de stock obteniendo la información nuevamente del servicio.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonActualizarProducto_Click(object sender, EventArgs e)
        {
            CargarStock();
        }
    }
}
