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
    public partial class MostrarProveedoresForm : Form, IIdiomaObserver
    {
        private readonly ProveedorService _proveedorService;

        /// <summary>
        /// Inicializa el listado de proveedores.
        /// </summary>
        public MostrarProveedoresForm()
        {
            InitializeComponent();
            _proveedorService = new ProveedorService();

            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);

            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);

            // Configurar ayuda F1
            this.KeyPreview = true;
            this.KeyDown += MostrarProveedoresForm_KeyDown;

            this.Load += MostrarProveedoresForm_Load;
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

                if (proveedores.Count == 0)
                {
                    Console.WriteLine("ℹ️ No hay proveedores disponibles.");
                }
            }
            catch (DatabaseException dbEx)
            {
                string username = ObtenerUsuarioActual();
                ErrorHandler.HandleDatabaseException(dbEx, username, showMessageBox: true);
                dataGridViewProveedores.DataSource = new List<Proveedor>();
                Console.WriteLine("❌ Error de conexión al cargar proveedores");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralException(ex);
                dataGridViewProveedores.DataSource = new List<Proveedor>();
            }
        }

        private void MostrarProveedoresForm_Load(object sender, EventArgs e)
        {
            try
            {
                CargarProveedores();
                ConfigurarDataGridView();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralException(ex);
            }
        }

        /// <summary>
        /// Configura la visualización del DataGridView ocultando columnas no relevantes para el usuario.
        /// </summary>
        private void ConfigurarDataGridView()
        {
            // Ocultar la columna de ID que no es relevante para el usuario
            if (dataGridViewProveedores.Columns["IdProveedor"] != null)
                dataGridViewProveedores.Columns["IdProveedor"].Visible = false;

            // Configurar nombres de encabezados más amigables
            if (dataGridViewProveedores.Columns["Nombre"] != null)
                dataGridViewProveedores.Columns["Nombre"].HeaderText = IdiomaService.Translate("Nombre");
            
            if (dataGridViewProveedores.Columns["Cuit"] != null)
                dataGridViewProveedores.Columns["Cuit"].HeaderText = IdiomaService.Translate("CUIT");
            
            if (dataGridViewProveedores.Columns["Direccion"] != null)
                dataGridViewProveedores.Columns["Direccion"].HeaderText = IdiomaService.Translate("Dirección");
            
            if (dataGridViewProveedores.Columns["Telefono"] != null)
                dataGridViewProveedores.Columns["Telefono"].HeaderText = IdiomaService.Translate("Teléfono");
            
            if (dataGridViewProveedores.Columns["Email"] != null)
                dataGridViewProveedores.Columns["Email"].HeaderText = IdiomaService.Translate("Email");

            // Ajustar ancho de columnas
            dataGridViewProveedores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
                string messageKey = "SeleccioneProveedorModificar";
                string translatedMessage = IdiomaService.Translate(messageKey);
                string titleKey = "Advertencia";
                string translatedTitle = IdiomaService.Translate(titleKey);
                MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                string messageKey = "ConfirmarEliminarProveedor";
                string translatedMessage = string.Format(IdiomaService.Translate(messageKey), proveedorSeleccionado.Nombre);
                string titleKey = "ConfirmarEliminacion";
                string translatedTitle = IdiomaService.Translate(titleKey);
                
                DialogResult result = MessageBox.Show(
                    translatedMessage,
                    translatedTitle,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _proveedorService.EliminarProveedor(proveedorSeleccionado.IdProveedor);
                        
                        string successKey = "ProveedorEliminadoExito";
                        string translatedSuccess = IdiomaService.Translate(successKey);
                        string successTitleKey = "Éxito";
                        string translatedSuccessTitle = IdiomaService.Translate(successTitleKey);
                        MessageBox.Show(translatedSuccess, translatedSuccessTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarProveedores();
                    }
                    catch (Exception ex)
                    {
                        string errorKey = "ErrorEliminarProveedor";
                        string translatedError = IdiomaService.Translate(errorKey) + ": " + ex.Message;
                        string errorTitleKey = "Error";
                        string translatedErrorTitle = IdiomaService.Translate(errorTitleKey);
                        MessageBox.Show(translatedError, translatedErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                string messageKey = "SeleccioneProveedorEliminar";
                string translatedMessage = IdiomaService.Translate(messageKey);
                string titleKey = "Advertencia";
                string translatedTitle = IdiomaService.Translate(titleKey);
                MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                string messageKey = "ConfirmarEliminarProveedor";
                string translatedMessage = string.Format(IdiomaService.Translate(messageKey), proveedorSeleccionado.Nombre);
                string titleKey = "ConfirmarEliminacion";
                string translatedTitle = IdiomaService.Translate(titleKey);
                
                DialogResult resultado = MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    try
                    {
                        // Llamar al servicio para eliminar el proveedor
                        _proveedorService.EliminarProveedor(proveedorSeleccionado.IdProveedor);

                        // Refrescar la lista de proveedores después de eliminar
                        CargarProveedores();

                        string successKey = "ProveedorEliminadoExito";
                        string translatedSuccess = IdiomaService.Translate(successKey);
                        string successTitleKey = "Éxito";
                        string translatedSuccessTitle = IdiomaService.Translate(successTitleKey);
                        MessageBox.Show(translatedSuccess, translatedSuccessTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        string errorKey = "ErrorEliminarProveedor";
                        string translatedError = IdiomaService.Translate(errorKey) + ": " + ex.Message;
                        string errorTitleKey = "Error";
                        string translatedErrorTitle = IdiomaService.Translate(errorTitleKey);
                        MessageBox.Show(translatedError, translatedErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                string messageKey = "SeleccioneProveedorEliminar";
                string translatedMessage = IdiomaService.Translate(messageKey);
                string titleKey = "Advertencia";
                string translatedTitle = IdiomaService.Translate(titleKey);
                MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        private void MostrarProveedoresForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaMostrarProveedores();
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
        /// Cancela la suscripción al servicio de idioma al cerrar el formulario.
        /// </summary>
        /// <param name="e">Argumentos del evento.</param>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            IdiomaService.Unsubscribe(this);
            base.OnFormClosed(e);
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
