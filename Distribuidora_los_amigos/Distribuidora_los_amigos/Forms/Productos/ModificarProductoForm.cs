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
using Service.Facade;
using Services.Facade;
using Service.DAL.Contracts;

namespace Distribuidora_los_amigos.Forms.Productos
{
    public partial class ModificarProductoForm : Form, IIdiomaObserver
    {
        private readonly ProductoService _productoService;
        private Producto _producto;

        /// <summary>
        /// Constructor por defecto requerido por el diseñador.
        /// </summary>
        public ModificarProductoForm()
        {
        }

        /// <summary>
        /// Inicializa el formulario cargando los datos del producto a modificar.
        /// </summary>
        /// <param name="producto">Producto seleccionado para edición.</param>
        public ModificarProductoForm(Producto producto)
        {
            InitializeComponent();
            _productoService = new ProductoService();
            _producto = producto;

            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);
            
            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);

            // Configurar ayuda F1
            this.KeyPreview = true;
            this.KeyDown += ModificarProductoForm_KeyDown;

            // Cargar los datos en los campos del formulario
            textBoxNombreProducto.Text = _producto.Nombre;
            comboBox2.Text = _producto.Categoria;
            numericUpDownPrecioProducto.Value = _producto.Precio;

            // Cargar fechas
            dateTimePicker1.Value = _producto.FechaIngreso;
            if (_producto.Vencimiento.HasValue)
            {
                Disponible.Checked = true;
                dateTimePicker2.Value = _producto.Vencimiento.Value;
            }
            else
            {
                Disponible.Checked = false;
                dateTimePicker2.Enabled = false;
            }
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        private void ModificarProductoForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaModificarProducto();
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
        /// Actualiza los textos del formulario cuando cambia el idioma.
        /// </summary>
        public void UpdateIdioma()
        {
            IdiomaService.TranslateForm(this);
            this.Refresh();
        }

        /// <summary>
        /// Desuscribirse del servicio de idiomas al cerrar el formulario.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            IdiomaService.Unsubscribe(this);
            base.OnFormClosing(e);
        }

        /// <summary>
        /// Valida la información ingresada y actualiza el producto en la base de datos.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void btnGuardarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ Validaciones básicas de UI (formato/entrada del usuario)
                if (string.IsNullOrWhiteSpace(textBoxNombreProducto.Text))
                {
                    MessageBox.Show(
                        IdiomaService.Translate("Debe ingresar un nombre para el producto."),
                        IdiomaService.Translate("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxNombreProducto.Focus();
                    return;
                }

                if (!decimal.TryParse(numericUpDownPrecioProducto.Text, out decimal precio))
                {
                    MessageBox.Show(
                        IdiomaService.Translate("El precio debe ser un número válido."),
                        IdiomaService.Translate("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numericUpDownPrecioProducto.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(comboBox2.Text))
                {
                    MessageBox.Show(
                        IdiomaService.Translate("Debe seleccionar una categoría."),
                        IdiomaService.Translate("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBox2.Focus();
                    return;
                }

                // 🎯 Construir los datos del producto
                DateTime fechaIngreso = dateTimePicker1.Value.Date;
                DateTime? fechaVencimiento = dateTimePicker2.Checked 
                    ? (DateTime?)dateTimePicker2.Value.Date 
                    : null;

                // Actualizar los valores del producto
                _producto.Nombre = textBoxNombreProducto.Text.Trim();
                _producto.Categoria = comboBox2.Text.Trim();
                _producto.Precio = precio;
                _producto.FechaIngreso = fechaIngreso;
                _producto.Vencimiento = fechaVencimiento;

                // 🚀 El BLL se encarga de TODAS las validaciones de negocio:
                // - Validar nombre no vacío
                // - Validar categoría no vacía
                // - Validar precio > 0
                // - Validar fecha de ingreso válida
                // - Validar vencimiento >= fecha de ingreso
                _productoService.ModificarProducto(_producto);

                MessageBox.Show(
                    IdiomaService.Translate("✅ Producto modificado correctamente."),
                    IdiomaService.Translate("Éxito"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (BLL.Exceptions.ProductoException prodEx)
            {
                // 🎯 Excepciones de reglas de negocio de productos
                MessageBox.Show(
                    $"❌ {prodEx.Message}",
                    IdiomaService.Translate("Error de Validación"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoggerService.WriteException(prodEx);
            }
            catch (BLL.Exceptions.DatabaseException dbEx)
            {
                // 🎯 Errores de conexión/base de datos
                string username = ObtenerUsuarioActual();
                Service.ManegerEx.ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                
                if (dbEx.ErrorType == BLL.Exceptions.DatabaseErrorType.ConnectionFailed)
                {
                    MessageBox.Show(
                        "No se puede modificar el producto sin conexión a la base de datos.\n" +
                        "Por favor, verifique la conexión e intente nuevamente.",
                        "Error de Conexión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // 🎯 Errores inesperados
                MessageBox.Show(
                    $"Error inesperado al modificar el producto: {ex.Message}",
                    IdiomaService.Translate("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Obtiene el nombre del usuario actual de forma segura.
        /// </summary>
        private string ObtenerUsuarioActual()
        {
            try
            {
                return SesionService.UsuarioLogueado?.UserName ?? "Desconocido";
            }
            catch
            {
                return "Desconocido";
            }
        }

        /// <summary>
        /// Placeholder del evento Load para futuras inicializaciones.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void ModificarProductoForm_Load(object sender, EventArgs e)
        {

        }
    }
}
