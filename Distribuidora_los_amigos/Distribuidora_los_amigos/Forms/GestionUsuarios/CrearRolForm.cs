using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Service.DAL.Contracts;
using Service.DOMAIN;
using Service.Facade;
using Services.Facade;

namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    public partial class CrearRolForm : Form, IIdiomaObserver
    {
        /// <summary>
        /// Inicializa el formulario y carga la lista de patentes disponibles.
        /// </summary>
        public CrearRolForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosed += CrearRolForm_FormClosed;
            this.KeyPreview = true;
            this.KeyDown += CrearRolForm_KeyDown;
            
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
        /// Traduce la interfaz y carga las patentes disponibles al iniciar el formulario.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void CrearRolForm_Load(object sender, EventArgs e)
        {
            LoggerService.WriteLog($"Formulario '{this.Text}' abierto.", System.Diagnostics.TraceLevel.Info);
            try
            {
                // Cargar todas las patentes disponibles en el CheckedListBox
                List<Patente> patentes = FamiliaService.GetAllPatentes();
                checkedListBoxPatents.DataSource = patentes;
                checkedListBoxPatents.DisplayMember = "Nombre";  // Mostrar el nombre de la patente
                checkedListBoxPatents.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                string messageKey = "Ocurrió un error al cargar las patentes:";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        /// <summary>
        /// Registra el cierre del formulario para auditoría.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento de cierre.</param>
        private void CrearRolForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Desuscribirse del servicio de idiomas
            IdiomaService.Unsubscribe(this);
            
            // Registrar cuando el formulario se cierra
            LoggerService.WriteLog($"Formulario '{this.Text}' cerrado.", System.Diagnostics.TraceLevel.Info);
        }

        /// <summary>
        /// Valida la selección de patentes y crea una nueva familia de permisos.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void buttonCreateRole_Click(object sender, EventArgs e)
        {
            // Recopilar el nombre de la familia desde la UI

            var nombreFamilia = textBoxRoleName.Text.Trim();

            // Recopilar las patentes seleccionadas desde la UI
            var patentesSeleccionadas = new List<Patente>();
            foreach (var item in checkedListBoxPatents.CheckedItems)
            {
                var patente = (Patente)item;
                patentesSeleccionadas.Add(patente);
            }

            if (patentesSeleccionadas.Count == 0)
            {

                string messageKey = "Debe seleccionar al menos una patente.";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage);
                LoggerService.WriteLog($"Debe seleccionar al menos una patente. Usuario: {SesionService.UsuarioLogueado.UserName}", System.Diagnostics.TraceLevel.Warning);
                return;

            }
            try
            {

                var nombresPatentes = string.Join(", ", patentesSeleccionadas.Select(p => p.Nombre));

                // Llamar a la fachada para crear la familia con las patentes seleccionadas
                FamiliaService.CrearFamiliaConPatentes(nombreFamilia, patentesSeleccionadas);

                LoggerService.WriteLog($"El Usuario {SesionService.UsuarioLogueado.UserName} Creo la familia {nombreFamilia} la cual tiene la patente de {nombresPatentes}", System.Diagnostics.TraceLevel.Info);


                string messageKey = "Familia creada con éxito.";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage);

                this.Close(); // Cierra el formulario después de guardar
            }
            catch (ArgumentException ex)
            {
                string messageKey = "Ocurrió un error al crear la familia:";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string messageKey = "Ocurrió un error al crear la familia:";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                LoggerService.WriteLog($"Ocurrió un error al crear la familia. Usuario: {SesionService.UsuarioLogueado.UserName}", System.Diagnostics.TraceLevel.Error);
            }
        }

        /// <summary>
        /// Traduce la clave indicada utilizando el servicio de idiomas.
        /// </summary>
        /// <param name="messageKey">Clave a traducir.</param>
        /// <returns>Mensaje localizado.</returns>
        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }

        /// <summary>
        /// Muestra la ayuda contextual del formulario al presionar F1.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void CrearRolForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaCrearRol();
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
