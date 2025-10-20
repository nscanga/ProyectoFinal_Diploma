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
                // 🆕 VALIDACIONES ANTES DE CREAR
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("El nombre es obligatorio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox1.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    MessageBox.Show("El email es obligatorio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox3.Focus();
                    return;
                }

                // 🔧 VALIDAR QUE EMAIL NO SEA TELÉFONO
                if (System.Text.RegularExpressions.Regex.IsMatch(textBox3.Text.Trim(), @"^\d{8,15}$"))
                {
                    MessageBox.Show("Ha ingresado un número de teléfono en el campo Email.\nPor favor ingrese un email válido (ejemplo: nombre@empresa.com).", 
                                   "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox3.Focus();
                    textBox3.SelectAll();
                    return;
                }

                // 🔧 VALIDAR FORMATO DE EMAIL
                if (!textBox3.Text.Contains("@"))
                {
                    MessageBox.Show("El email debe contener '@'.\nFormato correcto: nombre@empresa.com", 
                                   "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox3.Focus();
                    return;
                }

                // 🔧 VALIDAR QUE TELÉFONO NO SEA EMAIL
                if (textBox4.Text.Contains("@"))
                {
                    MessageBox.Show("Ha ingresado un email en el campo Teléfono.\nPor favor ingrese solo números.", 
                                   "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox4.Focus();
                    textBox4.SelectAll();
                    return;
                }

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
