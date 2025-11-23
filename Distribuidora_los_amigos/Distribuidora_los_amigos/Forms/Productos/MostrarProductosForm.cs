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

namespace Distribuidora_los_amigos.Forms.Productos
{
    public partial class MostrarProductosForm : Form, IIdiomaObserver
    {
        private readonly ProductoService _productoService;
        private readonly StockService _stockService;
        private Producto _producto;

        /// <summary>
        /// Inicializa el listado de productos y registra el evento de carga.
        /// </summary>
        public MostrarProductosForm()
        {
            InitializeComponent();
            _productoService = new ProductoService();
            _stockService = new StockService();
            
            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);
            
            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);
            
            // Configurar ayuda F1
            this.KeyPreview = true;
            this.KeyDown += MostrarProductosForm_KeyDown;
            
            this.Load += MostrarProductosForm_Load;
        }

        /// <summary>
        /// Constructor auxiliar para mostrar productos desde una selección de stock.
        /// </summary>
        /// <param name="stockSeleccionado">Stock desde el que se abrió el listado.</param>
        public MostrarProductosForm(Stock stockSeleccionado)
        {
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        private void MostrarProductosForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaMostrarProductos();
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
        /// Carga los productos al inicializarse el formulario.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void MostrarProductosForm_Load(object sender, EventArgs e)
        {
            CargarProductos();
            ConfigurarDataGridView();
        }

        /// <summary>
        /// Configura la visualización del DataGridView ocultando columnas no relevantes para el usuario.
        /// </summary>
        private void ConfigurarDataGridView()
        {
            // Ocultar la columna de ID que no es relevante para el usuario
            if (dataGridView1.Columns["IdProducto"] != null)
                dataGridView1.Columns["IdProducto"].Visible = false;
            
            // Ocultar la columna de ProductoOriginal si existe
            if (dataGridView1.Columns["ProductoOriginal"] != null)
                dataGridView1.Columns["ProductoOriginal"].Visible = false;

            // Configurar nombres de encabezados más amigables
            if (dataGridView1.Columns["Nombre"] != null)
                dataGridView1.Columns["Nombre"].HeaderText = IdiomaService.Translate("Producto");
            
            if (dataGridView1.Columns["Categoria"] != null)
                dataGridView1.Columns["Categoria"].HeaderText = IdiomaService.Translate("Categoria");
            
            if (dataGridView1.Columns["Precio"] != null)
            {
                dataGridView1.Columns["Precio"].HeaderText = IdiomaService.Translate("Precio");
                dataGridView1.Columns["Precio"].DefaultCellStyle.Format = "C2"; // Formato de moneda
            }
            
            if (dataGridView1.Columns["Stock"] != null)
                dataGridView1.Columns["Stock"].HeaderText = IdiomaService.Translate("Stock");

            if (dataGridView1.Columns["FechaIngreso"] != null)
            {
                dataGridView1.Columns["FechaIngreso"].HeaderText = IdiomaService.Translate("Fecha Ingreso");
                dataGridView1.Columns["FechaIngreso"].DefaultCellStyle.Format = "dd/MM/yyyy"; // Formato de fecha
            }

            if (dataGridView1.Columns["Vencimiento"] != null)
            {
                dataGridView1.Columns["Vencimiento"].HeaderText = IdiomaService.Translate("Vencimiento");
                dataGridView1.Columns["Vencimiento"].DefaultCellStyle.Format = "dd/MM/yyyy"; // Formato de fecha
                
                // Opcional: resaltar productos próximos a vencer o vencidos
                // Se puede agregar formato condicional si lo deseas
            }

            // Ajustar ancho de columnas
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Obtiene los productos desde el servicio y los muestra en la grilla.
        /// </summary>
        private void CargarProductos()
        {
            try
            {
                List<Producto> listaProductos = _productoService.ObtenerTodosProductos();

                var productosEnriquecidos = listaProductos.Select(p => new
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.Nombre,
                    Categoria = p.Categoria,
                    Precio = p.Precio,
                    Stock = ObtenerStockProducto(p.IdProducto),
                    FechaIngreso = p.FechaIngreso,
                    Vencimiento = p.Vencimiento,
                    ProductoOriginal = p
                }).ToList();

                dataGridView1.DataSource = productosEnriquecidos;
            }
            catch (DatabaseException dbEx)
            {
                string username = ObtenerUsuarioActual();
                ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                dataGridView1.DataSource = new List<object>();
                Console.WriteLine("❌ Error de conexión al cargar productos");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralException(ex);
                dataGridView1.DataSource = new List<object>();
            }
        }

        private int ObtenerStockProducto(Guid idProducto)
        {
            try
            {
                var stock = _stockService.ObtenerStockPorProducto(idProducto);
                return stock?.Cantidad ?? 0;
            }
            catch (DatabaseException dbEx)
            {
                ErrorHandler.HandleDatabaseException(dbEx, ObtenerUsuarioActual(), showMessageBox: false);
                return 0;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralException(ex);
                return 0;
            }
        }

        /// <summary>
        /// Abre el formulario de creación de producto y recarga la lista al finalizar.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonCrearProducto2_Click(object sender, EventArgs e)
        {
            CrearProductoForm formCrear = new CrearProductoForm();
            formCrear.ShowDialog();
            CargarProductos(); // Refresca la lista después de crear
        }

        /// <summary>
        /// Abre el formulario de modificación para el producto seleccionado.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonModificarProducto2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                dynamic item = dataGridView1.SelectedRows[0].DataBoundItem;
                Producto productoSeleccionado = item.ProductoOriginal;
                ModificarProductoForm formModificar = new ModificarProductoForm(productoSeleccionado);
                formModificar.ShowDialog();
                CargarProductos();
            }
            else
            {
                string message = IdiomaService.Translate("Seleccione un producto para modificar.");
                string title = IdiomaService.Translate("Advertencia");
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Elimina el producto seleccionado previa confirmación del usuario.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    dynamic item = dataGridView1.SelectedRows[0].DataBoundItem;
                    Producto productoSeleccionado = item.ProductoOriginal;

                    string confirmMessage = IdiomaService.Translate("¿Está seguro de eliminar el producto?");
                    string confirmTitle = IdiomaService.Translate("Confirmar eliminación");
                    DialogResult result = MessageBox.Show($"{confirmMessage} {productoSeleccionado.Nombre}?",
                                                          confirmTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            _productoService.EliminarProducto(productoSeleccionado.IdProducto);
                            
                            string successMessage = IdiomaService.Translate("Producto eliminado correctamente");
                            string successTitle = IdiomaService.Translate("Éxito");
                            MessageBox.Show(successMessage, successTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            CargarProductos();
                        }
                        catch (InvalidOperationException invalidEx)
                        {
                            // Error de restricción de clave foránea o regla de negocio
                            LoggerService.WriteException(invalidEx);
                            
                            string errorTitle = IdiomaService.Translate("No se puede eliminar");
                            MessageBox.Show(invalidEx.Message, errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        catch (DatabaseException dbEx)
                        {
                            // Registrar en bitácora
                            LoggerService.WriteException(dbEx);
                            
                            // Manejar según el tipo de error
                            string username = ObtenerUsuarioActual();
                            ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                        }
                        catch (Exception ex)
                        {
                            // Registrar cualquier otro error
                            LoggerService.WriteException(ex);
                            
                            string errorTitle = IdiomaService.Translate("Error");
                            string errorMessage = IdiomaService.Translate("Error al eliminar el producto");
                            MessageBox.Show($"{errorMessage}: {ex.Message}", 
                                          errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Registrar error al obtener el producto seleccionado
                    LoggerService.WriteException(ex);
                    
                    string errorTitle = IdiomaService.Translate("Error");
                    string errorMessage = IdiomaService.Translate("Error al procesar la selección");
                    MessageBox.Show($"{errorMessage}: {ex.Message}", 
                                  errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                string message = IdiomaService.Translate("Seleccione un producto para eliminar.");
                string title = IdiomaService.Translate("Advertencia");
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Recarga manualmente el listado de productos.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonActualizarProducto_Click(object sender, EventArgs e)
        {
            CargarProductos();
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
