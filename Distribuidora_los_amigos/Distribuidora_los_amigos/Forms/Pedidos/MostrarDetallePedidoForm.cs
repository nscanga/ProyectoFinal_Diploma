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

namespace Distribuidora_los_amigos.Forms.Pedidos
{
    public partial class MostrarDetallePedidoForm : Form, IIdiomaObserver
    {
        private readonly PedidoService _pedidoService;
        private readonly DetallePedido _detallePedido;
        private Pedido _pedidoSeleccionado;
        private readonly DetallePedidoService _detallePedidoService;
        private readonly ProductoService _productoService;
        private readonly PdfService _pdfService; // 🆕 Agregar PdfService

        /// <summary>
        /// Inicializa la vista de detalle de pedido configurando servicios y controles.
        /// </summary>
        /// <param name="pedido">Pedido seleccionado cuyos detalles se mostrarán.</param>
        public MostrarDetallePedidoForm(Pedido pedido)
        {
            InitializeComponent();
            _pedidoService = new PedidoService();
            _detallePedidoService = new DetallePedidoService();
            _productoService = new ProductoService();
            _pdfService = new PdfService(); // 🆕 Inicializar PdfService
            _pedidoSeleccionado = pedido;

            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);
            
            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);

            // Configurar ayuda F1
            this.KeyPreview = true;
            this.KeyDown += MostrarDetallePedidoForm_KeyDown;

            ConfigurarDataGridView();
            CargarDetallesPedido();
            ConfigurarBotonPdf(); // 🆕 Configurar botón PDF
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        private void MostrarDetallePedidoForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaDetallePedido();
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

        // 🆕 Configurar visibilidad del botón PDF según el estado
        /// <summary>
        /// Crea y muestra el botón para generar PDF cuando el pedido está entregado.
        /// </summary>
        private void ConfigurarBotonPdf()
        {
            // Obtener el estado del pedido
            string estadoPedido = _pedidoService.ObtenerNombreEstadoPorId(_pedidoSeleccionado.IdEstadoPedido);
            
            // Solo mostrar el botón si el pedido está "Entregado"
            if (estadoPedido.Equals("Entregado", StringComparison.OrdinalIgnoreCase))
            {
                // Verificar si ya existe el botón, si no, crearlo
                if (this.Controls.Find("buttonGenerarPdf", true).Length == 0)
                {
                    Button buttonGenerarPdf = new Button();
                    buttonGenerarPdf.Name = "buttonGenerarPdf";
                    buttonGenerarPdf.Text = "📄 Generar PDF";
                    buttonGenerarPdf.Size = new System.Drawing.Size(120, 35);
                    buttonGenerarPdf.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
                    buttonGenerarPdf.ForeColor = System.Drawing.Color.White;
                    buttonGenerarPdf.FlatStyle = FlatStyle.Flat;
                    buttonGenerarPdf.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
                    
                    // Posicionar al lado del botón Cerrar
                    Button buttonCerrar = this.Controls.Find("buttonCerrar", true).FirstOrDefault() as Button;
                    if (buttonCerrar != null)
                    {
                        buttonGenerarPdf.Location = new System.Drawing.Point(
                            buttonCerrar.Location.X - buttonGenerarPdf.Width - 10, 
                            buttonCerrar.Location.Y
                        );
                    }
                    else
                    {
                        // Posición por defecto si no encuentra el botón cerrar
                        buttonGenerarPdf.Location = new System.Drawing.Point(this.Width - 250, this.Height - 70);
                    }
                    
                    buttonGenerarPdf.Click += ButtonGenerarPdf_Click;
                    this.Controls.Add(buttonGenerarPdf);
                    buttonGenerarPdf.BringToFront();
                }
            }
        }

