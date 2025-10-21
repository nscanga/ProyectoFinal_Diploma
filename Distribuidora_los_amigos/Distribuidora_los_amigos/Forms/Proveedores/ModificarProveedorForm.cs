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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Distribuidora_los_amigos.Forms.Proveedores
{
    public partial class ModificarProveedorForm : Form
    {

        private readonly ProveedorService _proveedorService;
        private Proveedor _proveedor;
        /// <summary>
        /// Inicializa el formulario de modificación sin datos precargados.
        /// </summary>
        public ModificarProveedorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Inicializa el formulario cargando la información del proveedor a editar.
        /// </summary>
        /// <param name="proveedor">Proveedor seleccionado para modificar.</param>
        public ModificarProveedorForm(Proveedor proveedor)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _proveedorService = new ProveedorService();
            _proveedor = proveedor;

            // Cargar los datos en los campos del formulario
            textBox1.Text = _proveedor.Nombre;
            textBox2.Text = _proveedor.Direccion;
            textBox3.Text = _proveedor.Email;
            textBox4.Text = _proveedor.Telefono;
            comboBox1.Text = _proveedor.Categoria;
            checkBox1.Checked = _proveedor.Activo;
        }

        /// <summary>
        /// Guarda las modificaciones realizadas sobre el proveedor actual.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonGuardarProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                // Actualizar los datos del proveedor con los valores ingresados
                _proveedor.Nombre = textBox1.Text.Trim();
                _proveedor.Direccion = textBox2.Text.Trim();
                _proveedor.Email = textBox3.Text.Trim();
                _proveedor.Telefono = textBox4.Text.Trim();
                _proveedor.Categoria = comboBox1.Text.Trim();
                _proveedor.Activo = checkBox1.Checked;

                // Guardar los cambios en la base de datos
                _proveedorService.ModificarProveedor(_proveedor);

                MessageBox.Show("Proveedor modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el proveedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
