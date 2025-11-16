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
    public partial class CrearPedidoForm : Form, IIdiomaObserver
    {
        private readonly PedidoService _pedidoService;
        private readonly ClienteService _clienteService;
        private readonly ProductoService _productoService;
        private readonly StockService _stockService;
        private List<DetallePedido> _detallePedidoList;
        private List<DetallePedidoDTO> _detallePedidoDTOList;
        private List<DetallePedido> _detallesPedido;

        /// <summary>
        /// Inicializa el formulario de creación de pedido.
        /// </summary>
        public CrearPedidoForm()
        {
            InitializeComponent();
            _pedidoService = new PedidoService();
            _clienteService = new ClienteService();
            _productoService = new ProductoService();
            _stockService = new StockService();
            _detallePedidoList = new List<DetallePedido>();
            _detallePedidoDTOList = new List<DetallePedidoDTO>();
            _detallesPedido = new List<DetallePedido>();

            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);

            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);

            // Configurar ayuda F1
            this.KeyPreview = true;
            this.KeyDown += CrearPedidoForm_KeyDown;

            CargarClientes();
            CargarProductos();
            CargarEstadosPedido();
            
            // Configurar DataGridView con las columnas
            ConfigurarDataGridViewProductos();
            ConfigurarDataGridViewDetallePedido();
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        private void CrearPedidoForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaCrearPedido();
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
            // CONFIGURAR SCROLL Y AJUSTE AUTOMÁTICO para DataGridView de productos
            dataGridViewProductos.ScrollBars = ScrollBars.Both; // Habilitar scroll horizontal y vertical
            dataGridViewProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; // Ajustar al contenido
            dataGridViewProductos.AllowUserToResizeColumns = true;
            dataGridViewProductos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right; // Anclaje para redimensionar

            // CONFIGURAR SCROLL Y AJUSTE AUTOMÁTICO para DataGridView de detalles
            dataGridViewDetallePedido.ScrollBars = ScrollBars.Both; // Habilitar scroll horizontal y vertical
            dataGridViewDetallePedido.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; // Ajustar al contenido
            dataGridViewDetallePedido.AllowUserToResizeColumns = true;
            dataGridViewDetallePedido.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; // Anclaje completo
            dataGridViewDetallePedido.AutoGenerateColumns = true;

            // Configurar DataGridView de productos disponibles
            if (dataGridViewProductos.Columns.Count > 0)
            {
                if (dataGridViewProductos.Columns["IdProducto"] != null)
                    dataGridViewProductos.Columns["IdProducto"].Visible = false;

                if (dataGridViewProductos.Columns["Nombre"] != null)
                    dataGridViewProductos.Columns["Nombre"].HeaderText = "Producto";
                
                if (dataGridViewProductos.Columns["Categoria"] != null)
                    dataGridViewProductos.Columns["Categoria"].HeaderText = "Categoría";
                
                if (dataGridViewProductos.Columns["Precio"] != null)
                {
                    dataGridViewProductos.Columns["Precio"].HeaderText = "Precio";
                    dataGridViewProductos.Columns["Precio"].DefaultCellStyle.Format = "C2";
                }
                
                if (dataGridViewProductos.Columns["FechaIngreso"] != null)
                    dataGridViewProductos.Columns["FechaIngreso"].HeaderText = "Fecha Ingreso";
                
                if (dataGridViewProductos.Columns["Vencimiento"] != null)
                    dataGridViewProductos.Columns["Vencimiento"].HeaderText = "Vencimiento";
                
                if (dataGridViewProductos.Columns["Activo"] != null)
                    dataGridViewProductos.Columns["Activo"].Visible = false;
            }
        }

        /// <summary>
        /// Configura el comportamiento de los DataGridView utilizados en la selección y detalle de productos.
        /// </summary>
        private void ConfigurarDataGridViewProductos()
        {
            // Configurar columnas y propiedades específicas para el DataGridView de productos
            if (dataGridViewProductos.Columns.Count > 0)
            {
                // Ocultar ID del producto
                if (dataGridViewProductos.Columns["IdProducto"] != null)
                    dataGridViewProductos.Columns["IdProducto"].Visible = false;

                // Ajustar encabezados y anchos de columnas
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
                
                if (dataGridViewProductos.Columns["FechaIngreso"] != null)
                {
                    dataGridViewProductos.Columns["FechaIngreso"].HeaderText = "Fecha Ingreso";
                    dataGridViewProductos.Columns["FechaIngreso"].Width = 120;
                }
                
                if (dataGridViewProductos.Columns["Vencimiento"] != null)
                {
                    dataGridViewProductos.Columns["Vencimiento"].HeaderText = "Vencimiento";
                    dataGridViewProductos.Columns["Vencimiento"].Width = 120;
                }
                
                if (dataGridViewProductos.Columns["Activo"] != null)
                    dataGridViewProductos.Columns["Activo"].Visible = false;
            }
        }

        /// <summary>
        /// Configura el comportamiento de los DataGridView utilizados en la selección y detalle de productos.
        /// </summary>
        private void ConfigurarDataGridViewDetallePedido()
        {
            // Configurar columnas y propiedades específicas para el DataGridView de detalles de pedido
            if (dataGridViewDetallePedido.Columns.Count > 0)
            {
                // Ocultar IDs innecesarios
                if (dataGridViewDetallePedido.Columns["IdDetallePedido"] != null)
                    dataGridViewDetallePedido.Columns["IdDetallePedido"].Visible = false;
                
                if (dataGridViewDetallePedido.Columns["IdPedido"] != null)
                    dataGridViewDetallePedido.Columns["IdPedido"].Visible = false;
                
                if (dataGridViewDetallePedido.Columns["IdProducto"] != null)
                    dataGridViewDetallePedido.Columns["IdProducto"].Visible = false;

                // Configurar encabezados y anchos de columnas
                if (dataGridViewDetallePedido.Columns["NombreProducto"] != null)
                {
                    dataGridViewDetallePedido.Columns["NombreProducto"].HeaderText = "Producto";
                    dataGridViewDetallePedido.Columns["NombreProducto"].DisplayIndex = 0;
                    dataGridViewDetallePedido.Columns["NombreProducto"].MinimumWidth = 150; // Ancho mínimo
                }
                
                if (dataGridViewDetallePedido.Columns["Categoria"] != null)
                {
                    dataGridViewDetallePedido.Columns["Categoria"].HeaderText = "Categoría";
                    dataGridViewDetallePedido.Columns["Categoria"].DisplayIndex = 1;
                    dataGridViewDetallePedido.Columns["Categoria"].MinimumWidth = 100; // Ancho mínimo
                }
                
                if (dataGridViewDetallePedido.Columns["Cantidad"] != null)
                {
                    dataGridViewDetallePedido.Columns["Cantidad"].HeaderText = "Cantidad";
                    dataGridViewDetallePedido.Columns["Cantidad"].DisplayIndex = 2;
                    dataGridViewDetallePedido.Columns["Cantidad"].MinimumWidth = 70; // Ancho mínimo
                }
                
                if (dataGridViewDetallePedido.Columns["PrecioUnitario"] != null)
                {
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].HeaderText = "Precio Unitario";
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].DefaultCellStyle.Format = "C2";
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].DisplayIndex = 3;
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].MinimumWidth = 100; // Ancho mínimo
                }
                
                if (dataGridViewDetallePedido.Columns["Subtotal"] != null)
                {
                    dataGridViewDetallePedido.Columns["Subtotal"].HeaderText = "Subtotal";
                    dataGridViewDetallePedido.Columns["Subtotal"].DefaultCellStyle.Format = "C2";
                    dataGridViewDetallePedido.Columns["Subtotal"].DisplayIndex = 4;
                    dataGridViewDetallePedido.Columns["Subtotal"].MinimumWidth = 100; // Ancho mínimo
                }
            }
        }

        // 🆕 NUEVO MÉTODO: Manejar redimensionamiento del formulario
        /// <summary>
        /// Ajusta dinámicamente los tamaños de los grids al modificar el tamaño de la ventana.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void CrearPedidoForm_Resize(object sender, EventArgs e)
        {
            // Calcular nuevo ancho para los DataGridViews cuando se redimensiona la ventana
            int margenIzquierdo = 406;
            int margenDerecho = 20;
            int nuevoAncho = this.ClientSize.Width - margenIzquierdo - margenDerecho;

            if (nuevoAncho > 200) // Ancho mínimo
            {
                dataGridViewProductos.Width = nuevoAncho;
                dataGridViewDetallePedido.Width = nuevoAncho;
            }

            // Ajustar altura del DataGridView de detalles
            int alturaDisponible = this.ClientSize.Height - dataGridViewDetallePedido.Top - 30;
            if (alturaDisponible > 100)
            {
                dataGridViewDetallePedido.Height = alturaDisponible;
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
        /// Rellena el combo de estados con los valores disponibles en el servicio.
        /// </summary>
        private void CargarEstadosPedido()
        {
            try
            {
                List<EstadoPedido> estados = _pedidoService.ObtenerEstadosPedido();
                comboBoxEstadoPedido.DataSource = estados;
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
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonAgregarProducto_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewProductos.SelectedRows.Count > 0)
            {
                Producto productoSeleccionado = (Producto)dataGridViewProductos.SelectedRows[0].DataBoundItem;

                // ✅ Validación básica de UI (formato/entrada)
                if (numericUpDown1.Value <= 0)
                {
                    MessageBox.Show("Ingrese una cantidad válida mayor a 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int cantidad = (int)numericUpDown1.Value;

                // 🔍 Verificar stock disponible (validación de negocio)
                try
                {
                    Stock stockProducto = _stockService.ObtenerStockPorProducto(productoSeleccionado.IdProducto);

                    if (stockProducto == null)
                    {
                        MessageBox.Show($"No hay registro de stock para el producto: {productoSeleccionado.Nombre}", 
                                        "Stock no disponible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Calcular cantidad total incluyendo lo ya agregado
                    var detalleExistente = _detallePedidoList.FirstOrDefault(d => d.IdProducto == productoSeleccionado.IdProducto);
                    int cantidadTotal = detalleExistente != null ? detalleExistente.Cantidad + cantidad : cantidad;
                    
                    if (stockProducto.Cantidad < cantidadTotal)
                    {
                        MessageBox.Show($"No hay suficiente stock para el producto: {productoSeleccionado.Nombre}.\n" +
                                      $"Stock disponible: {stockProducto.Cantidad}\n" +
                                      $"Ya agregado: {detalleExistente?.Cantidad ?? 0}\n" +
                                      $"Solicitado: {cantidad}",
                                        "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Agregar o actualizar el detalle
                    if (detalleExistente != null)
                    {
                        detalleExistente.Cantidad = cantidadTotal;
                        detalleExistente.Subtotal = cantidadTotal * productoSeleccionado.Precio;
                        
                        var dtoExistente = _detallePedidoDTOList.FirstOrDefault(d => d.IdProducto == productoSeleccionado.IdProducto);
                        if (dtoExistente != null)
                        {
                            dtoExistente.Cantidad = cantidadTotal;
                            dtoExistente.Subtotal = cantidadTotal * productoSeleccionado.Precio;
                        }
                    }
                    else
                    {
                        DetallePedido nuevoDetalle = new DetallePedido()
                        {
                            IdDetallePedido = Guid.NewGuid(),
                            IdProducto = productoSeleccionado.IdProducto,
                            Cantidad = cantidad,
                            PrecioUnitario = productoSeleccionado.Precio,
                            Subtotal = cantidad * productoSeleccionado.Precio
                        };

                        DetallePedidoDTO nuevoDetalleDTO = new DetallePedidoDTO()
                        {
                            IdDetallePedido = nuevoDetalle.IdDetallePedido,
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
                MessageBox.Show("Seleccione un producto para agregar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    dataGridViewDetallePedido.Columns["NombreProducto"].MinimumWidth = 150; // Ancho mínimo
                }
                
                if (dataGridViewDetallePedido.Columns["Categoria"] != null)
                {
                    dataGridViewDetallePedido.Columns["Categoria"].HeaderText = "Categoría";
                    dataGridViewDetallePedido.Columns["Categoria"].DisplayIndex = 1;
                    dataGridViewDetallePedido.Columns["Categoria"].MinimumWidth = 100; // Ancho mínimo
                }
                
                if (dataGridViewDetallePedido.Columns["Cantidad"] != null)
                {
                    dataGridViewDetallePedido.Columns["Cantidad"].HeaderText = "Cantidad";
                    dataGridViewDetallePedido.Columns["Cantidad"].DisplayIndex = 2;
                    dataGridViewDetallePedido.Columns["Cantidad"].MinimumWidth = 70; // Ancho mínimo
                }
                
                if (dataGridViewDetallePedido.Columns["PrecioUnitario"] != null)
                {
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].HeaderText = "Precio Unitario";
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].DefaultCellStyle.Format = "C2";
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].DisplayIndex = 3;
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].MinimumWidth = 100; // Ancho mínimo
                }
                
                if (dataGridViewDetallePedido.Columns["Subtotal"] != null)
                {
                    dataGridViewDetallePedido.Columns["Subtotal"].HeaderText = "Subtotal";
                    dataGridViewDetallePedido.Columns["Subtotal"].DefaultCellStyle.Format = "C2";
                    dataGridViewDetallePedido.Columns["Subtotal"].DisplayIndex = 4;
                    dataGridViewDetallePedido.Columns["Subtotal"].MinimumWidth = 100; // Ancho mínimo
                }
            }
        }

        /// <summary>
        /// Calcula el total acumulado del pedido para depuración y totales.
        /// </summary>
        private void ActualizarTotalPedido()
        {
            decimal total = _detallePedidoList.Sum(d => d.Subtotal);
            Console.WriteLine($"Total del pedido: {total:C2}");
        }

        /// <summary>
        /// Valida la información ingresada y persiste el nuevo pedido con sus detalles.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonGuardarPedido_Click_1(object sender, EventArgs e)
        {
            try
            {
                // ✅ Validaciones básicas de UI (formato/entrada)
                if (comboBoxSeleccionCliente.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un cliente.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_detallePedidoList.Count == 0)
                {
                    MessageBox.Show("Debe agregar al menos un producto al pedido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (comboBoxEstadoPedido.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un estado para el pedido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 🎯 Construir el pedido
                Pedido nuevoPedido = new Pedido()
                {
                    IdPedido = Guid.NewGuid(),
                    IdCliente = (Guid)comboBoxSeleccionCliente.SelectedValue,
                    FechaPedido = dateTimePickerCrearPedido.Value,
                    IdEstadoPedido = (Guid)comboBoxEstadoPedido.SelectedValue,
                    Detalles = new List<DetallePedido>(_detallePedidoList),
                    Total = _detallePedidoList.Sum(d => d.Subtotal)
                };

                int cantidadTotal = _detallePedidoList.Sum(d => d.Cantidad);

                // 🚀 El BLL se encarga de TODAS las validaciones de negocio:
                // - Validar cliente existe
                // - Validar estado existe
                // - Validar fecha no futura
                // - Validar productos existen
                // - Validar cantidades > 0
                // - Validar stock suficiente
                // - Verificar y notificar stock bajo
                _pedidoService.CrearPedido(nuevoPedido, cantidadTotal);

                MessageBox.Show($"✅ Pedido creado correctamente.\n\n" +
                              $"Total: {nuevoPedido.Total:C2}\n" +
                              $"Productos: {_detallePedidoList.Count}\n" +
                              $"Cantidad total: {cantidadTotal}", 
                                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (PedidoException pedEx)
            {
                // 🎯 Excepciones de reglas de negocio de pedidos
                MessageBox.Show($"❌ {pedEx.Message}", "Error de Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoggerService.WriteException(pedEx);
            }
            catch (StockException stockEx)
            {
                // 🎯 Excepciones de stock insuficiente
                MessageBox.Show($"⚠️ {stockEx.Message}", "Stock Insuficiente", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoggerService.WriteException(stockEx);
            }
            catch (DatabaseException dbEx)
            {
                // 🎯 Errores de conexión/base de datos
                string username = ObtenerUsuarioActual();
                ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    MessageBox.Show(
                        "No se puede crear el pedido sin conexión a la base de datos.\n" +
                        "Por favor, verifique la conexión e intente nuevamente.",
                        "Error de Conexión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // 🎯 Errores inesperados
                ErrorHandler.HandleGeneralException(ex);
                MessageBox.Show($"Error inesperado al crear el pedido: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Placeholder para reaccionar a cambios del estado seleccionado del pedido.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void comboBoxEstadoPedido_SelectedIndexChanged(object sender, EventArgs e)
        {

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
