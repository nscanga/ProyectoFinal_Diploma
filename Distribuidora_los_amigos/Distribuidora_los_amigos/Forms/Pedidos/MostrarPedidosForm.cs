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

        // 🆕 Agregar un ComboBox para estados en el formulario principal
        private ComboBox comboBoxCambiarEstado;
        private Button buttonCambiarEstado;

        /// <summary>
        /// Inicializa el formulario de listado de pedidos configurando servicios y controles auxiliares.
        /// </summary>
        public MostrarPedidosForm()
        {
            InitializeComponent();
            _pedidoService = new PedidoService();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            
            // 🔧 SOLUCIÓN: Inicializar los controles que faltan
            InitializeCustomControls();
            
            // 🔍 DEBUGGING: Verificar que el botón existe
            Console.WriteLine($"Botón Modificar existe: {buttonModificarPedido != null}");
            Console.WriteLine($"Botón Modificar visible: {buttonModificarPedido?.Visible}");
            Console.WriteLine($"Botón Modificar posición: {buttonModificarPedido?.Location}");
            Console.WriteLine($"Total controles en formulario: {this.Controls.Count}");
        }

        // 🆕 Método para inicializar los controles personalizados
        /// <summary>
        /// Crea los controles dinámicos para cambiar el estado desde la grilla.
        /// </summary>
        private void InitializeCustomControls()
        {
            // Crear ComboBox para cambiar estado
            comboBoxCambiarEstado = new ComboBox()
            {
                Location = new Point(118, 420),
                Size = new Size(150, 23),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            
            // Crear Button para cambiar estado
            buttonCambiarEstado = new Button()
            {
                Location = new Point(274, 420),
                Size = new Size(100, 30),
                Text = "Cambiar Estado",
                UseVisualStyleBackColor = true,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            buttonCambiarEstado.Click += buttonCambiarEstado_Click;
            
            // Agregar los controles al formulario
            this.Controls.Add(comboBoxCambiarEstado);
            this.Controls.Add(buttonCambiarEstado);
        }

        /// <summary>
        /// Configura el formulario como MDI child y carga pedidos y estados disponibles.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void MostrarPedidosForm_Load_1(object sender, EventArgs e)
        {
            if (this.MdiParent != null)
            {
                this.WindowState = FormWindowState.Maximized;
                this.Dock = DockStyle.Fill;
            }

            ConfigurarDataGridView(); // 🆕 Configurar el grid antes de cargar datos
            CargarPedidos();
            CargarEstadosEnCombo(); // 🆕 Cargar estados al iniciar
        }

        /// <summary>
        /// Recupera los pedidos y los enriquece con información adicional para mostrarlos en la grilla.
        /// </summary>
        private void CargarPedidos()
        {
            try
            {
                List<Pedido> listaPedidos = _pedidoService.ObtenerTodosLosPedidos();
                
                // 🎯 SOLUCIÓN: Usar el método ObtenerNombreEstadoPorId que sí funciona
                var pedidosEnriquecidos = listaPedidos.Select(p => {
                    string nombreCliente = _pedidoService.ObtenerNombreClientePorId(p.IdCliente);
                    var detalles = _pedidoService.ObtenerDetallesPorPedido(p.IdPedido);
                    int cantidadItems = detalles != null ? detalles.Sum(d => d.Cantidad) : 0;
                    
                    // 🔧 ESTA ES LA LÍNEA CLAVE: usar el mismo método que en MostrarDetallePedidoForm
                    string nombreEstado = _pedidoService.ObtenerNombreEstadoPorId(p.IdEstadoPedido);
                    
                    return new
                    {
                        IdPedido = p.IdPedido,
                        FechaPedido = p.FechaPedido,
                        NombreCliente = nombreCliente ?? "Cliente no encontrado",
                        NombreEstado = nombreEstado ?? "Estado no definido", // 🎯 Ahora debería mostrar "Entregado"
                        Total = p.Total,
                        CantidadItems = cantidadItems,
                        PedidoOriginal = p // Mantener referencia al objeto original
                    };
                }).ToList();

                dataGridViewPedidos.DataSource = pedidosEnriquecidos;
                
                // 🆕 Actualizar contador de pedidos
                ActualizarContadorPedidos(listaPedidos.Count);
                
                // 🔍 Debug: Verificar si hay datos
                if (pedidosEnriquecidos.Count == 0)
                {
                    MessageBox.Show("No se encontraron pedidos en la base de datos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los pedidos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🆕 Método para mostrar información adicional
        /// <summary>
        /// Actualiza el indicador visual con la cantidad total de pedidos cargados.
        /// </summary>
        /// <param name="total">Número de pedidos encontrados.</param>
        private void ActualizarContadorPedidos(int total)
        {
            // Asumiendo que tienes un Label llamado lblContadorPedidos en el formulario
            if (this.Controls.Find("lblContadorPedidos", true).Length > 0)
            {
                Label lblContador = (Label)this.Controls.Find("lblContadorPedidos", true)[0];
                lblContador.Text = $"Total de pedidos: {total}";
            }
        }

        /// <summary>
        /// Abre el formulario de creación de pedidos y recarga la lista al finalizar.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
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

        /// <summary>
        /// Elimina el pedido seleccionado tras solicitar confirmación.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonEliminarPedido_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewPedidos.SelectedRows.Count > 0)
            {
                // Fix: Access the PedidoOriginal property from the anonymous type
                dynamic selectedItem = dataGridViewPedidos.SelectedRows[0].DataBoundItem;
                Pedido pedidoSeleccionado = selectedItem.PedidoOriginal;

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

        /// <summary>
        /// Abre el detalle del pedido seleccionado en un diálogo modal.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonVerDetalle_Click(object sender, EventArgs e)
        {
            if (dataGridViewPedidos.SelectedRows.Count > 0)
            {
                // 🔄 Adaptar para trabajar con el objeto enriquecido
                var filaSeleccionada = dataGridViewPedidos.SelectedRows[0].DataBoundItem;
                
                // Obtener el pedido original usando reflexión o casting
                Pedido pedidoSeleccionado;
                if (filaSeleccionada.GetType().GetProperty("PedidoOriginal") != null)
                {
                    pedidoSeleccionado = (Pedido)filaSeleccionada.GetType().GetProperty("PedidoOriginal").GetValue(filaSeleccionada);
                }
                else
                {
                    // Fallback: obtener por ID
                    Guid idPedido = (Guid)filaSeleccionada.GetType().GetProperty("IdPedido").GetValue(filaSeleccionada);
                    pedidoSeleccionado = _pedidoService.ObtenerPedidoPorId(idPedido);
                }
                
                MostrarDetallePedidoForm detallePedidoForm = new MostrarDetallePedidoForm(pedidoSeleccionado);
                detallePedidoForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Seleccione un pedido para ver su detalle.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Abre el formulario de modificación para el pedido seleccionado.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonModificarPedido_Click(object sender, EventArgs e)
        {
            if (dataGridViewPedidos.SelectedRows.Count > 0)
            {
                var filaSeleccionada = dataGridViewPedidos.SelectedRows[0].DataBoundItem;
                
                // Obtener el pedido original
                Pedido pedidoSeleccionado;
                if (filaSeleccionada.GetType().GetProperty("PedidoOriginal") != null)
                {
                    pedidoSeleccionado = (Pedido)filaSeleccionada.GetType().GetProperty("PedidoOriginal").GetValue(filaSeleccionada);
                }
                else
                {
                    Guid idPedido = (Guid)filaSeleccionada.GetType().GetProperty("IdPedido").GetValue(filaSeleccionada);
                    pedidoSeleccionado = _pedidoService.ObtenerPedidoPorId(idPedido);
                }
                
                ModificarPedidoForm modificarPedidoForm = new ModificarPedidoForm(pedidoSeleccionado);
                modificarPedidoForm.ShowDialog();
                CargarPedidos(); // Recargar después de modificar
            }
            else
            {
                MessageBox.Show("Seleccione un pedido para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Placeholder para actualizar manualmente el listado de pedidos.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonActualizarPedido_Click(object sender, EventArgs e)
        {

        }

        // 🆕 Método para configurar las columnas del DataGridView
        /// <summary>
        /// Define las columnas y estilos del grid principal de pedidos.
        /// </summary>
        private void ConfigurarDataGridView()
        {
            dataGridViewPedidos.AutoGenerateColumns = false;
            dataGridViewPedidos.Columns.Clear();

            // Fecha
            dataGridViewPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaPedido",
                HeaderText = "Fecha",
                Name = "FechaPedido",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            // Cliente
            dataGridViewPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreCliente", // ✅ Con DataPropertyName
                HeaderText = "Cliente",
                Name = "NombreCliente",
                Width = 150
            });

            // Estado - ESTA ES LA COLUMNA CLAVE
            dataGridViewPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreEstado", // ✅ Con DataPropertyName
                HeaderText = "Estado",
                Name = "NombreEstado",
                Width = 120
            });

            // Total
            dataGridViewPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Total",
                HeaderText = "Total",
                Name = "Total",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Format = "C2",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            // Items
            dataGridViewPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CantidadItems", // ✅ Con DataPropertyName
                HeaderText = "Items",
                Name = "CantidadItems",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            // Configuraciones adicionales
            dataGridViewPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPedidos.MultiSelect = false;
            dataGridViewPedidos.ReadOnly = true;
            dataGridViewPedidos.AllowUserToAddRows = false;
            dataGridViewPedidos.AllowUserToDeleteRows = false;
            dataGridViewPedidos.RowHeadersVisible = false;
            dataGridViewPedidos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dataGridViewPedidos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 122, 183);
        }

        // 🆕 Método para cambiar estado directamente
        /// <summary>
        /// Cambia el estado del pedido seleccionado validando la elección en el combo.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonCambiarEstado_Click(object sender, EventArgs e)
        {
            if (dataGridViewPedidos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un pedido para cambiar su estado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxCambiarEstado.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un estado válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Obtener pedido seleccionado
                var filaSeleccionada = dataGridViewPedidos.SelectedRows[0].DataBoundItem;
                Pedido pedidoSeleccionado = (Pedido)filaSeleccionada.GetType().GetProperty("PedidoOriginal").GetValue(filaSeleccionada);
                
                Guid nuevoEstadoId = (Guid)comboBoxCambiarEstado.SelectedValue;
                
                // Verificar si hay cambio
                if (pedidoSeleccionado.IdEstadoPedido == nuevoEstadoId)
                {
                    MessageBox.Show("El estado seleccionado es el mismo actual.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Confirmar cambio
                string nuevoEstado = _pedidoService.ObtenerNombreEstadoPorId(nuevoEstadoId);
                DialogResult confirmacion = MessageBox.Show(
                    $"¿Confirma cambiar el estado a '{nuevoEstado}'?", 
                    "Confirmar Cambio", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

                if (confirmacion == DialogResult.Yes)
                {
                    // Cambiar estado
                    _pedidoService.CambiarEstadoPedido(pedidoSeleccionado.IdPedido, nuevoEstadoId);

                    // Mostrar resultado
                    bool envioEmail = nuevoEstado.Equals("En camino", StringComparison.OrdinalIgnoreCase);
                    if (envioEmail)
                    {
                        MessageBox.Show($"✅ Estado cambiado a '{nuevoEstado}'.\n📧 Se envió notificación por email.", 
                                       "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"✅ Estado cambiado a '{nuevoEstado}' correctamente.", 
                                       "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Recargar grid
                    CargarPedidos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar estado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🆕 Cargar estados en el ComboBox
        /// <summary>
        /// Pobla el combo auxiliar con los estados disponibles para el cambio rápido.
        /// </summary>
        private void CargarEstadosEnCombo()
        {
            comboBoxCambiarEstado.DataSource = _pedidoService.ObtenerEstadosPedido();
            comboBoxCambiarEstado.DisplayMember = "NombreEstado";
            comboBoxCambiarEstado.ValueMember = "IdEstadoPedido";
            comboBoxCambiarEstado.SelectedIndex = -1; // Sin selección inicial
        }
    }
}
