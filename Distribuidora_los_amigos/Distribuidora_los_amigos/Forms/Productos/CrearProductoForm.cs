using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using BLL.Exceptions;
using DOMAIN;
using Service.Facade;
using Services.Facade;
using Service.DAL.Contracts;
using Service.ManegerEx;

namespace Distribuidora_los_amigos.Forms.Productos
{
    public partial class CrearProductoForm : Form, IIdiomaObserver
    {
        private readonly ProductoService _productoService;
        
        /// <summary>
        /// Inicializa el formulario de creación de productos cargando catálogos auxiliares.
        /// </summary>
        public CrearProductoForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _productoService = new ProductoService();
            this.KeyPreview = true;
            this.KeyDown += CrearProductoForm_KeyDown; // Agregar evento KeyDown
            
            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);
            
            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);
            
            // Cargar datos en los ComboBox
            CargarCategorias();
            CargarTiposStock();
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
        /// Rellena el combo de categorías disponibles para los productos.
        /// </summary>
        private void CargarCategorias()
        {
            comboBoxCrearProducto.Items.Clear();
            comboBoxCrearProducto.Items.Add("Pollo Fresco");
            comboBoxCrearProducto.Items.Add("Pollo Congelado");
            comboBoxCrearProducto.Items.Add("Menudencias");
            comboBoxCrearProducto.Items.Add("Embutidos de Pollo");
            comboBoxCrearProducto.Items.Add("Huevos");
            comboBoxCrearProducto.Items.Add("Productos Elaborados");
            comboBoxCrearProducto.Items.Add("Condimentos y Salsas");
            comboBoxCrearProducto.Items.Add("Insumos");
            
            comboBoxCrearProducto.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Inicializa las opciones de unidad de medida para el stock.
        /// </summary>
        private void CargarTiposStock()
        {
            comboBoxTipoStock.Items.Clear();
            comboBoxTipoStock.Items.Add("Unidad");
            comboBoxTipoStock.Items.Add("Kilogramo");
            comboBoxTipoStock.Items.Add("Caja");
            comboBoxTipoStock.Items.Add("Bandeja");
            comboBoxTipoStock.Items.Add("Maple");
            comboBoxTipoStock.Items.Add("Docena");
            comboBoxTipoStock.Items.Add("Paquete");
            comboBoxTipoStock.Items.Add("Litro");
            
            comboBoxTipoStock.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Valida los datos ingresados y crea un nuevo producto con su stock inicial.
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

                if (comboBoxCrearProducto.SelectedIndex == -1)
                {
                    MessageBox.Show(
                        IdiomaService.Translate("Debe seleccionar una categoría."),
                        IdiomaService.Translate("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxCrearProducto.Focus();
                    return;
                }

                if (comboBoxTipoStock.SelectedIndex == -1)
                {
                    MessageBox.Show(
                        IdiomaService.Translate("Debe seleccionar un tipo de stock."),
                        IdiomaService.Translate("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxTipoStock.Focus();
                    return;
                }

                // 🎯 Construir el producto
                int cantidadStock = (int)numericUpDownStock.Value;
                string tipoStock = comboBoxTipoStock.Text;
                DateTime fechaIngreso = dateTimePicker1.Value.Date;
                DateTime? fechaVencimiento = dateTimePicker2.Checked ? (DateTime?)dateTimePicker2.Value.Date : null;

                Producto producto = new Producto()
                {
                    IdProducto = Guid.NewGuid(),
                    Nombre = textBoxNombreProducto.Text.Trim(),
                    Categoria = comboBoxCrearProducto.Text.Trim(),
                    Precio = precio,
                    FechaIngreso = fechaIngreso,
                    Vencimiento = fechaVencimiento,
                    Activo = true
                };

                // 🚀 El BLL se encarga de TODAS las validaciones de negocio:
                // - Validar nombre no vacío
                // - Validar categoría no vacía
                // - Validar precio > 0
                // - Validar fecha de ingreso válida
                // - Validar vencimiento >= fecha de ingreso
                // - Validar cantidad inicial >= 0
                // - Validar tipo de stock requerido
                _productoService.CrearProducto(producto, cantidadStock, tipoStock);
                
                LoggerService.WriteLog(
                    $"Producto creado: {producto.Nombre}, Categoría: {producto.Categoria}, Cantidad inicial: {cantidadStock}",
                    System.Diagnostics.TraceLevel.Info);

                MessageBox.Show(
                    IdiomaService.Translate("✅ Producto creado correctamente."),
                    IdiomaService.Translate("Éxito"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar los campos
                textBoxNombreProducto.Clear();
                comboBoxCrearProducto.SelectedIndex = -1;
                numericUpDownPrecioProducto.Value = 0;
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
                dateTimePicker2.Checked = false;
                numericUpDownStock.Value = 0;
                comboBoxTipoStock.SelectedIndex = -1;
                
                textBoxNombreProducto.Focus();
            }
            catch (ProductoException prodEx)
            {
                // 🎯 Excepciones de reglas de negocio de productos
                MessageBox.Show(
                    $"❌ {prodEx.Message}",
                    IdiomaService.Translate("Error de Validación"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoggerService.WriteException(prodEx);
            }
            catch (DatabaseException dbEx)
            {
                // 🎯 Errores de conexión/base de datos
                string username = ObtenerUsuarioActual();
                ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    MessageBox.Show(
                        "No se puede crear el producto sin conexión a la base de datos.\n" +
                        "Por favor, verifique la conexión e intente nuevamente.",
                        "Error de Conexión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // 🎯 Errores inesperados
                ErrorHandler.HandleGeneralException(ex);
                MessageBox.Show(
                    $"Error inesperado al crear el producto: {ex.Message}",
                    IdiomaService.Translate("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Traduce la clave textual usando el servicio de idiomas.
        /// </summary>
        /// <param name="messageKey">Clave a traducir.</param>
        /// <returns>Mensaje localizado.</returns>
        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }

        /// <summary>
        /// Placeholder para reaccionar a cambios de categoría seleccionada.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void CrearProductoForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaCrearProducto();
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
    }
}
