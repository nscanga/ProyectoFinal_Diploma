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

namespace Distribuidora_los_amigos.Forms.Pedidos
{
    public partial class ModificarPedidoForm : Form, IIdiomaObserver
    {
        private readonly PedidoService _pedidoService;
        private readonly ClienteService _clienteService;
        private readonly StockService _stockService;
        private readonly ProductoService _productoService;
        private Pedido _pedidoSeleccionado;
        private List<DetallePedido> _detallePedidoList;
        private List<DetallePedidoDTO> _detallePedidoDTOList;
        private List<DetallePedido> _detallesOriginales; // Para restaurar stock si se cancela

        /// <summary>
        /// Inicializa el formulario de modificación cargando el pedido y sus dependencias.
        /// </summary>
        /// <param name="pedido">Pedido que se desea modificar.</param>
        public ModificarPedidoForm(Pedido pedido)
        {
            InitializeComponent();
            _pedidoService = new PedidoService();
            _clienteService = new ClienteService();
            _stockService = new StockService();
            _productoService = new ProductoService();
            _pedidoSeleccionado = pedido;
            _detallePedidoList = new List<DetallePedido>();
            _detallePedidoDTOList = new List<DetallePedidoDTO>();

            // Cargar los detalles desde la base de datos en caso de que no vengan en el objeto
            if (_pedidoSeleccionado.Detalles == null || _pedidoSeleccionado.Detalles.Count == 0)
            {
                _pedidoSeleccionado.Detalles = _pedidoService.ObtenerDetallesPorPedido(_pedidoSeleccionado.IdPedido);
                Console.WriteLine($"🛑 Cargando detalles del pedido {_pedidoSeleccionado.IdPedido}: {_pedidoSeleccionado.Detalles.Count} encontrados.");
            }

            // Copiar detalles originales
            _detallesOriginales = new List<DetallePedido>();
            foreach (var detalle in _pedidoSeleccionado.Detalles)
            {
                _detallesOriginales.Add(new DetallePedido
                {
                    IdDetallePedido = detalle.IdDetallePedido,
                    IdPedido = detalle.IdPedido,
                    IdProducto = detalle.IdProducto,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = detalle.PrecioUnitario,
                    Subtotal = detalle.Subtotal
                });
            }

            this.StartPosition = FormStartPosition.CenterScreen;
            
            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);
            
            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);
            
            // Configurar ayuda F1
            this.KeyPreview = true;
            this.KeyDown += ModificarPedidoForm_KeyDown;

