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
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Stock = ObtenerStockProducto(p.IdProducto),
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
                Producto productoSeleccionado = (Producto)dataGridView1.SelectedRows[0].DataBoundItem;
                ModificarProductoForm formModificar = new ModificarProductoForm(productoSeleccionado);
                formModificar.ShowDialog();
                CargarProductos(); // Refresca la lista después de modificar
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
                Producto productoSeleccionado = (Producto)dataGridView1.SelectedRows[0].DataBoundItem;

                string confirmMessage = IdiomaService.Translate("¿Está seguro de eliminar el producto?");
                string confirmTitle = IdiomaService.Translate("Confirmar eliminación");
                DialogResult result = MessageBox.Show($"{confirmMessage} {productoSeleccionado.Nombre}?",
                                                      confirmTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _productoService.EliminarProducto(productoSeleccionado.IdProducto);
                        CargarProductos();
                    }
                    catch (Exception ex)
                    {
                        string errorTitle = IdiomaService.Translate("Error");
                        MessageBox.Show("Error al eliminar el producto: " + ex.Message, errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
