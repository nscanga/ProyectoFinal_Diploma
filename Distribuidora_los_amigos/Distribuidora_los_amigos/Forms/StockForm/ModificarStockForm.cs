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

            // Cargar los datos en los campos
            numericUpDownStock.Value = _stock.Cantidad;
            comboBoxTipoStock.Text = _stock.Tipo;
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
                // Validar que la cantidad no sea negativa
                int nuevaCantidad = (int)numericUpDownStock.Value;
                if (nuevaCantidad < 0)
                {
                    string messageKey = "CantidadStockNoNegativa";
                    string translatedMessage = IdiomaService.Translate(messageKey);
                    string titleKey = "Error";
                    string translatedTitle = IdiomaService.Translate(titleKey);
                    MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Asignar los nuevos valores al stock
                _stock.Cantidad = nuevaCantidad;
                _stock.Tipo = comboBoxTipoStock.Text;

                // Actualizar en la base de datos
                _stockService.ModificarStock(_stock.IdProducto, _stock.Cantidad);

                string successKey = "StockModificadoExito";
                string translatedSuccess = IdiomaService.Translate(successKey);
                string successTitleKey = "Éxito";
                string translatedSuccessTitle = IdiomaService.Translate(successTitleKey);
                MessageBox.Show(translatedSuccess, translatedSuccessTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                string errorKey = "ErrorModificarStock";
                string translatedError = IdiomaService.Translate(errorKey) + ": " + ex.Message;
                string errorTitleKey = "Error";
                string translatedErrorTitle = IdiomaService.Translate(errorTitleKey);
                MessageBox.Show(translatedError, translatedErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