            // Inicializar controles
            CargarEstadosPedido();
            CargarClientes();
            CargarProductos();
            CargarPedido();
            ConfigurarDataGridViews();
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        private void ModificarPedidoForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaModificarPedido();
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
        /// Configura el comportamiento de los DataGridView utilizados en la selección y detalle de productos.
        /// </summary>
        private void ConfigurarDataGridViews()
        {
            // Configurar DataGridView de productos disponibles
            dataGridViewProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewProductos.MultiSelect = false;
            dataGridViewProductos.ReadOnly = true;
            dataGridViewProductos.AllowUserToAddRows = false;
            dataGridViewProductos.AllowUserToDeleteRows = false;

            // Configurar DataGridView de detalles del pedido
            dataGridViewDetallePedido.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDetallePedido.MultiSelect = false;
            dataGridViewDetallePedido.ReadOnly = true;
            dataGridViewDetallePedido.AllowUserToAddRows = false;
            
            // ⭐ Suscribirse al evento CellDoubleClick para editar cantidades con doble clic
            dataGridViewDetallePedido.CellDoubleClick += DataGridViewDetallePedido_CellDoubleClick;

            if (dataGridViewProductos.Columns.Count > 0)
            {
                // 🔒 Ocultar todos los IDs
                if (dataGridViewProductos.Columns["IdProducto"] != null)
                    dataGridViewProductos.Columns["IdProducto"].Visible = false;
                
                if (dataGridViewProductos.Columns["Id"] != null)
                    dataGridViewProductos.Columns["Id"].Visible = false;

                if (dataGridViewProductos.Columns["Nombre"] != null)
                {
                    dataGridViewProductos.Columns["Nombre"].HeaderText = "Producto";
                    dataGridViewProductos.Columns["Nombre"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                
                if (dataGridViewProductos.Columns["Categoria"] != null)
                {
                    dataGridViewProductos.Columns["Categoria"].HeaderText = "Categoría";
                    dataGridViewProductos.Columns["Categoria"].Width = 120;
                }
                
                if (dataGridViewProductos.Columns["Precio"] != null)
                {
                    dataGridViewProductos.Columns["Precio"].HeaderText = "Precio";
                    dataGridViewProductos.Columns["Precio"].DefaultCellStyle.Format = "C2";
                    dataGridViewProductos.Columns["Precio"].Width = 100;
                }
                
                if (dataGridViewProductos.Columns["Activo"] != null)
                    dataGridViewProductos.Columns["Activo"].Visible = false;
                
                if (dataGridViewProductos.Columns["FechaIngreso"] != null)
                    dataGridViewProductos.Columns["FechaIngreso"].Visible = false;
                
                if (dataGridViewProductos.Columns["Vencimiento"] != null)
                    dataGridViewProductos.Columns["Vencimiento"].Visible = false;
            }
        }

        /// <summary>
        /// Permite editar la cantidad con doble clic en la grilla de detalles.
        /// </summary>
        private void DataGridViewDetallePedido_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que se hizo doble clic en una fila válida y en la columna de cantidad
            if (e.RowIndex >= 0 && dataGridViewDetallePedido.Columns[e.ColumnIndex].Name == "Cantidad")
            {
                ModificarCantidadProductoSeleccionado();
            }
        }

        /// <summary>
        /// Modifica la cantidad del producto seleccionado en el detalle del pedido.
        /// </summary>
        private void buttonModificarCantidad_Click(object sender, EventArgs e)
        {
            ModificarCantidadProductoSeleccionado();
        }

        /// <summary>
        /// Lógica centralizada para modificar la cantidad de un producto en el pedido.
        /// </summary>
        private void ModificarCantidadProductoSeleccionado()
        {
            if (dataGridViewDetallePedido.SelectedRows.Count > 0)
            {
                try
                {
                    DetallePedidoDTO detalleDTO = (DetallePedidoDTO)dataGridViewDetallePedido.SelectedRows[0].DataBoundItem;
                    
                    // Crear un formulario de diálogo para pedir la nueva cantidad
                    using (Form formularioCantidad = new Form())
                    {
                        formularioCantidad.Text = "Modificar Cantidad";
                        formularioCantidad.Size = new Size(350, 180);
                        formularioCantidad.FormBorderStyle = FormBorderStyle.FixedDialog;
                        formularioCantidad.StartPosition = FormStartPosition.CenterParent;
                        formularioCantidad.MaximizeBox = false;
                        formularioCantidad.MinimizeBox = false;

                        Label labelProducto = new Label()
                        {
                            Text = $"Producto: {detalleDTO.NombreProducto}",
                            Location = new Point(20, 20),
                            Size = new Size(300, 20),
                            Font = new Font(formularioCantidad.Font, FontStyle.Bold)
                        };

                        Label labelCantidadActual = new Label()
                        {
                            Text = $"Cantidad actual: {detalleDTO.Cantidad}",
                            Location = new Point(20, 50),
                            Size = new Size(300, 20)
                        };

                        Label labelNuevaCantidad = new Label()
                        {
                            Text = "Nueva cantidad:",
                            Location = new Point(20, 80),
                            Size = new Size(100, 20)
                        };

                        NumericUpDown numericCantidad = new NumericUpDown()
                        {
                            Location = new Point(130, 78),
                            Size = new Size(100, 20),
                            Minimum = 1,
                            Maximum = 1000000,
                            Value = detalleDTO.Cantidad
                        };

                        Button buttonAceptar = new Button()
                        {
                            Text = "Aceptar",
                            Location = new Point(130, 110),
                            Size = new Size(80, 25),
                            DialogResult = DialogResult.OK
                        };

                        Button buttonCancelar = new Button()
                        {
                            Text = "Cancelar",
                            Location = new Point(220, 110),
                            Size = new Size(80, 25),
                            DialogResult = DialogResult.Cancel
                        };

                        formularioCantidad.Controls.AddRange(new Control[] 
                        { 
                            labelProducto, 
                            labelCantidadActual, 
                            labelNuevaCantidad, 
                            numericCantidad, 
                            buttonAceptar, 
                            buttonCancelar 
                        });

                        formularioCantidad.AcceptButton = buttonAceptar;
                        formularioCantidad.CancelButton = buttonCancelar;

                        if (formularioCantidad.ShowDialog() == DialogResult.OK)
                        {
                            int nuevaCantidad = (int)numericCantidad.Value;

                            if (nuevaCantidad == detalleDTO.Cantidad)
                            {
                                MessageBox.Show("La cantidad no ha cambiado.", "Información", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            // Validar stock disponible
                            Stock stockProducto = _stockService.ObtenerStockPorProducto(detalleDTO.IdProducto);

                            if (stockProducto == null)
                            {
                                MessageBox.Show($"No hay registro de stock para el producto: {detalleDTO.NombreProducto}", 
                                    "Stock no disponible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            // Calcular stock considerando la cantidad original del pedido
                            var detalleOriginal = _detallesOriginales.FirstOrDefault(d => d.IdProducto == detalleDTO.IdProducto);
                            int cantidadOriginal = detalleOriginal?.Cantidad ?? 0;
                            
                            // Stock disponible = stock actual + cantidad original del pedido
                            int stockDisponible = stockProducto.Cantidad + cantidadOriginal;

                            if (stockDisponible < nuevaCantidad)
                            {
                                MessageBox.Show($"No hay suficiente stock para el producto: {detalleDTO.NombreProducto}.\n" +
                                              $"Stock disponible: {stockDisponible}\n" +
                                              $"Cantidad solicitada: {nuevaCantidad}",
                                                "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            // Actualizar cantidad en ambas listas
                            var detalle = _detallePedidoList.FirstOrDefault(d => d.IdDetallePedido == detalleDTO.IdDetallePedido);
                            if (detalle != null)
                            {
                                detalle.Cantidad = nuevaCantidad;
                                detalle.Subtotal = nuevaCantidad * detalle.PrecioUnitario;

                                detalleDTO.Cantidad = nuevaCantidad;
                                detalleDTO.Subtotal = nuevaCantidad * detalleDTO.PrecioUnitario;

                                ActualizarListaProductosPedido();

                                MessageBox.Show($"Cantidad actualizada correctamente.\n" +
                                              $"Producto: {detalleDTO.NombreProducto}\n" +
                                              $"Nueva cantidad: {nuevaCantidad}", 
                                              "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (DatabaseException dbEx)
                {
                    ErrorHandler.HandleDatabaseException(dbEx, ObtenerUsuarioActual(), showMessageBox: true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al modificar cantidad: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoggerService.WriteException(ex);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto del detalle para modificar su cantidad.", "Advertencia", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Carga la lista de productos disponibles desde el servicio y la muestra en la grilla.
        /// </summary>
        private void CargarProductos()
        {
            try
            {
                List<Producto> listaProductos = _productoService.ObtenerTodosProductos();
                dataGridViewProductos.DataSource = listaProductos;
                ConfigurarDataGridViews();
            }
            catch (DatabaseException dbEx)
            {
                ErrorHandler.HandleDatabaseException(dbEx, ObtenerUsuarioActual(), showMessageBox: true);
                dataGridViewProductos.DataSource = new List<Producto>();
                dataGridViewProductos.Enabled = false;
                buttonAgregarProducto.Enabled = false;
                Console.WriteLine("❌ Error de conexión al cargar productos");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralException(ex);
                dataGridViewProductos.DataSource = new List<Producto>();
            }
        }

        /// <summary>
        /// Obtiene los clientes y los asigna al combo de selección.
        /// </summary>
        private void CargarClientes()
        {
            try
            {
                List<Cliente> clientes = _clienteService.ObtenerTodosClientes();
                comboBoxSeleccionCliente.DataSource = clientes;
                comboBoxSeleccionCliente.DisplayMember = "Nombre";
                comboBoxSeleccionCliente.ValueMember = "IdCliente";
            }
            catch (DatabaseException dbEx)
            {
                ErrorHandler.HandleDatabaseException(dbEx, ObtenerUsuarioActual(), showMessageBox: true);
                comboBoxSeleccionCliente.Enabled = false;
                Console.WriteLine("❌ Error de conexión al cargar clientes");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralException(ex);
                comboBoxSeleccionCliente.Enabled = false;
            }
        }

        /// <summary>
        /// Completa los controles del formulario con los datos del pedido seleccionado.
        /// </summary>
        private void CargarPedido()
        {
            textBoxIdPedido.Text = _pedidoSeleccionado.IdPedido.ToString();
            dateTimePickerCrearPedido.Value = _pedidoSeleccionado.FechaPedido;

            // Seleccionar el estado correcto en el ComboBox
            comboBoxEstadoPedido.SelectedValue = _pedidoSeleccionado.IdEstadoPedido;

            // Seleccionar el cliente correcto
            comboBoxSeleccionCliente.SelectedValue = _pedidoSeleccionado.IdCliente;

            // Cargar detalles del pedido
            foreach (var detalle in _pedidoSeleccionado.Detalles)
            {
                var producto = _productoService.ObtenerProductoPorId(detalle.IdProducto);
                if (producto != null)
                {
                    var nuevoDetalle = new DetallePedido
                    {
                        IdDetallePedido = detalle.IdDetallePedido,
                        IdPedido = detalle.IdPedido,
                        IdProducto = detalle.IdProducto,
                        Cantidad = detalle.Cantidad,
                        PrecioUnitario = detalle.PrecioUnitario,
                        Subtotal = detalle.Subtotal
                    };

                    var nuevoDetalleDTO = new DetallePedidoDTO
                    {
                        IdDetallePedido = detalle.IdDetallePedido,
                        IdPedido = detalle.IdPedido,
                        IdProducto = detalle.IdProducto,
                        NombreProducto = producto.Nombre,
                        Categoria = producto.Categoria,
                        Cantidad = detalle.Cantidad,
                        PrecioUnitario = detalle.PrecioUnitario,
                        Subtotal = detalle.Subtotal
                    };

                    _detallePedidoList.Add(nuevoDetalle);
                    _detallePedidoDTOList.Add(nuevoDetalleDTO);
                }
            }

            ActualizarListaProductosPedido();
        }

        /// <summary>
        /// Carga el catálogo de estados disponibles para asignar al pedido.
        /// </summary>
        private void CargarEstadosPedido()
        {
            try
            {
                comboBoxEstadoPedido.DataSource = _pedidoService.ObtenerEstadosPedido();
                comboBoxEstadoPedido.DisplayMember = "NombreEstado";
                comboBoxEstadoPedido.ValueMember = "IdEstadoPedido";
            }
            catch (DatabaseException dbEx)
            {
                ErrorHandler.HandleDatabaseException(dbEx, ObtenerUsuarioActual(), showMessageBox: true);
                Console.WriteLine("⚠️ Error de conexión al cargar estados, usando valores por defecto");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralException(ex);
            }
        }

        /// <summary>
        /// Agrega el producto seleccionado al detalle del pedido validando stock y cantidades.
        /// </summary>
        private void buttonAgregarProducto_Click(object sender, EventArgs e)
        {
            if (dataGridViewProductos.SelectedRows.Count > 0)
            {
                Producto productoSeleccionado = (Producto)dataGridViewProductos.SelectedRows[0].DataBoundItem;

                // ✅ Validación básica de UI (formato/entrada)
                if (numericUpDown1.Value <= 0)
                {
                    MessageBox.Show("Ingrese una cantidad válida mayor a 0.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int cantidad = (int)numericUpDown1.Value;

                try
                {
                    // 🔍 Verificar stock disponible
                    Stock stockProducto = _stockService.ObtenerStockPorProducto(productoSeleccionado.IdProducto);

                    if (stockProducto == null)
                    {
                        MessageBox.Show($"No hay registro de stock para el producto: {productoSeleccionado.Nombre}", 
                            "Stock no disponible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Calcular stock considerando la cantidad original del pedido
                    var detalleExistente = _detallePedidoList.FirstOrDefault(d => d.IdProducto == productoSeleccionado.IdProducto);
                    var detalleOriginal = _detallesOriginales.FirstOrDefault(d => d.IdProducto == productoSeleccionado.IdProducto);
                    
                    int cantidadOriginal = detalleOriginal?.Cantidad ?? 0;
                    int cantidadActual = detalleExistente?.Cantidad ?? 0;
                    int nuevaCantidadTotal = detalleExistente != null ? cantidadActual + cantidad : cantidad;
                    
                    // Stock disponible = stock actual + cantidad original del pedido
                    int stockDisponible = stockProducto.Cantidad + cantidadOriginal;
                    
                    if (stockDisponible < nuevaCantidadTotal)
                    {
                        MessageBox.Show($"No hay suficiente stock para el producto: {productoSeleccionado.Nombre}.\n" +
                                      $"Stock disponible: {stockDisponible}\n" +
                                      $"Ya agregado al pedido: {cantidadActual}\n" +
                                      $"Cantidad a agregar: {cantidad}",
                                        "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Agregar o actualizar el detalle
                    if (detalleExistente != null)
                    {
                        detalleExistente.Cantidad = nuevaCantidadTotal;
                        detalleExistente.Subtotal = nuevaCantidadTotal * productoSeleccionado.Precio;
                        
                        var dtoExistente = _detallePedidoDTOList.FirstOrDefault(d => d.IdProducto == productoSeleccionado.IdProducto);
                        if (dtoExistente != null)
                        {
                            dtoExistente.Cantidad = nuevaCantidadTotal;
                            dtoExistente.Subtotal = nuevaCantidadTotal * productoSeleccionado.Precio;
                        }
                    }
                    else
                    {
                        DetallePedido nuevoDetalle = new DetallePedido()
                        {
                            IdDetallePedido = Guid.NewGuid(),
                            IdPedido = _pedidoSeleccionado.IdPedido,
                            IdProducto = productoSeleccionado.IdProducto,
                            Cantidad = cantidad,
                            PrecioUnitario = productoSeleccionado.Precio,
                            Subtotal = cantidad * productoSeleccionado.Precio
                        };

                        DetallePedidoDTO nuevoDetalleDTO = new DetallePedidoDTO()
                        {
                            IdDetallePedido = nuevoDetalle.IdDetallePedido,
                            IdPedido = nuevoDetalle.IdPedido,
                            IdProducto = nuevoDetalle.IdProducto,
                            NombreProducto = productoSeleccionado.Nombre,
                            Categoria = productoSeleccionado.Categoria,
                            Cantidad = nuevoDetalle.Cantidad,
                            PrecioUnitario = nuevoDetalle.PrecioUnitario,
                            Subtotal = nuevoDetalle.Subtotal
                        };

                        _detallePedidoList.Add(nuevoDetalle);
                        _detallePedidoDTOList.Add(nuevoDetalleDTO);
                    }

                    ActualizarListaProductosPedido();
                    numericUpDown1.Value = 1;
                }
                catch (DatabaseException dbEx)
                {
                    ErrorHandler.HandleDatabaseException(dbEx, ObtenerUsuarioActual(), showMessageBox: true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al agregar producto: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoggerService.WriteException(ex);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto para agregar.", "Advertencia", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Quita el producto seleccionado del detalle del pedido.
        /// </summary>
        private void buttonQuitarProducto_Click(object sender, EventArgs e)
        {
            if (dataGridViewDetallePedido.SelectedRows.Count > 0)
            {
                try
                {
                    DetallePedidoDTO detalleDTO = (DetallePedidoDTO)dataGridViewDetallePedido.SelectedRows[0].DataBoundItem;
                    
                    DialogResult confirmacion = MessageBox.Show(
                        $"¿Desea quitar '{detalleDTO.NombreProducto}' del pedido?",
                        "Confirmar",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmacion == DialogResult.Yes)
                    {
                        // Quitar de ambas listas
                        var detalleAQuitar = _detallePedidoList.FirstOrDefault(d => d.IdDetallePedido == detalleDTO.IdDetallePedido);
                        if (detalleAQuitar != null)
                        {
                            _detallePedidoList.Remove(detalleAQuitar);
                        }

                        _detallePedidoDTOList.Remove(detalleDTO);
                        ActualizarListaProductosPedido();

                        MessageBox.Show($"Producto '{detalleDTO.NombreProducto}' quitado del pedido.", 
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al quitar producto: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoggerService.WriteException(ex);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto del detalle para quitar.", "Advertencia", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Refresca la grilla de detalles y recalcula el total del pedido.
        /// </summary>
        private void ActualizarListaProductosPedido()
        {
            dataGridViewDetallePedido.DataSource = null;
            dataGridViewDetallePedido.DataSource = _detallePedidoDTOList;

            ConfigurarDataGridViewDetalles();
            ActualizarTotalPedido();
        }

        /// <summary>
        /// Ajusta la visibilidad y formato de las columnas en el detalle del pedido.
        /// </summary>
        private void ConfigurarDataGridViewDetalles()
        {
            if (dataGridViewDetallePedido.Columns.Count > 0)
            {
                // Ocultar IDs
                if (dataGridViewDetallePedido.Columns["IdDetallePedido"] != null)
                    dataGridViewDetallePedido.Columns["IdDetallePedido"].Visible = false;
                
                if (dataGridViewDetallePedido.Columns["IdPedido"] != null)
                    dataGridViewDetallePedido.Columns["IdPedido"].Visible = false;
                
                if (dataGridViewDetallePedido.Columns["IdProducto"] != null)
                    dataGridViewDetallePedido.Columns["IdProducto"].Visible = false;

                // Configurar encabezados
                if (dataGridViewDetallePedido.Columns["NombreProducto"] != null)
                {
                    dataGridViewDetallePedido.Columns["NombreProducto"].HeaderText = "Producto";
                    dataGridViewDetallePedido.Columns["NombreProducto"].DisplayIndex = 0;
                    dataGridViewDetallePedido.Columns["NombreProducto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                
                if (dataGridViewDetallePedido.Columns["Categoria"] != null)
                {
                    dataGridViewDetallePedido.Columns["Categoria"].HeaderText = "Categoría";
                    dataGridViewDetallePedido.Columns["Categoria"].DisplayIndex = 1;
                    dataGridViewDetallePedido.Columns["Categoria"].Width = 100;
                }
                
                if (dataGridViewDetallePedido.Columns["Cantidad"] != null)
                {
                    dataGridViewDetallePedido.Columns["Cantidad"].HeaderText = "Cantidad";
                    dataGridViewDetallePedido.Columns["Cantidad"].DisplayIndex = 2;
                    dataGridViewDetallePedido.Columns["Cantidad"].Width = 80;
                }
                
                if (dataGridViewDetallePedido.Columns["PrecioUnitario"] != null)
                {
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].HeaderText = "Precio Unit.";
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].DefaultCellStyle.Format = "C2";
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].DisplayIndex = 3;
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].Width = 100;
                }
                
                if (dataGridViewDetallePedido.Columns["Subtotal"] != null)
                {
                    dataGridViewDetallePedido.Columns["Subtotal"].HeaderText = "Subtotal";
                    dataGridViewDetallePedido.Columns["Subtotal"].DefaultCellStyle.Format = "C2";
                    dataGridViewDetallePedido.Columns["Subtotal"].DisplayIndex = 4;
                    dataGridViewDetallePedido.Columns["Subtotal"].Width = 100;
                }
            }
        }

        /// <summary>
        /// Calcula el total acumulado del pedido y actualiza el label.
        /// </summary>
        private void ActualizarTotalPedido()
        {
            decimal total = _detallePedidoList.Sum(d => d.Subtotal);
            labelTotalValor.Text = total.ToString("C2");
        }

        /// <summary>
        /// Guarda todos los cambios realizados al pedido.
        /// </summary>
        private void buttonGuardarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ Validaciones básicas de UI
                if (comboBoxSeleccionCliente.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un cliente.", "Advertencia", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_detallePedidoList.Count == 0)
                {
                    MessageBox.Show("El pedido debe tener al menos un producto.", "Advertencia", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (comboBoxEstadoPedido.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un estado para el pedido.", "Advertencia", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Construir el pedido modificado
                _pedidoSeleccionado.IdCliente = (Guid)comboBoxSeleccionCliente.SelectedValue;
                _pedidoSeleccionado.IdEstadoPedido = (Guid)comboBoxEstadoPedido.SelectedValue;
                _pedidoSeleccionado.Detalles = new List<DetallePedido>(_detallePedidoList);
                _pedidoSeleccionado.Total = _detallePedidoList.Sum(d => d.Subtotal);

                // 🚀 Llamar al servicio para modificar el pedido
                _pedidoService.ModificarPedido(_pedidoSeleccionado, _detallesOriginales);

                // Verificar si se cambió a "En camino" para notificar email
                string nuevoEstado = _pedidoService.ObtenerNombreEstadoPorId(_pedidoSeleccionado.IdEstadoPedido);
                bool cambiaAEnCamino = nuevoEstado.Equals("En camino", StringComparison.OrdinalIgnoreCase);

                if (cambiaAEnCamino)
                {
                    Cliente cliente = _clienteService.ObtenerClientePorId(_pedidoSeleccionado.IdCliente);
                    if (cliente != null && !string.IsNullOrEmpty(cliente.Email))
                    {
                        MessageBox.Show(
                            $"✅ Pedido modificado correctamente.\n\n" +
                            $"Total: {_pedidoSeleccionado.Total:C2}\n" +
                            $"Productos: {_detallePedidoList.Count}\n\n" +
                            $"📧 Se envió notificación por email a: {cliente.Email}", 
                            "Éxito - Email Enviado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            $"✅ Pedido modificado correctamente.\n\n" +
                            $"Total: {_pedidoSeleccionado.Total:C2}\n" +
                            $"Productos: {_detallePedidoList.Count}\n\n" +
                            $"⚠️ No se pudo enviar email (cliente sin email válido).", 
                            "Éxito - Sin Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(
                        $"✅ Pedido modificado correctamente.\n\n" +
                        $"Total: {_pedidoSeleccionado.Total:C2}\n" +
                        $"Productos: {_detallePedidoList.Count}", 
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (PedidoException pedEx)
            {
                MessageBox.Show($"❌ {pedEx.Message}", "Error de Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoggerService.WriteException(pedEx);
            }
            catch (StockException stockEx)
            {
                MessageBox.Show($"⚠️ {stockEx.Message}", "Stock Insuficiente", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoggerService.WriteException(stockEx);
            }
            catch (DatabaseException dbEx)
            {
                string username = ObtenerUsuarioActual();
                ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    MessageBox.Show(
                        "No se puede modificar el pedido sin conexión a la base de datos.\n" +
                        "Por favor, verifique la conexión e intente nuevamente.",
                        "Error de Conexión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralException(ex);
                MessageBox.Show($"Error inesperado al modificar el pedido: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Elimina el pedido y restituye el stock de los productos asociados.
        /// </summary>
        private void buttonEliminarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ Confirmación del usuario
                DialogResult confirmacion = MessageBox.Show(
                    "¿Está seguro de eliminar este pedido?\nSe devolverá el stock de todos los productos.",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmacion != DialogResult.Yes)
                    return;

                // 🚀 El BLL maneja la devolución de stock y eliminación
                _pedidoService.EliminarPedido(_pedidoSeleccionado.IdPedido);

                MessageBox.Show("✅ Pedido eliminado correctamente.\nEl stock ha sido devuelto.", 
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (PedidoException pedEx)
            {
                MessageBox.Show($"❌ {pedEx.Message}", "Error de Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoggerService.WriteException(pedEx);
            }
            catch (DatabaseException dbEx)
            {
                string username = ObtenerUsuarioActual();
                ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    MessageBox.Show(
                        "No se puede eliminar el pedido sin conexión a la base de datos.\n" +
                        "Por favor, verifique la conexión e intente nuevamente.",
                        "Error de Conexión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado al eliminar el pedido: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Obtiene el nombre del usuario actual de forma segura.
        /// </summary>
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
