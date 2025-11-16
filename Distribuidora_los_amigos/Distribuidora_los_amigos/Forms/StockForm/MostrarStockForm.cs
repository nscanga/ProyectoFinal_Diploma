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
using BLL.Exceptions;
using DOMAIN;
using Service.Facade;
using Services.Facade;
using Service.DAL.Contracts;
using Service.ManegerEx;

namespace Distribuidora_los_amigos.Forms.StockForm
{
    public partial class MostrarStockForm : Form, IIdiomaObserver
    {
        private readonly StockService _stockService;

        /// <summary>
        /// Inicializa el formulario de visualización de stock.
        /// </summary>
        public MostrarStockForm()
        {
            InitializeComponent();
            _stockService = new StockService();

            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);

            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);

            // Configurar ayuda F1
            this.KeyPreview = true;
            this.KeyDown += MostrarStockForm_KeyDown;
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        private void MostrarStockForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaMostrarStock();
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
        /// Carga el listado de stock y ajusta la grilla al iniciar el formulario.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void MostrarStockForm_Load(object sender, EventArgs e)
        {
            try
            {
                CargarStock();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralException(ex);
            }
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

            // Configurar nombres de encabezados más amigables con traducciones
            if (dataGridViewStock.Columns["NombreProducto"] != null)
                dataGridViewStock.Columns["NombreProducto"].HeaderText = IdiomaService.Translate("Producto");
            
            if (dataGridViewStock.Columns["Categoria"] != null)
                dataGridViewStock.Columns["Categoria"].HeaderText = IdiomaService.Translate("Categoria");
            
            if (dataGridViewStock.Columns["Cantidad"] != null)
                dataGridViewStock.Columns["Cantidad"].HeaderText = IdiomaService.Translate("Cantidad");
            
            if (dataGridViewStock.Columns["Tipo"] != null)
                dataGridViewStock.Columns["Tipo"].HeaderText = IdiomaService.Translate("TipoStock");
            
            if (dataGridViewStock.Columns["PrecioUnitario"] != null)
            {
                dataGridViewStock.Columns["PrecioUnitario"].HeaderText = IdiomaService.Translate("Precio");
                dataGridViewStock.Columns["PrecioUnitario"].DefaultCellStyle.Format = "C2"; // Formato de moneda
            }
            
            if (dataGridViewStock.Columns["Activo"] != null)
                dataGridViewStock.Columns["Activo"].HeaderText = IdiomaService.Translate("Activo");

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

                // Configurar DataGridView después de cargar los datos
                ConfigurarDataGridView();

                LoggerService.WriteLog($"Se cargó la lista de stock en {this.Text}", System.Diagnostics.TraceLevel.Info);
            }
            catch (DatabaseException dbEx)
            {
                string username = ObtenerUsuarioActual();
                ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                dataGridViewStock.DataSource = new List<object>();
                Console.WriteLine("❌ Error de conexión al cargar stock");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralException(ex);
                dataGridViewStock.DataSource = new List<object>();
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
                string messageKey = "SeleccioneStockModificar";
                string translatedMessage = IdiomaService.Translate(messageKey);
                string titleKey = "Advertencia";
                string translatedTitle = IdiomaService.Translate(titleKey);
                MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                string messageKey = "ConfirmarEliminarStock";
                string translatedMessage = string.Format(IdiomaService.Translate(messageKey), stockDTOSeleccionado.NombreProducto);
                string titleKey = "ConfirmarEliminacion";
                string translatedTitle = IdiomaService.Translate(titleKey);
                
                DialogResult resultado = MessageBox.Show(
                    translatedMessage,
                    translatedTitle,
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

                        string successKey = "StockEliminadoExito";
                        string translatedSuccess = IdiomaService.Translate(successKey);
                        string successTitleKey = "Éxito";
                        string translatedSuccessTitle = IdiomaService.Translate(successTitleKey);
                        MessageBox.Show(translatedSuccess, translatedSuccessTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        string errorKey = "ErrorEliminarStock";
                        string translatedError = IdiomaService.Translate(errorKey) + ": " + ex.Message;
                        string errorTitleKey = "Error";
                        string translatedErrorTitle = IdiomaService.Translate(errorTitleKey);
                        MessageBox.Show(translatedError, translatedErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LoggerService.WriteException(ex);
                    }
                }
            }
            else
            {
                string messageKey = "SeleccioneStockEliminar";
                string translatedMessage = IdiomaService.Translate(messageKey);
                string titleKey = "Advertencia";
                string translatedTitle = IdiomaService.Translate(titleKey);
                MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private string ObtenerUsuarioActual()
        {
            try
            {
                return SesionService.UsuarioLogueado?.UserName ?? "Desconocido";
            }
            catch
            {
                return "Desconocido";
            }
        }
    }
}
