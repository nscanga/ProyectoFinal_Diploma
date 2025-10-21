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
using Service.DAL.Contracts;
using Service.Facade;

namespace Distribuidora_los_amigos.Forms.Proveedores
{
    public partial class MostrarProveedoresForm : Form, IIdiomaObserver
    {
        private readonly ProveedorService _proveedorService;

        /// <summary>
        /// Inicializa el listado de proveedores, carga datos y suscribe el formulario a cambios de idioma.
        /// </summary>
        public MostrarProveedoresForm()
        {
            InitializeComponent();
            _proveedorService = new ProveedorService();
            CargarProveedores();

            IdiomaService.Subscribe(this);
            IdiomaService.TranslateForm(this);
        }

        /// <summary>
        /// Recupera los proveedores del servicio y los muestra en la grilla.
        /// </summary>
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


        /// <summary>
        /// Abre el formulario de creación y actualiza la lista al cerrarlo.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonCrearProveedor_Click(object sender, EventArgs e)
        {
            CrearProveedorForm crearForm = new CrearProveedorForm();
            crearForm.ShowDialog();
            CargarProveedores();
        }

        /// <summary>
        /// Abre el formulario de modificación para el proveedor seleccionado.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
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

        /// <summary>
        /// Elimina el proveedor seleccionado tras solicitar confirmación.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
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

        /// <summary>
        /// Maneja la eliminación desde el botón adicional confirmando y recargando la lista.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
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

        /// <summary>
        /// Retraduce el formulario cuando se modifica el idioma.
        /// </summary>
        public void UpdateIdioma()
        {
            IdiomaService.TranslateForm(this);
            this.Refresh();
        }

        /// <summary>
        /// Cancela la suscripción al servicio de idioma al cerrar el formulario.
        /// </summary>
        /// <param name="e">Argumentos del evento.</param>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            IdiomaService.Unsubscribe(this);
            base.OnFormClosed(e);
        }
    }
}
