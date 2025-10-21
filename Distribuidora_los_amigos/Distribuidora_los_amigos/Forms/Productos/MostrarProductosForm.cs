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

namespace Distribuidora_los_amigos.Forms.Productos
{
    public partial class MostrarProductosForm : Form
    {
        private readonly ProductoService _productoService;
        private Producto    
            _producto;

        /// <summary>
        /// Inicializa el listado de productos y registra el evento de carga.
        /// </summary>
        public MostrarProductosForm()
        {
            InitializeComponent();
            _productoService = new ProductoService();
            this.Load += MostrarProductosForm_Load;
        }

        /// <summary>
        /// Constructor auxiliar para mostrar productos desde una selección de stock.
        /// </summary>
        /// <param name="stockSeleccionado">Stock desde el que se abrió el listado.</param>
        public MostrarProductosForm(Stock stockSeleccionado)
        {
        }

        /// <summary>
        /// Carga los productos al inicializarse el formulario.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void MostrarProductosForm_Load(object sender, EventArgs e)
        {
            CargarProductos();
        }

        /// <summary>
        /// Obtiene los productos desde el servicio y los muestra en la grilla.
        /// </summary>
        private void CargarProductos()
        {
            try
            {
                List<Producto> productos = _productoService.ObtenerTodosProductos();
                dataGridView1.DataSource = productos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Abre el formulario de creación de producto y recarga la lista al finalizar.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonCrearProducto2_Click(object sender, EventArgs e)
        {
            CrearProductoForm formCrear = new CrearProductoForm();
            formCrear.ShowDialog();
            CargarProductos(); // Refresca la lista después de crear
        }

        /// <summary>
        /// Abre el formulario de modificación para el producto seleccionado.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonModificarProducto2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Producto productoSeleccionado = (Producto)dataGridView1.SelectedRows[0].DataBoundItem;
                ModificarProductoForm formModificar = new ModificarProductoForm(productoSeleccionado);
                formModificar.ShowDialog();
                CargarProductos(); // Refresca la lista después de modificar
            }
            else
            {
                MessageBox.Show("Seleccione un producto para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Elimina el producto seleccionado previa confirmación del usuario.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Producto productoSeleccionado = (Producto)dataGridView1.SelectedRows[0].DataBoundItem;

                DialogResult result = MessageBox.Show($"¿Está seguro de eliminar el producto {productoSeleccionado.Nombre}?",
                                                      "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _productoService.EliminarProducto(productoSeleccionado.IdProducto);
                        CargarProductos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Recarga manualmente el listado de productos.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonActualizarProducto_Click(object sender, EventArgs e)
        {
            CargarProductos();
        }
    }
}
