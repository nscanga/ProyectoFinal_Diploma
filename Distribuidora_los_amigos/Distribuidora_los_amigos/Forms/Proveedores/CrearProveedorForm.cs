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

namespace Distribuidora_los_amigos.Forms.Proveedores
{
    public partial class CrearProveedorForm : Form
    {
        private readonly ProveedorService _proveedorService;

        public CrearProveedorForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _proveedorService = new ProveedorService();
            this.KeyPreview = true;
        }

        private void buttonCrearProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                Proveedor nuevoProveedor = new Proveedor()
                {
                    IdProveedor = Guid.NewGuid(),
                    Nombre = textBoxNombreProveedor.Text.Trim(),
                    Direccion = textBoxMDireccionProveedor.Text.Trim(),
                    Email = textBoxEmailProveedor.Text.Trim(),
                    Telefono = textBoxTelefonoProveedor.Text.Trim(),
                    Categoria = comboBoxCategoriaProveedor.Text.Trim(),
                    Activo = checkBoxProveedor.Checked
                };

                _proveedorService.CrearProveedor(nuevoProveedor);

                MessageBox.Show("Proveedor creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el proveedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
