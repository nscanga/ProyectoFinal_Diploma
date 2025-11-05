using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Service.DAL.Contracts;
using Service.DOMAIN.DTO;
using Service.Facade;
using Service.ManegerEx;
using Services.Facade;

namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    public partial class MostrarUsuariosForm : Form, IIdiomaObserver
    {
        /// <summary>
        /// Inicializa el formulario de usuarios configurando la grilla y eventos principales.
        /// </summary>
        public MostrarUsuariosForm()
        {
            InitializeComponent();
            ConfigureGridColumns();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosed += MostrarUsuariosForm_FormClosed;
            this.KeyPreview = true;
            this.KeyDown += MostrarUsuariosForm_KeyDown;
            
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
            ConfigureGridColumns(); // Reconfigurar columnas con nuevas traducciones
            this.Refresh();
        }

        /// <summary>
        /// Configura las columnas del grid con los encabezados traducidos y las propiedades necesarias.
        /// </summary>
        private void ConfigureGridColumns()
        {
            // Desactivar la generación automática de columnas
            sfDataGrid.AutoGenerateColumns = false;
            
            // Limpiar columnas existentes si las hay
            sfDataGrid.Columns.Clear();

            // Crear y configurar las columnas
            var gridTextColumn1 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            var gridTextColumn2 = new Syncfusion.WinForms.DataGrid.GridTextColumn();
            var gridTextColumn3 = new Syncfusion.WinForms.DataGrid.GridTextColumn();

            string translatedUsuario = TranslateMessageKey("Usuario");
            string translatedFamilia = TranslateMessageKey("Familia");
            string translatedEstado = TranslateMessageKey("Estado");

            // Configurar las columnas
            gridTextColumn1.HeaderText = translatedUsuario;
            gridTextColumn1.MappingName = "Usuario";
            gridTextColumn1.Width = 150D;
            gridTextColumn1.AllowFiltering = true;

            gridTextColumn2.HeaderText = translatedFamilia;
            gridTextColumn2.MappingName = "Familia";
            gridTextColumn2.Width = 200D;
            gridTextColumn2.AllowFiltering = true;

            gridTextColumn3.HeaderText = translatedEstado;
            gridTextColumn3.MappingName = "Estado";
            gridTextColumn3.Width = 100D;
            gridTextColumn3.AllowFiltering = true;

            // Agregar las columnas al grid
            sfDataGrid.Columns.Add(gridTextColumn1);
            sfDataGrid.Columns.Add(gridTextColumn2);
            sfDataGrid.Columns.Add(gridTextColumn3);
        }

        /// <summary>
        /// Traduce la interfaz y carga los usuarios con sus familias y patentes.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void MostrarUsuariosForm_Load(object sender, EventArgs e)
        {
            LoggerService.WriteLog($"Formulario '{this.Text}' abierto.", System.Diagnostics.TraceLevel.Info);

            try
            {
                // Intentar cargar los usuarios con sus familias y patentes
                List<UsuarioRolDto> usuariosConFamilias = UserService.GetUsuariosConFamilasYPatentes();

                // Asignar la lista al DataGridView
                sfDataGrid.DataSource = usuariosConFamilias;

                // Log exitoso
                LoggerService.WriteLog($"Se cargaron {usuariosConFamilias.Count} usuarios en el formulario '{this.Text}'", System.Diagnostics.TraceLevel.Info);
            }
            catch (SqlException ex)
            {
                // Manejo de excepciones relacionadas con la base de datos
                string messageKey = "Error al cargar usuarios con familias y patentes.";
                string translatedMessage = TranslateMessageKey(messageKey);


                ErrorHandler.HandleSqlException(ex, translatedMessage);
                LoggerService.WriteLog($"Error SQL al cargar usuarios en el formulario '{this.Text}': {ex.Message}", System.Diagnostics.TraceLevel.Error);
            }
            catch (Exception ex)
            {
                // Manejo de cualquier otro tipo de excepción
                string messageKey = "Ocurrió un error al cargar los usuarios:";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteLog($"Error al cargar usuarios en el formulario '{this.Text}': {ex.Message}", System.Diagnostics.TraceLevel.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Registra el cierre del formulario para auditoría.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento de cierre.</param>
        private void MostrarUsuariosForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Desuscribirse del servicio de idiomas
            IdiomaService.Unsubscribe(this);
            
            // Registrar cuando el formulario se cierra
            LoggerService.WriteLog($"Formulario '{this.Text}' cerrado.", System.Diagnostics.TraceLevel.Info);
        }

        /// <summary>
        /// Traduce una clave textual usando el servicio de idiomas.
        /// </summary>
        /// <param name="messageKey">Clave a traducir.</param>
        /// <returns>Cadena traducida.</returns>
        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }
        /// <summary>
        /// Abre la ayuda del formulario cuando se presiona F1.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void MostrarUsuariosForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {

                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaMostrarUsuario();
                    e.Handled = true; // Prevenir propagación del evento
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ayuda: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }

        }
    }
}
