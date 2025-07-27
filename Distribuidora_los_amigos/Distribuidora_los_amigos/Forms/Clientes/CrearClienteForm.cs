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
    public partial class CrearClienteForm : Form
    {

        private readonly ClienteService _clienteService;


        public CrearClienteForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _clienteService = new ClienteService();
        }

        private void buttonGuardarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente cliente = new Cliente()
                {
                    IdCliente = Guid.NewGuid(),
                    Nombre = textBox1.Text.Trim(),
                    Direccion = textBox2.Text.Trim(),
                    Email = textBox3.Text.Trim(),
                    Telefono = textBox4.Text.Trim(),
                    CUIT = textBox5.Text.Trim(),
                    Activo = checkBox1.Checked
                };

                _clienteService.CrearCliente(cliente);
                MessageBox.Show("Cliente creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar los campos
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                checkBox1.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
