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
using Service.DAL.Contracts;
using Services.Facade;

namespace Distribuidora_los_amigos.Forms.Clientes
{
    public partial class ModificarClienteForm : Form, IIdiomaObserver
    {
        private readonly ClienteService _clienteService;
        private Cliente _cliente;

        /// <summary>
        /// Inicializa el formulario de edición cargando el cliente a modificar.
        /// </summary>
        /// <param name="cliente">Cliente que se editará.</param>
        public ModificarClienteForm(Cliente cliente)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _clienteService = new ClienteService();
            _cliente = cliente;

            // Cargar datos en los campos del formulario
            textBox1.Text = _cliente.Nombre;
            textBox2.Text = _cliente.Direccion;
            textBox3.Text = _cliente.Email;
            textBox4.Text = _cliente.Telefono;
            textBox5.Text = _cliente.CUIT;
            checkBox1.Checked = _cliente.Activo;

            // Habilitar captura de teclas para F1
            this.KeyPreview = true;
            this.KeyDown += ModificarClienteForm_KeyDown;

            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);
            
            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);
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
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        private void ModificarClienteForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaModificarCliente();
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
        /// Guarda los cambios realizados sobre el cliente en edición.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonGuardarCliente_Click(object sender, EventArgs e)
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

                if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show(
                        IdiomaService.Translate("La dirección es obligatoria."),
                        IdiomaService.Translate("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox2.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    MessageBox.Show(
                        IdiomaService.Translate("El CUIT es obligatorio."),
                        IdiomaService.Translate("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox5.Focus();
                    return;
                }

                // 🎯 Actualizar los datos del cliente
                _cliente.Nombre = textBox1.Text.Trim();
                _cliente.Direccion = textBox2.Text.Trim();
                _cliente.Email = textBox3.Text.Trim();
                _cliente.Telefono = textBox4.Text.Trim();
                _cliente.CUIT = textBox5.Text.Trim();
                _cliente.Activo = checkBox1.Checked;

                // 🚀 El BLL se encarga de TODAS las validaciones de negocio:
                // - Validar email formato válido y no sea teléfono
                // - Validar teléfono mínimo 10 dígitos y no sea email
                // - Validar CUIT 11 dígitos
                _clienteService.ModificarCliente(_cliente);

                MessageBox.Show(
                    IdiomaService.Translate("✅ Cliente modificado correctamente."),
                    IdiomaService.Translate("Éxito"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (BLL.Exceptions.ClienteException cliEx)
            {
                // 🎯 Excepciones de reglas de negocio de clientes
                MessageBox.Show(
                    $"❌ {cliEx.Message}",
                    IdiomaService.Translate("Error de Validación"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoggerService.WriteException(cliEx);
            }
            catch (BLL.Exceptions.DatabaseException dbEx)
            {
                // 🎯 Errores de conexión/base de datos
                string username = ObtenerUsuarioActual();
                Service.ManegerEx.ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                
                if (dbEx.ErrorType == BLL.Exceptions.DatabaseErrorType.ConnectionFailed)
                {
                    MessageBox.Show(
                        "No se puede modificar el cliente sin conexión a la base de datos.\n" +
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
                    $"Error inesperado al modificar el cliente: {ex.Message}",
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
