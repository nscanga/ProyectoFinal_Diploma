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

namespace Distribuidora_los_amigos.Forms.Clientes
{
    public partial class ModificarClienteForm : Form
    {
        private readonly ClienteService _clienteService;
        private Cliente _cliente;

        /// <summary>
        /// Inicializa el formulario de edición cargando el cliente a modificar.
        /// </summary>
        /// <param name="cliente">Cliente que se editará.</param>
        public ModificarClienteForm(Cliente cliente)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _clienteService = new ClienteService();
            _cliente = cliente;

            // Cargar datos en los campos del formulario
            textBox1.Text = _cliente.Nombre;
            textBox2.Text = _cliente.Direccion;
            textBox3.Text = _cliente.Email;
            textBox4.Text = _cliente.Telefono;
            textBox5.Text = _cliente.CUIT;
            checkBox1.Checked = _cliente.Activo;

        }

        

        /// <summary>
        /// Guarda los cambios realizados sobre el cliente en edición.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonGuardarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                _cliente.Nombre = textBox1.Text.Trim();
                _cliente.Direccion = textBox2.Text.Trim();
                _cliente.Email = textBox3.Text.Trim();
                _cliente.Telefono = textBox4.Text.Trim();
                _cliente.CUIT = textBox5.Text.Trim();
                _cliente.Activo = checkBox1.Checked.Equals(true);


                _clienteService.ModificarCliente(_cliente);
                MessageBox.Show("Cliente modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el cliente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
