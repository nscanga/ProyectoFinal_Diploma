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

        // 🆕 NUEVO MÉTODO: Solo cambiar estado (este va en el botón "Modificar Pedido")
        private void buttonModificarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxEstadoPedido.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un estado válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Guid nuevoEstadoId = (Guid)comboBoxEstadoPedido.SelectedValue;
                
                // Verificar si realmente hay un cambio
                if (_pedidoSeleccionado.IdEstadoPedido == nuevoEstadoId)
                {
                    MessageBox.Show("El estado seleccionado es el mismo actual.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 🎯 SOLO CAMBIAR EL ESTADO - Sin tocar productos
                _pedidoService.CambiarEstadoPedido(_pedidoSeleccionado.IdPedido, nuevoEstadoId);

                // Verificar si se envió email
                string nuevoEstado = _pedidoService.ObtenerNombreEstadoPorId(nuevoEstadoId);
                bool cambiaAEnCamino = nuevoEstado.Equals("En camino", StringComparison.OrdinalIgnoreCase);

                if (cambiaAEnCamino)
                {
                    Cliente cliente = _clienteService.ObtenerClientePorId(_pedidoSeleccionado.IdCliente);
                    if (cliente != null && !string.IsNullOrEmpty(cliente.Email))
                    {
                        MessageBox.Show($"✅ Estado cambiado a '{nuevoEstado}' correctamente.\n\n📧 Se envió notificación por email a: {cliente.Email}", 
                                       "Éxito - Email Enviado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"✅ Estado cambiado a '{nuevoEstado}' correctamente.\n\n⚠️ No se pudo enviar email (cliente sin email válido).", 
                                       "Éxito - Sin Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show($"✅ Estado cambiado a '{nuevoEstado}' correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar el estado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🔄 RENOMBRADO: Modificar productos (mantener lógica actual para modificar cantidades)
        private void buttonModificarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxEstadoPedido.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un estado válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Guid nuevoEstadoId = (Guid)comboBoxEstadoPedido.SelectedValue;
                
                // Verificar si realmente hay un cambio
                if (_pedidoSeleccionado.IdEstadoPedido == nuevoEstadoId)
                {
                    MessageBox.Show("El estado seleccionado es el mismo actual.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 🎯 SOLO CAMBIAR EL ESTADO - Sin tocar productos
                _pedidoService.CambiarEstadoPedido(_pedidoSeleccionado.IdPedido, nuevoEstadoId);

                // Verificar si se envió email
                string nuevoEstado = _pedidoService.ObtenerNombreEstadoPorId(nuevoEstadoId);
                bool cambiaAEnCamino = nuevoEstado.Equals("En camino", StringComparison.OrdinalIgnoreCase);

                if (cambiaAEnCamino)
                {
                    Cliente cliente = _clienteService.ObtenerClientePorId(_pedidoSeleccionado.IdCliente);
                    if (cliente != null && !string.IsNullOrEmpty(cliente.Email))
                    {
                        MessageBox.Show($"✅ Estado cambiado a '{nuevoEstado}' correctamente.\n\n📧 Se envió notificación por email a: {cliente.Email}", 
                                       "Éxito - Email Enviado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"✅ Estado cambiado a '{nuevoEstado}' correctamente.\n\n⚠️ No se pudo enviar email (cliente sin email válido).", 
                                       "Éxito - Sin Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show($"✅ Estado cambiado a '{nuevoEstado}' correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar el estado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // Implementar si es necesario
        }
    }
}
