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
using Service.DAL.Contracts;
using Service.Facade;
using Services.Facade;
using Service.ManegerEx;

namespace Distribuidora_los_amigos.Forms.Clientes
{
    public partial class CrearClienteForm : Form, IIdiomaObserver
    {
        private readonly ClienteService _clienteService;

        /// <summary>
        /// Inicializa el formulario de creación de clientes y prepara el servicio asociado.
        /// </summary>
        public CrearClienteForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _clienteService = new ClienteService();
            
            // Habilitar captura de teclas para F1
            this.KeyPreview = true;
            this.KeyDown += CrearClienteForm_KeyDown;
            
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
        /// Valida los datos ingresados y registra un nuevo cliente en el sistema.
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

                // 🎯 Construir el cliente
                Cliente cliente = new Cliente()
                {
                    IdCliente = Guid.NewGuid(),
                    Nombre = textBox1.Text.Trim(),
                    Direccion = textBox2.Text.Trim(),
                    Email = textBox3.Text.Trim(),
                    Telefono = textBox4.Text.Trim(),
                    CUIT = textBox5.Text.Trim(),
                    Activo = checkBox1.Checked
                };

                // 🚀 El BLL se encarga de TODAS las validaciones de negocio:
                // - Validar email formato válido y no sea teléfono
                // - Validar teléfono mínimo 10 dígitos y no sea email
                // - Validar CUIT 11 dígitos
                // - Verificar CUIT no duplicado
                _clienteService.CrearCliente(cliente);
                
                MessageBox.Show(
                    IdiomaService.Translate("✅ Cliente creado correctamente."),
                    IdiomaService.Translate("Éxito"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar los campos
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                checkBox1.Checked = false;
                
                textBox1.Focus();
            }
            catch (ClienteException cliEx)
            {
                // 🎯 Excepciones de reglas de negocio de clientes
                MessageBox.Show(
                    $"❌ {cliEx.Message}",
                    IdiomaService.Translate("Error de Validación"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoggerService.WriteException(cliEx);
            }
            catch (DatabaseException dbEx)
            {
                // 🎯 Errores de conexión/base de datos
                string username = ObtenerUsuarioActual();
                ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    MessageBox.Show(
                        "No se puede crear el cliente sin conexión a la base de datos.\n" +
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
                    $"Error inesperado: {ex.Message}",
                    IdiomaService.Translate("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void CrearClienteForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaCrearCliente();
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
