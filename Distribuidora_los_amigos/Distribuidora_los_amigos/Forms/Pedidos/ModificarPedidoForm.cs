using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DOMAIN;

namespace Distribuidora_los_amigos.Forms.Pedidos
{
    public partial class ModificarPedidoForm : Form
    {
        private readonly PedidoService _pedidoService;
        private readonly ClienteService _clienteService;
        private readonly StockService _stockService;
        private Pedido _pedidoSeleccionado;
        private ProductoService _productoService;

        public ModificarPedidoForm(Pedido pedido)
        {
            InitializeComponent();
            _pedidoService = new PedidoService();
            _clienteService = new ClienteService();
            _stockService = new StockService();
            _productoService = new ProductoService();
            _pedidoSeleccionado = pedido;

            // Cargar los detalles desde la base de datos en caso de que no vengan en el objeto
            if (_pedidoSeleccionado.Detalles == null || _pedidoSeleccionado.Detalles.Count == 0)
            {
                _pedidoSeleccionado.Detalles = _pedidoService.ObtenerDetallesPorPedido(_pedidoSeleccionado.IdPedido);
                Console.WriteLine($"🛑 Cargando detalles del pedido {_pedidoSeleccionado.IdPedido}: {_pedidoSeleccionado.Detalles.Count} encontrados.");
            }

            this.StartPosition = FormStartPosition.CenterScreen;
            CargarEstadosPedido(); 
            CargarPedido();
        }

        private void CargarPedido()
        {
            textBoxIdPedido.Text = _pedidoSeleccionado.IdPedido.ToString();
            dateTimePickerCrearPedido.Value = _pedidoSeleccionado.FechaPedido;

            // Seleccionar el estado correcto en el ComboBox
            comboBoxEstadoPedido.SelectedValue = _pedidoSeleccionado.IdEstadoPedido;

            // Cargar clientes en el comboBox y seleccionar el correcto
            comboBoxSeleccionCliente.DataSource = _clienteService.ObtenerTodosClientes();
            comboBoxSeleccionCliente.DisplayMember = "Nombre";
            comboBoxSeleccionCliente.ValueMember = "IdCliente";
            comboBoxSeleccionCliente.SelectedValue = _pedidoSeleccionado.IdCliente;

            // Obtener cantidad total del pedido
            numericUpDown1.Value = _pedidoSeleccionado.Detalles.Sum(d => d.Cantidad);
        }


        private void CargarEstadosPedido()
        {
            comboBoxEstadoPedido.DataSource = _pedidoService.ObtenerEstadosPedido();
            comboBoxEstadoPedido.DisplayMember = "NombreEstado"; // Se muestra el nombre
            comboBoxEstadoPedido.ValueMember = "IdEstadoPedido"; // Se usa el ID internamente
        }


        private void buttonModificarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                int nuevaCantidad = (int)numericUpDown1.Value;
                if (nuevaCantidad <= 0)
                {
                    MessageBox.Show("La cantidad debe ser mayor a 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 📌 1️⃣ Obtener el detalle del pedido
                DetallePedido detalleSeleccionado = _pedidoSeleccionado.Detalles.FirstOrDefault();

                if (detalleSeleccionado == null)
                {
                    MessageBox.Show("Error al obtener el detalle del pedido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 📌 2️⃣ Recuperar el stock del producto
                Stock stockProducto = _stockService.ObtenerStockPorProducto(detalleSeleccionado.IdProducto);
                if (stockProducto == null)
                {
                    MessageBox.Show("No se encontró stock para este producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 📌 3️⃣ Devolver la cantidad original al stock antes de modificar
                _stockService.AumentarStock(detalleSeleccionado.IdProducto, detalleSeleccionado.Cantidad);

                // 📌 4️⃣ Verificar si hay suficiente stock para la nueva cantidad
                if (stockProducto.Cantidad < nuevaCantidad)
                {
                    MessageBox.Show("No hay suficiente stock disponible para este cambio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 📌 5️⃣ Modificar la cantidad y recalcular el subtotal
                detalleSeleccionado.Cantidad = nuevaCantidad;
                detalleSeleccionado.Subtotal = nuevaCantidad * _productoService.ObtenerProductoPorId(detalleSeleccionado.IdProducto).Precio;

                // 📌 6️⃣ Descontar el stock con la nueva cantidad
                _stockService.DisminuirStock(detalleSeleccionado.IdProducto, nuevaCantidad);

                // 📌 7️⃣ ACTUALIZAR EL ESTADO DEL PEDIDO  
                if (comboBoxEstadoPedido.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un estado válido para el pedido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _pedidoSeleccionado.IdEstadoPedido = (Guid)comboBoxEstadoPedido.SelectedValue; // ✅ Guardar el nuevo estado

                // 📌 8️⃣ Guardar los cambios en el pedido (incluye los detalles)
                _pedidoSeleccionado.Total = _pedidoSeleccionado.Detalles.Sum(d => d.Subtotal);
                _pedidoService.ModificarPedido(_pedidoSeleccionado);

                MessageBox.Show("Pedido modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Cerrar el formulario y notificar que se ha realizado un cambio
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el producto en el pedido: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }








        private void buttonEliminarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                // Devolver stock antes de eliminar el pedido
                foreach (var detalle in _pedidoSeleccionado.Detalles)
                {
                    _stockService.AumentarStock(detalle.IdProducto, detalle.Cantidad);
                }

                // Eliminar pedido
                _pedidoService.EliminarPedido(_pedidoSeleccionado.IdPedido);

                MessageBox.Show("Pedido eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el pedido: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonGuardarPedido_Click(object sender, EventArgs e)
        {

        }

    }
}
