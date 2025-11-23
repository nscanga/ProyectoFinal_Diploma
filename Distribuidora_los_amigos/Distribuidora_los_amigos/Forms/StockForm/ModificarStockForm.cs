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
using BLL.Exceptions;
using DOMAIN;
using Service.Facade;
using Services.Facade;
using Service.DAL.Contracts;
using Service.ManegerEx;

namespace Distribuidora_los_amigos.Forms.StockForm
{
    public partial class ModificarStockForm : Form, IIdiomaObserver
    {
        private readonly StockService _stockService;
        private Stock _stock;

        /// <summary>
        /// Inicializa el formulario con los datos del stock a modificar.
        /// </summary>
        /// <param name="stock">Stock seleccionado para edición.</param>
        public ModificarStockForm(Stock stock)
        {
            InitializeComponent();
            _stockService = new StockService();
            _stock = stock;

            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);

            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);

            // Configurar ayuda F1
            this.KeyPreview = true;
            this.KeyDown += ModificarStockForm_KeyDown;

            // Cargar los tipos de stock disponibles
            CargarTiposStock();

            // Cargar los datos en los campos
            numericUpDownStock.Value = _stock.Cantidad;

            // Preseleccionar el tipo de stock actual
            int tipoIndex = comboBoxTipoStock.Items.IndexOf(_stock.Tipo);
            if (tipoIndex >= 0)
            {
                comboBoxTipoStock.SelectedIndex = tipoIndex;
            }
            else
            {
                // Si el tipo no está en la lista, agregarlo y seleccionarlo
                comboBoxTipoStock.Items.Add(_stock.Tipo);
                comboBoxTipoStock.SelectedIndex = comboBoxTipoStock.Items.Count - 1;
            }
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
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        private void ModificarStockForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaModificarStock();
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
        /// Valida y guarda la nueva cantidad de stock para el producto.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void btnGuardarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ Validaciones básicas de UI (entrada del usuario)
                int nuevaCantidad = (int)numericUpDownStock.Value;
                
                if (nuevaCantidad < 0)
                {
                    MessageBox.Show(
                        IdiomaService.Translate("La cantidad no puede ser negativa."),
                        IdiomaService.Translate("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numericUpDownStock.Focus();
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

                // 🎯 Actualizar los valores del stock (reemplazar, no sumar)
                _stock.Cantidad = nuevaCantidad;
                _stock.Tipo = comboBoxTipoStock.Text.Trim();

                // 🚀 El BLL se encarga de TODAS las validaciones de negocio:
                // - Validar cantidad >= 0
                // - Validar tipo no vacío
                // - Validar que el producto existe
                // - REEMPLAZAR la cantidad (no sumar)
                _stockService.ModificarStock(_stock);

                MessageBox.Show(
                    IdiomaService.Translate("✅ Stock modificado correctamente."),
                    IdiomaService.Translate("Éxito"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (StockException stockEx)
            {
                // 🎯 Excepciones de reglas de negocio de stock
                MessageBox.Show(
                    $"❌ {stockEx.Message}",
                    IdiomaService.Translate("Error de Validación"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoggerService.WriteException(stockEx);
            }
            catch (DatabaseException dbEx)
            {
                // 🎯 Errores de conexión/base de datos
                string username = ObtenerUsuarioActual();
                ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    MessageBox.Show(
                        "No se puede modificar el stock sin conexión a la base de datos.\n" +
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
                    $"Error inesperado al modificar el stock: {ex.Message}",
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
        /// Cierra el formulario sin guardar cambios.
        /// </summary>
        private void btnCancelarProducto_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
