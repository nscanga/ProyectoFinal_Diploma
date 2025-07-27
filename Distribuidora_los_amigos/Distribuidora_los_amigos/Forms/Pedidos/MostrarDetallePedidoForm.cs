using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BLL;
using DOMAIN;

namespace Distribuidora_los_amigos.Forms.Pedidos
{
    public partial class MostrarDetallePedidoForm : Form
    {
        private readonly PedidoService _pedidoService;
        private readonly DetallePedido _detallePedido;
        private Pedido _pedidoSeleccionado;
        private readonly DetallePedidoService _detallePedidoService;


        public MostrarDetallePedidoForm(Pedido pedido)
        {
            InitializeComponent();
            _pedidoService = new PedidoService();
            _detallePedidoService = new DetallePedidoService();
            _pedidoSeleccionado = pedido;
            CargarDetallesPedido();
        }

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

            // 📌 Obtener el nombre del estado del pedido usando el IdEstadoPedido
            string nombreEstado = _pedidoService.ObtenerNombreEstadoPorId(_pedidoSeleccionado.IdEstadoPedido);
            textBoxEstado.Text = nombreEstado;

            // Obtener detalles del pedido desde la base de datos
            List<DetallePedido> detalles = _detallePedidoService.ObtenerDetallesPorPedido(_pedidoSeleccionado.IdPedido);

            // Cargar la lista en el DataGridView
            dataGridViewDetallePedido.DataSource = detalles;
        }


        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCerrar_Click(object sender, EventArgs e)
        {

        }
    }
}
