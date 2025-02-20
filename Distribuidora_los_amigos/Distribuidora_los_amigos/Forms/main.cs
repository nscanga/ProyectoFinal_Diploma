using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL.Exceptions;
using Distribuidora_los_amigos.Forms.GestionUsuarios;
using Distribuidora_los_amigos.Forms.Productos;
using Distribuidora_los_amigos.Forms.Proveedores;
using Distribuidora_los_amigos.Forms.StockForm;
using DOMAIN;
using Service.DAL.Contracts;
using Service.DOMAIN;
using Service.Facade;
using Services.Facade;
using Syncfusion.WinForms.DataGrid;
using Usuario = Service.DOMAIN.Usuario;

namespace Distribuidora_los_amigos
{
    public partial class main : Form, IIdiomaObserver
    {
        public main()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                InicializadorDeIdioma();


                // Obtener el usuario logueado desde el SesionService
                Usuario usuarioLogueado = SesionService.UsuarioLogueado;
                // Obtener los roles del usuario logueado
                string rolesUsuario = SesionService.ObtenerRolesUsuario();
                // Obtener las patentes del usuario logueado
                List<Patente> patentesDelUsuario = usuarioLogueado.GetPatentes();
                // Obtener las familias para mostrar en el label de perfil
                List<Familia> familiasDelUsuario = usuarioLogueado.GetFamilias();
                // Aplicar los accesos solo a los menús
                AccesoService.ConfigureMenuItems(tOOLSToolStripMenuItem1, patentesDelUsuario);

                label2.Text = $": {usuarioLogueado.UserName}";
                label4.Text = $": {rolesUsuario}";
                // Suscribirse a los cambios de idioma
                IdiomaService.Subscribe(this);

                // Obtener el idioma guardado y aplicarlo
                string currentLanguage = IdiomaService.LoadUserLanguage();

                // Establecer la cultura del hilo principal al idioma guardado
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLanguage);

                // Traducir los controles del formulario
                IdiomaService.TranslateForm(this);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Error");
                LoggerService.WriteException(ex);
            }




        }


        private void cATEGORYToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            try
            {

                // Cierra el formulario principal
                LoggerService.WriteLog($"El usuario {SesionService.UsuarioLogueado.UserName} cerró sesión.", System.Diagnostics.TraceLevel.Info);
                // Desuscribirse cuando se cierra el formulario

                SesionService.ClearSession(); // Limpiar la sesión  

                Application.Restart();
            }
            catch (Exception ex)
            {
                string messageKey = "Ocurrió un error al cerrar sesión:";
                string translatedMessage = TranslateMessageKey(messageKey);
                MessageBox.Show(translatedMessage + ex.Message);

                // Manejar errores durante el proceso de cierre de sesión

                // Registrar el error
                LoggerService.WriteLog($"Error al cerrar sesión: {ex.Message}", System.Diagnostics.TraceLevel.Error);
            }
        }

        public void UpdateIdioma()
        {
            IdiomaService.TranslateForm(this);
            this.Refresh();
        }

        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            var form = new CrearUsuarioForm();
            form.Show();
        }

        private void notepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in this.MdiChildren)
            {
                if (form is MostrarUsuariosForm)
                {
                    form.BringToFront();
                    return;
                }
            }
            MostrarUsuariosForm usuariosForm = new MostrarUsuariosForm();
            usuariosForm.MdiParent = this;
            usuariosForm.WindowState = FormWindowState.Maximized; // Opcional: lo abre maximizado
            usuariosForm.Show();

        }

        private void AsignarRolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AsignarRolForm();
            form.Show();
        }

        private void taskManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Mostrar el formulario ModificarUsuarioForm
            var form = new ModificarUsuarioForm();
            form.Show();
        }

        private void crearRolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CrearRolForm();
            form.Show();
        }

        private void InicializadorDeIdioma()
        {
            listBox1.Items.AddRange(new string[] { "Español", "Inglés", "Portugués" });

            // Seleccionar el idioma actual
            Usuario usuarioLogueado = SesionService.UsuarioLogueado;

            string currentLanguage = UserService.GetUserLanguage(usuarioLogueado.IdUsuario);
            //string currentLanguage = IdiomaService.LoadUserLanguage();
            Dictionary<string, string> reverseLanguageMap = new Dictionary<string, string>()
            {
            { "es-ES", "Español" },
            { "en-US", "Inglés" },
            { "pt-PT", "Portugués" }
             };

            // Asignar el idioma seleccionado en la ListBox
            listBox1.SelectedItem = reverseLanguageMap.ContainsKey(currentLanguage) ? reverseLanguageMap[currentLanguage] : "Inglés"; // Predeterminado a Inglés
        }

        private void crearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CrearRolForm();
            form.Show();
        }

        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CrearProductoForm();
            form.Show();
        }

        private void vERPRODUCTOSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in this.MdiChildren)
            {
                if (form is MostrarProductosForm)
                {
                    form.BringToFront();
                    return;
                }
            }
            MostrarProductosForm usuariosForm = new MostrarProductosForm();
            usuariosForm.MdiParent = this;
            usuariosForm.WindowState = FormWindowState.Maximized; // Opcional: lo abre maximizado
            usuariosForm.Show();
        }

        private void mODIFICARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ModificarProductoForm();
            form.Show();
        }

        private void mostrarStockStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MostrarStockForm();
            form.Show();

        }

        private void mostrarProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MostrarProveedoresForm();
            form.Show();
        }

        private void crearProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CrearProductoForm();
            form.Show();
        }

        private void modificarProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ModificarProveedorForm();
            form.Show();
        }
    }
}
