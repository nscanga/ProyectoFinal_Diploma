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
    public partial class ModificarPedidoForm : Form, IIdiomaObserver
    {
        private readonly PedidoService _pedidoService;
        private readonly ClienteService _clienteService;
        private readonly StockService _stockService;
        private Pedido _pedidoSeleccionado;
        private ProductoService _productoService;

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

            // Cargar los detalles desde la base de datos en caso de que no vengan en el objeto
            if (_pedidoSeleccionado.Detalles == null || _pedidoSeleccionado.Detalles.Count == 0)
            {
                _pedidoSeleccionado.Detalles = _pedidoService.ObtenerDetallesPorPedido(_pedidoSeleccionado.IdPedido);
                Console.WriteLine($"🛑 Cargando detalles del pedido {_pedidoSeleccionado.IdPedido}: {_pedidoSeleccionado.Detalles.Count} encontrados.");
            }

            this.StartPosition = FormStartPosition.CenterScreen;
            
            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);
            
            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);
            
            // Configurar ayuda F1
            this.KeyPreview = true;
            this.KeyDown += ModificarPedidoForm_KeyDown;

            CargarEstadosPedido(); 
            CargarPedido();
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
        /// Completa los controles del formulario con los datos del pedido seleccionado.
        /// </summary>
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

        /// <summary>
        /// Carga el catálogo de estados disponibles para asignar al pedido.
        /// </summary>
        private void CargarEstadosPedido()
        {
            comboBoxEstadoPedido.DataSource = _pedidoService.ObtenerEstadosPedido();
            comboBoxEstadoPedido.DisplayMember = "NombreEstado"; // Se muestra el nombre
            comboBoxEstadoPedido.ValueMember = "IdEstadoPedido"; // Se usa el ID internamente
        }

        // 🆕 NUEVO MÉTODO: Solo cambiar estado (este va en el botón "Modificar Pedido")
        /// <summary>
        /// Cambia el estado del pedido notificando al cliente cuando corresponde.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
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
        /// <summary>
        /// Reutiliza la lógica de cambio de estado para el botón de modificar productos.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
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

        /// <summary>
        /// Elimina el pedido y restituye el stock de los productos asociados.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
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

        /// <summary>
        /// Placeholder para persistir cambios adicionales al pedido.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonGuardarPedido_Click(object sender, EventArgs e)
        {
            // Implementar si es necesario
        }
    }
}
