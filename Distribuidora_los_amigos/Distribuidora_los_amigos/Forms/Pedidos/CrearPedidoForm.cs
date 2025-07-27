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

        public CrearPedidoForm()
        {
            InitializeComponent();
            _pedidoService = new PedidoService();
            _clienteService = new ClienteService();
            _productoService = new ProductoService();
            _stockService = new StockService();
            _detallePedidoList = new List<DetallePedido>();

            CargarClientes();
            CargarProductos();
            CargarEstadosPedido(); 
        }

        private void CargarClientes()
        {
            comboBoxSeleccionCliente.DataSource = _clienteService.ObtenerTodosClientes();
            comboBoxSeleccionCliente.DisplayMember = "Nombre";
            comboBoxSeleccionCliente.ValueMember = "IdCliente";
        }


        private void CargarProductos()
        {
            try
            {
                List<Producto> listaProductos = _productoService.ObtenerTodosProductos();
                dataGridViewProductos.DataSource = listaProductos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEstadosPedido()
        {
            comboBoxEstadoPedido.DataSource = _pedidoService.ObtenerEstadosPedido();
            comboBoxEstadoPedido.DisplayMember = "NombreEstado"; // Se muestra el nombre
            comboBoxEstadoPedido.ValueMember = "IdEstadoPedido"; // Se usa el ID internamente
        }


        private void buttonAgregarProducto_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewProductos.SelectedRows.Count > 0)
            {
                Producto productoSeleccionado = (Producto)dataGridViewProductos.SelectedRows[0].DataBoundItem;

                if (!int.TryParse(numericUpDown1.Text, out int cantidad) || cantidad <= 0)
                {
                    MessageBox.Show("Ingrese una cantidad válida mayor a 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener el stock del producto seleccionado
                Stock stockProducto = _stockService.ObtenerStockPorProducto(productoSeleccionado.IdProducto);

                if (stockProducto == null || stockProducto.Cantidad < cantidad)
                {
                    MessageBox.Show($"No hay suficiente stock para el producto: {productoSeleccionado.Nombre}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                // Crear el detalle del pedido
                DetallePedido nuevoDetalle = new DetallePedido()
                {
                    IdDetallePedido = Guid.NewGuid(),
                    IdProducto = productoSeleccionado.IdProducto,
                    Cantidad = cantidad,
                    PrecioUnitario = productoSeleccionado.Precio,
                    Subtotal = cantidad * productoSeleccionado.Precio
                };

                // Agregar a la lista
                _detallePedidoList.Add(nuevoDetalle);
                ActualizarListaProductosPedido();
            }
            else
            {
                MessageBox.Show("Seleccione un producto para agregar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void ActualizarListaProductosPedido()
        {
            dataGridViewDetallePedido.DataSource = null;
            dataGridViewDetallePedido.DataSource = _detallePedidoList;
        }

        private void buttonGuardarPedido_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxSeleccionCliente.SelectedValue == null || _detallePedidoList.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar un cliente y agregar al menos un producto.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                //Obtener el estado seleccionado
                Guid idEstadoPedido = (Guid)comboBoxEstadoPedido.SelectedValue;


                // 📌 Crear el pedido con el ID del estado
                Pedido nuevoPedido = new Pedido()
                {
                    IdPedido = Guid.NewGuid(),
                    IdCliente = (Guid)comboBoxSeleccionCliente.SelectedValue, // Asociar el cliente seleccionado
                    FechaPedido = dateTimePickerCrearPedido.Value,
                    IdEstadoPedido = (Guid)comboBoxEstadoPedido.SelectedValue, // 📌 Verifica que no sea null
                    Detalles = new List<DetallePedido>(_detallePedidoList), // Asegurar que se asignan los detalles
                    Total = _detallePedidoList.Sum(d => d.Subtotal) // Calculamos el total antes de enviar

                };
                if (comboBoxEstadoPedido.SelectedValue == null)
                {
                    MessageBox.Show("El estado del pedido no ha sido seleccionado correctamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }



                int cantidadTotal = _detallePedidoList.Sum(d => d.Cantidad);

                // Guardar pedido en la base de datos
                _pedidoService.CrearPedido(nuevoPedido, cantidadTotal);

                MessageBox.Show("Pedido creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el pedido: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
