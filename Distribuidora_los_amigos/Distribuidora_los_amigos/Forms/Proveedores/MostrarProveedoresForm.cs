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
    public partial class MostrarProveedoresForm : Form
    {
        private readonly ProveedorService _proveedorService;

        public MostrarProveedoresForm()
        {
            InitializeComponent();
            _proveedorService = new ProveedorService();
            this.StartPosition = FormStartPosition.CenterScreen;
            CargarProveedores();
        }

        private void CargarProveedores()
        {
            try
            {
                List<Proveedor> proveedores = _proveedorService.ObtenerTodosProveedores();
                dataGridViewProveedores.DataSource = proveedores;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar proveedores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void buttonCrearProveedor_Click(object sender, EventArgs e)
        {
            CrearProveedorForm crearForm = new CrearProveedorForm();
            crearForm.ShowDialog();
            CargarProveedores();
        }

        private void buttonModificarProveedor_Click(object sender, EventArgs e)
        {
            if (dataGridViewProveedores.SelectedRows.Count > 0)
            {
                Proveedor proveedorSeleccionado = (Proveedor)dataGridViewProveedores.SelectedRows[0].DataBoundItem;
                ModificarProveedorForm modificarForm = new ModificarProveedorForm(proveedorSeleccionado);
                modificarForm.ShowDialog();
                CargarProveedores();
            }
            else
            {
                MessageBox.Show("Seleccione un proveedor para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonEliminarProveedor_Click(object sender, EventArgs e)
        {
            if (dataGridViewProveedores.SelectedRows.Count > 0)
            {
                Proveedor proveedorSeleccionado = (Proveedor)dataGridViewProveedores.SelectedRows[0].DataBoundItem;

                DialogResult result = MessageBox.Show(
                    $"¿Está seguro que desea eliminar al proveedor {proveedorSeleccionado.Nombre}?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _proveedorService.EliminarProveedor(proveedorSeleccionado.IdProveedor);
                        MessageBox.Show("Proveedor eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarProveedores();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el proveedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un proveedor para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonEliminarProveedor_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewProveedores.SelectedRows.Count > 0)
            {
                // Obtener el proveedor seleccionado
                Proveedor proveedorSeleccionado = (Proveedor)dataGridViewProveedores.SelectedRows[0].DataBoundItem;

                // Confirmación antes de eliminar
                DialogResult resultado = MessageBox.Show($"¿Está seguro de que desea eliminar al proveedor {proveedorSeleccionado.Nombre}?",
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    try
                    {
                        // Llamar al servicio para eliminar el proveedor
                        _proveedorService.EliminarProveedor(proveedorSeleccionado.IdProveedor);

                        // Refrescar la lista de proveedores después de eliminar
                        CargarProveedores();

                        MessageBox.Show("Proveedor eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el proveedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un proveedor para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
