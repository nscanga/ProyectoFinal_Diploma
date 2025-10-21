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
                MessageBox.Show("Seleccione un cliente para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                DialogResult result = MessageBox.Show("¿Está seguro de eliminar este cliente?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _clienteService.EliminarCliente(clienteSeleccionado.IdCliente);
                    MessageBox.Show("Cliente eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarClientes();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un cliente para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