        // 🆕 Evento del botón Generar PDF
        /// <summary>
        /// Genera el comprobante en PDF del pedido y ofrece abrirlo al finalizar.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void ButtonGenerarPdf_Click(object sender, EventArgs e)
        {
            try
            {
                // Generar nombre de archivo sugerido
                string nombreArchivo = _pdfService.GenerarNombreArchivoPdf(_pedidoSeleccionado);
                
                // Mostrar diálogo para guardar archivo
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Archivos PDF (*.pdf)|*.pdf";
                saveFileDialog.Title = "Guardar Comprobante de Pedido";
                saveFileDialog.FileName = nombreArchivo;
                saveFileDialog.DefaultExt = "pdf";
                
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Generar el PDF
                    _pdfService.GenerarPdfPedido(_pedidoSeleccionado, saveFileDialog.FileName);
                    
                    // Preguntar si quiere abrir el archivo
                    DialogResult result = MessageBox.Show(
                        "PDF generado correctamente.\n¿Desea abrirlo ahora?", 
                        "PDF Generado", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Information
                    );
                    
                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🆕 Configurar las columnas del DataGridView para mejor UX
        /// <summary>
        /// Configura las columnas visibles y estilos del grid de detalles de pedido.
        /// </summary>
        private void ConfigurarDataGridView()
        {
            dataGridViewDetallePedido.AutoGenerateColumns = false;
            dataGridViewDetallePedido.Columns.Clear();

            // Ocultar IDs que no son útiles para el usuario
            dataGridViewDetallePedido.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdDetallePedido",
                HeaderText = "ID Detalle",
                Name = "IdDetallePedido",
                Visible = false // 🔹 Ocultar GUID
            });

            dataGridViewDetallePedido.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdPedido",
                HeaderText = "ID Pedido",
                Name = "IdPedido",
                Visible = false // 🔹 Ocultar GUID
            });

            dataGridViewDetallePedido.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdProducto",
                HeaderText = "ID Producto",
                Name = "IdProducto",
                Visible = false // 🔹 Ocultar GUID
            });

            // 🎯 MOSTRAR EL NOMBRE DEL PRODUCTO en lugar del ID
            dataGridViewDetallePedido.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreProducto", // 🆕 Nueva propiedad
                HeaderText = "Producto",
                Name = "NombreProducto",
                Width = 200
            });

            dataGridViewDetallePedido.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cantidad",
                HeaderText = "Cantidad",
                Name = "Cantidad",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dataGridViewDetallePedido.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioUnitario",
                HeaderText = "Precio Unitario",
                Name = "PrecioUnitario",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Format = "C2",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            dataGridViewDetallePedido.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Subtotal",
                HeaderText = "Subtotal",
                Name = "Subtotal",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Format = "C2",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            // Configuraciones adicionales de UX
            dataGridViewDetallePedido.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDetallePedido.MultiSelect = false;
            dataGridViewDetallePedido.ReadOnly = true;
            dataGridViewDetallePedido.AllowUserToAddRows = false;
            dataGridViewDetallePedido.AllowUserToDeleteRows = false;
            dataGridViewDetallePedido.RowHeadersVisible = false;
            dataGridViewDetallePedido.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
        }

        /// <summary>
        /// Obtiene los detalles del pedido, los enriquece con datos de producto y los muestra en la grilla.
        /// </summary>
        private void CargarDetallesPedido()
        {
            if (_pedidoSeleccionado == null)
            {
                MessageBox.Show("No se pudo cargar el pedido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Mostrar los datos del pedido en los textboxes
            textBoxCliente.Text = _pedidoService.ObtenerNombreClientePorId(_pedidoSeleccionado.IdCliente);
            textBoxFecha.Text = _pedidoSeleccionado.FechaPedido.ToString("dd/MM/yyyy");

            // Obtener el nombre del estado del pedido usando el IdEstadoPedido
            string nombreEstado = _pedidoService.ObtenerNombreEstadoPorId(_pedidoSeleccionado.IdEstadoPedido);
            textBoxEstado.Text = nombreEstado;

            // Obtener detalles del pedido desde la base de datos
            List<DetallePedido> detalles = _detallePedidoService.ObtenerDetallesPorPedido(_pedidoSeleccionado.IdPedido);

            // 🎯 ENRIQUECER LOS DATOS CON NOMBRES DE PRODUCTOS
            var detallesEnriquecidos = detalles.Select(detalle => {
                Producto producto = _productoService.ObtenerProductoPorId(detalle.IdProducto);
                
                return new
                {
                    IdDetallePedido = detalle.IdDetallePedido,
                    IdPedido = detalle.IdPedido,
                    IdProducto = detalle.IdProducto,
                    NombreProducto = producto != null ? producto.Nombre : "Producto no encontrado", // 🆕 Nombre del producto
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario,
                    Subtotal = detalle.Subtotal
                };
            }).ToList();

            // Cargar la lista enriquecida en el DataGridView
            dataGridViewDetallePedido.DataSource = detallesEnriquecidos;
        }

        /// <summary>
        /// Cierra el formulario cuando se presiona el botón cerrar del encabezado.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Maneja el botón secundario de cierre del formulario.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
