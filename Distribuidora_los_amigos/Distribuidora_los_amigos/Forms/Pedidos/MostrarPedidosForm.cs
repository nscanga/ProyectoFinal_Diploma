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
    public partial class MostrarPedidosForm : Form
    {
        private readonly PedidoService _pedidoService;

        public MostrarPedidosForm()
        {
            InitializeComponent();
            _pedidoService = new PedidoService();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None; 
        }

        private void MostrarPedidosForm_Load_1(object sender, EventArgs e)
        {
            if (this.MdiParent != null)
            {
                this.WindowState = FormWindowState.Maximized; // 📌 Forzar a maximizar dentro del MDI
                this.Dock = DockStyle.Fill; // 📌 Hacer que ocupe todo el espacio disponible
            }
            CargarPedidos(); // 🔹 Cargar pedidos cuando se abre el formulario
        }

        private void CargarPedidos()
        {
            try
            {
                List<Pedido> listaPedidos = _pedidoService.ObtenerTodosLosPedidos();
                dataGridViewPedidos.DataSource = listaPedidos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los pedidos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCrearPedido_Click_1(object sender, EventArgs e)
        {
            CrearPedidoForm crearPedidoForm = new CrearPedidoForm();
            crearPedidoForm.ShowDialog();
            CargarPedidos(); // Recargar la lista de pedidos
        }

        //private void buttonCrearPedido_Click_1(object sender, EventArgs e)
        //{
        //    if (dataGridViewPedidos.SelectedRows.Count > 0)
        //    {
        //        Pedido pedidoSeleccionado = (Pedido)dataGridViewPedidos.SelectedRows[0].DataBoundItem;
        //        ModificarPedidoForm modificarPedidoForm = new ModificarPedidoForm(pedidoSeleccionado);
        //        modificarPedidoForm.ShowDialog();
        //        CargarPedidos();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Seleccione un pedido para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //}

        private void buttonEliminarPedido_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewPedidos.SelectedRows.Count > 0)
            {
                Pedido pedidoSeleccionado = (Pedido)dataGridViewPedidos.SelectedRows[0].DataBoundItem;

                DialogResult confirmacion = MessageBox.Show("¿Está seguro de eliminar este pedido?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion == DialogResult.Yes)
                {
                    _pedidoService.EliminarPedido(pedidoSeleccionado.IdPedido);
                    CargarPedidos();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un pedido para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonVerDetalle_Click(object sender, EventArgs e)
        {
            if (dataGridViewPedidos.SelectedRows.Count > 0)
            {
                Pedido pedidoSeleccionado = (Pedido)dataGridViewPedidos.SelectedRows[0].DataBoundItem;
                MostrarDetallePedidoForm detallePedidoForm = new MostrarDetallePedidoForm(pedidoSeleccionado);
                detallePedidoForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Seleccione un pedido para ver su detalle.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonModificarPedido_Click(object sender, EventArgs e)
        {
            if (dataGridViewPedidos.SelectedRows.Count > 0)
            {
                Pedido pedidoSeleccionado = (Pedido)dataGridViewPedidos.SelectedRows[0].DataBoundItem;
                ModificarPedidoForm modificarPedidoForm = new ModificarPedidoForm(pedidoSeleccionado);
                modificarPedidoForm.ShowDialog();
                CargarPedidos();
            }
            else
            {
                MessageBox.Show("Seleccione un pedido para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
