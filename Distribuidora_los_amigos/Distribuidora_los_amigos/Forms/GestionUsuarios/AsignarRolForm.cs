using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Service.DOMAIN;
using Service.Facade;
using Services.Facade;

namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    public partial class AsignarRolForm : Form
    {
        public AsignarRolForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosed += AsignarRolForm_FormClosed;
            this.KeyPreview = true;
            this.KeyDown += AsignarRolForm_KeyDown;
        }

        private void AsignarRolForm_Load(object sender, EventArgs e)
        {
            try
            {
                IdiomaService.TranslateForm(this);
                // Usamos UserService para obtener la lista de usuarios
                List<Usuario> usuarios = UserService.GetAllUsuarios(); // Método en UserService
                comboBoxUsers.DataSource = usuarios;
                comboBoxUsers.DisplayMember = "UserName";
                comboBoxUsers.ValueMember = "IdUsuario";

                // Usamos FamiliaService para obtener la lista de familias
                List<Familia> familias = FamiliaService.GetAllFamilias(); // Método en FamiliaService
                comboBoxRoles.DataSource = familias;
                comboBoxRoles.DisplayMember = "Nombre";  // Mostrar el nombre de la familia
                comboBoxRoles.ValueMember = "Id";


                LoggerService.WriteLog($"Formulario '{this.Text}' abierto.", System.Diagnostics.TraceLevel.Info);
            }
            catch (Exception ex)
            {
                // Manejar excepciones en la carga del formulario
                LoggerService.WriteLog($"Error al cargar el formulario AsignarRolForm: {ex.Message}", System.Diagnostics.TraceLevel.Error);



                string messageKey = "Ocurrió un error al cargar el formulario:";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AsignarRolForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Registrar el cierre del formulario
            LoggerService.WriteLog($"Formulario '{this.Text}' cerrado.", System.Diagnostics.TraceLevel.Info);
        }

        private void buttonAssignRole_Click(object sender, EventArgs e)
        {
            Usuario usuarioSeleccionado = (Usuario)comboBoxUsers.SelectedItem;
            Familia familiaSeleccionada = (Familia)comboBoxRoles.SelectedItem;
            if (comboBoxUsers.SelectedValue == null || comboBoxRoles.SelectedValue == null)
            {
                string messageKey = "Por favor, seleccione un usuario y una familia.";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage);


                return;
            }
            try
            {
                // Validar que se ha seleccionado un usuario y una familia


                // Obtener el IdUsuario seleccionado y la familia seleccionada
                Guid usuarioId = (Guid)comboBoxUsers.SelectedValue;
                Guid familiaId = (Guid)comboBoxRoles.SelectedValue;

                // Asignar la familia al usuario usando el servicio
                FamiliaService.AsignarFamiliaAUsuario(usuarioId, new Familia { Id = familiaId });



                // Registro en el log
                LoggerService.WriteLog($"Rol {familiaSeleccionada.Nombre} asignado al usuario {usuarioSeleccionado.UserName} exitosamente. Usuario: {SesionService.UsuarioLogueado.UserName}", System.Diagnostics.TraceLevel.Info);

                string messageKey = "Rol asignado con éxito.";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage);

            }
            catch (Exception ex)
            {
                // Manejar excepciones al asignar el rol
                LoggerService.WriteLog($"Error al asignar el rol: {familiaSeleccionada.Nombre} {ex.Message}", System.Diagnostics.TraceLevel.Error);
                string messageKey = "Ocurrió un error al asignar el rol:";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage, $"{familiaSeleccionada.Nombre}" + ex.Message);

            }

        }

        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }

        private void AsignarRolForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {

                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaAsignarRol();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Error");
                LoggerService.WriteException(ex);
            }

        }
    }
}
