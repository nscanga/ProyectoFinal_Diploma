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

namespace Distribuidora_los_amigos.Forms.Pedidos
{
    public partial class CrearPedidoForm : Form
    {
        private readonly PedidoService _pedidoService;
        private readonly ClienteService _clienteService;
        private readonly ProductoService _productoService;
        private readonly StockService _stockService;
        private List<DetallePedido> _detallePedidoList;
        private List<DetallePedidoDTO> _detallePedidoDTOList;

        /// <summary>
        /// Inicializa el formulario de creación de pedidos cargando servicios, catálogos y eventos.
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

            // 🆕 Suscribirse al evento Resize
            this.Resize += CrearPedidoForm_Resize;

            CargarClientes();
            CargarProductos();
            CargarEstadosPedido();
            ConfigurarDataGridViews();
        }

        /// <summary>
        /// Configura el comportamiento de los DataGridView utilizados en la selección y detalle de productos.
        /// </summary>
        private void ConfigurarDataGridViews()
        {
            // 🆕 CONFIGURAR SCROLL Y AJUSTE AUTOMÁTICO para DataGridView de productos
            dataGridViewProductos.ScrollBars = ScrollBars.Both; // Habilitar scroll horizontal y vertical
            dataGridViewProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; // Ajustar al contenido
            dataGridViewProductos.AllowUserToResizeColumns = true;
            dataGridViewProductos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right; // Anclaje para redimensionar

            // 🆕 CONFIGURAR SCROLL Y AJUSTE AUTOMÁTICO para DataGridView de detalles
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
            comboBoxSeleccionCliente.DataSource = _clienteService.ObtenerTodosClientes();
            comboBoxSeleccionCliente.DisplayMember = "Nombre";
            comboBoxSeleccionCliente.ValueMember = "IdCliente";
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
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Rellena el combo de estados con los valores disponibles en el servicio.
        /// </summary>
        private void CargarEstadosPedido()
        {
            comboBoxEstadoPedido.DataSource = _pedidoService.ObtenerEstadosPedido();
            comboBoxEstadoPedido.DisplayMember = "NombreEstado";
            comboBoxEstadoPedido.ValueMember = "IdEstadoPedido";
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

                if (numericUpDown1.Value <= 0)
                {
                    MessageBox.Show("Ingrese una cantidad válida mayor a 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int cantidad = (int)numericUpDown1.Value;

                Stock stockProducto = _stockService.ObtenerStockPorProducto(productoSeleccionado.IdProducto);

                if (stockProducto == null || stockProducto.Cantidad < cantidad)
                {
                    MessageBox.Show($"No hay suficiente stock para el producto: {productoSeleccionado.Nombre}.\nStock disponible: {stockProducto?.Cantidad ?? 0}", 
                                    "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var detalleExistente = _detallePedidoList.FirstOrDefault(d => d.IdProducto == productoSeleccionado.IdProducto);
                
                if (detalleExistente != null)
                {
                    int cantidadTotal = detalleExistente.Cantidad + cantidad;
                    
                    if (stockProducto.Cantidad < cantidadTotal)
                    {
                        MessageBox.Show($"No hay suficiente stock para agregar {cantidad} unidades más.\nStock disponible: {stockProducto.Cantidad}\nYa agregado al pedido: {detalleExistente.Cantidad}", 
                                        "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
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
                    dataGridViewDetallePedido.Columns["NombreProducto"].MinimumWidth = 150; // 🆕 Ancho mínimo
                }
                
                if (dataGridViewDetallePedido.Columns["Categoria"] != null)
                {
                    dataGridViewDetallePedido.Columns["Categoria"].HeaderText = "Categoría";
                    dataGridViewDetallePedido.Columns["Categoria"].DisplayIndex = 1;
                    dataGridViewDetallePedido.Columns["Categoria"].MinimumWidth = 100; // 🆕 Ancho mínimo
                }
                
                if (dataGridViewDetallePedido.Columns["Cantidad"] != null)
                {
                    dataGridViewDetallePedido.Columns["Cantidad"].HeaderText = "Cantidad";
                    dataGridViewDetallePedido.Columns["Cantidad"].DisplayIndex = 2;
                    dataGridViewDetallePedido.Columns["Cantidad"].MinimumWidth = 70; // 🆕 Ancho mínimo
                }
                
                if (dataGridViewDetallePedido.Columns["PrecioUnitario"] != null)
                {
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].HeaderText = "Precio Unitario";
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].DefaultCellStyle.Format = "C2";
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].DisplayIndex = 3;
                    dataGridViewDetallePedido.Columns["PrecioUnitario"].MinimumWidth = 100; // 🆕 Ancho mínimo
                }
                
                if (dataGridViewDetallePedido.Columns["Subtotal"] != null)
                {
                    dataGridViewDetallePedido.Columns["Subtotal"].HeaderText = "Subtotal";
                    dataGridViewDetallePedido.Columns["Subtotal"].DefaultCellStyle.Format = "C2";
                    dataGridViewDetallePedido.Columns["Subtotal"].DisplayIndex = 4;
                    dataGridViewDetallePedido.Columns["Subtotal"].MinimumWidth = 100; // 🆕 Ancho mínimo
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
                if (comboBoxSeleccionCliente.SelectedValue == null || _detallePedidoList.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar un cliente y agregar al menos un producto.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Guid idEstadoPedido = (Guid)comboBoxEstadoPedido.SelectedValue;

                Pedido nuevoPedido = new Pedido()
                {
                    IdPedido = Guid.NewGuid(),
                    IdCliente = (Guid)comboBoxSeleccionCliente.SelectedValue,
                    FechaPedido = dateTimePickerCrearPedido.Value,
                    IdEstadoPedido = (Guid)comboBoxEstadoPedido.SelectedValue,
                    Detalles = new List<DetallePedido>(_detallePedidoList),
                    Total = _detallePedidoList.Sum(d => d.Subtotal)
                };
                
                if (comboBoxEstadoPedido.SelectedValue == null)
                {
                    MessageBox.Show("El estado del pedido no ha sido seleccionado correctamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int cantidadTotal = _detallePedidoList.Sum(d => d.Cantidad);

                _pedidoService.CrearPedido(nuevoPedido, cantidadTotal);

                MessageBox.Show($"Pedido creado correctamente.\n\nTotal: {nuevoPedido.Total:C2}\nProductos: {_detallePedidoList.Count}\nCantidad total: {cantidadTotal}", 
                                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el pedido: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
