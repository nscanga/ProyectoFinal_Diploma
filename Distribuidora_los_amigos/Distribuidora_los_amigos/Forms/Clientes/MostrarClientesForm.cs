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
using Distribuidora_los_amigos.Forms.Productos;
using DOMAIN;
using Service.Facade;
using Service.DAL.Contracts;
using Services.Facade;

namespace Distribuidora_los_amigos.Forms.Clientes
{
    public partial class MostrarClientesForm : Form, IIdiomaObserver
    {
        private readonly ClienteService _clienteService;

        /// <summary>
        /// Inicializa el listado de clientes, carga datos y suscribe el formulario al cambio de idioma.
        /// </summary>
        public MostrarClientesForm()
        {
            InitializeComponent();
            _clienteService = new ClienteService();
            CargarClientes();

            // Habilitar captura de teclas para F1
            this.KeyPreview = true;
            this.KeyDown += MostrarClientesForm_KeyDown;

            // ✅ Suscribirse a cambios de idioma
            IdiomaService.Subscribe(this);

            // ✅ Traducir al cargar
            IdiomaService.TranslateForm(this);
        }

        /// <summary>
        /// Reaplica la traducción del formulario cuando se modifica el idioma activo.
        /// </summary>
        public void UpdateIdioma()
        {
            IdiomaService.TranslateForm(this);
            this.Refresh();
        }

        /// <summary>
        /// Cancela la suscripción al servicio de idioma al cerrar el formulario.
        /// </summary>
        /// <param name="e">Argumentos del evento de cierre.</param>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            IdiomaService.Unsubscribe(this);
            base.OnFormClosed(e);
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        private void MostrarClientesForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaMostrarClientes();
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
        /// Obtiene los clientes desde el servicio y los muestra en la grilla.
        /// </summary>
        private void CargarClientes()
        {
            try
            {
                dataGridView1.DataSource = _clienteService.ObtenerTodosClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Abre el formulario para crear un cliente y recarga la grilla al finalizar.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            var formCrear = new CrearClienteForm();
            formCrear.MdiParent = this.MdiParent;
            formCrear.FormBorderStyle = FormBorderStyle.None;
            formCrear.Dock = DockStyle.Fill;
            formCrear.WindowState = FormWindowState.Maximized;
            formCrear.FormClosed += (s, args) => CargarClientes();
            formCrear.Show();
        }

        /// <summary>
        /// Abre el formulario de modificación para el cliente seleccionado, validando la selección.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Cliente clienteSeleccionado = (Cliente)dataGridView1.SelectedRows[0].DataBoundItem;
                // ✅ Eliminar código duplicado - solo una instancia
                ModificarClienteForm formModificar = new ModificarClienteForm(clienteSeleccionado);
                formModificar.MdiParent = this.MdiParent;
                formModificar.FormBorderStyle = FormBorderStyle.None;
                formModificar.Dock = DockStyle.Fill;
                formModificar.WindowState = FormWindowState.Maximized;
                formModificar.FormClosed += (s, args) => CargarClientes();
                formModificar.Show();
            }
            else
            {
                string message = IdiomaService.Translate("Seleccione un cliente para modificar.");
                string title = IdiomaService.Translate("Advertencia");
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Elimina el cliente seleccionado previa confirmación y actualiza la grilla.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Cliente clienteSeleccionado = (Cliente)dataGridView1.SelectedRows[0].DataBoundItem;
                string confirmMessage = IdiomaService.Translate("¿Está seguro de eliminar este cliente?");
                string confirmTitle = IdiomaService.Translate("Confirmar");
                DialogResult result = MessageBox.Show(confirmMessage, confirmTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _clienteService.EliminarCliente(clienteSeleccionado.IdCliente);
                    string successMessage = IdiomaService.Translate("Cliente eliminado correctamente.");
                    string successTitle = IdiomaService.Translate("Éxito");
                    MessageBox.Show(successMessage, successTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarClientes();
                }
            }
            else
            {
                string message = IdiomaService.Translate("Seleccione un cliente para eliminar.");
                string title = IdiomaService.Translate("Advertencia");
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
