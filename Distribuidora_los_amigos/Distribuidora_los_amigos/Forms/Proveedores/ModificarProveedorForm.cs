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

namespace Distribuidora_los_amigos.Forms.Proveedores
{
    public partial class ModificarProveedorForm : Form, IIdiomaObserver
    {
        private readonly ProveedorService _proveedorService;
        private Proveedor _proveedor;

        /// <summary>
        /// Inicializa el formulario con los datos del proveedor a modificar.
        /// </summary>
        /// <param name="proveedor">Proveedor seleccionado para edición.</param>
        public ModificarProveedorForm(Proveedor proveedor)
        {
            InitializeComponent();
            _proveedorService = new ProveedorService();
            _proveedor = proveedor;

            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);

            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);

            // Configurar ayuda F1
            this.KeyPreview = true;
            this.KeyDown += ModificarProveedorForm_KeyDown;

            // Cargar los datos en los campos del formulario
            textBox1.Text = _proveedor.Nombre;
            textBox2.Text = _proveedor.Direccion;
            textBox3.Text = _proveedor.Email;
            textBox4.Text = _proveedor.Telefono;
            comboBox1.Text = _proveedor.Categoria;
            checkBox1.Checked = _proveedor.Activo;
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        private void ModificarProveedorForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaModificarProveedor();
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
        /// Guarda las modificaciones realizadas sobre el proveedor actual.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonGuardarProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ Validaciones básicas de UI (entrada del usuario)
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show(
                        IdiomaService.Translate("El nombre es obligatorio."),
                        IdiomaService.Translate("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox1.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show(
                        IdiomaService.Translate("La dirección es obligatoria."),
                        IdiomaService.Translate("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox2.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    MessageBox.Show(
                        IdiomaService.Translate("El email es obligatorio."),
                        IdiomaService.Translate("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox3.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show(
                        IdiomaService.Translate("El teléfono es obligatorio."),
                        IdiomaService.Translate("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox4.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(comboBox1.Text))
                {
                    MessageBox.Show(
                        IdiomaService.Translate("La categoría es obligatoria."),
                        IdiomaService.Translate("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBox1.Focus();
                    return;
                }

                // 🎯 Actualizar los datos del proveedor con los valores ingresados
                _proveedor.Nombre = textBox1.Text.Trim();
                _proveedor.Direccion = textBox2.Text.Trim();
                _proveedor.Email = textBox3.Text.Trim();
                _proveedor.Telefono = textBox4.Text.Trim();
                _proveedor.Categoria = comboBox1.Text.Trim();
                _proveedor.Activo = checkBox1.Checked;

                // 🚀 El BLL se encarga de TODAS las validaciones de negocio:
                // - Validar email formato válido y no sea teléfono
                // - Validar teléfono mínimo 10 dígitos y no sea email
                // - Validar nombre no vacío
                // - Validar dirección no vacía
                _proveedorService.ModificarProveedor(_proveedor);

                MessageBox.Show(
                    IdiomaService.Translate("✅ Proveedor modificado correctamente."),
                    IdiomaService.Translate("Éxito"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (ProveedorException provEx)
            {
                // 🎯 Excepciones de reglas de negocio de proveedores
                MessageBox.Show(
                    $"❌ {provEx.Message}",
                    IdiomaService.Translate("Error de Validación"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoggerService.WriteException(provEx);
            }
            catch (DatabaseException dbEx)
            {
                // 🎯 Errores de conexión/base de datos
                string username = ObtenerUsuarioActual();
                ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    MessageBox.Show(
                        "No se puede modificar el proveedor sin conexión a la base de datos.\n" +
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
                    $"Error inesperado al modificar el proveedor: {ex.Message}",
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
    }
}
