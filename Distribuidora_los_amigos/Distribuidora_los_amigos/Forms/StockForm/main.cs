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
using Distribuidora_los_amigos.Forms.Clientes;
using Distribuidora_los_amigos.Forms.GestionUsuarios;
using Distribuidora_los_amigos.Forms.Pedidos;
using Distribuidora_los_amigos.Forms.Productos;
using Distribuidora_los_amigos.Forms.Proveedores;
using Distribuidora_los_amigos.Forms.Reportes;
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
        /// <summary>
        /// Inicializa el formulario principal como contenedor MDI y registra el evento de cambio de tamaño.
        /// </summary>
        public main()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            this.SizeChanged += MainForm_SizeChanged;
            this.KeyPreview = true; // Habilitar captura de teclas
            this.KeyDown += Main_KeyDown; // Agregar evento KeyDown
        }

        /// <summary>
        /// Carga la sesión activa, aplica permisos, configura el idioma y traduce el formulario al iniciarse.
        /// </summary>
        /// <param name="sender">Origen del evento de carga.</param>
        /// <param name="e">Argumentos del evento.</param>
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
                
                // ✅ Aplicar control de acceso granular a cada opción de menú
                ConfigurarAccesosMenu(patentesDelUsuario);

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

        /// <summary>
        /// Configura la visibilidad de todos los menús según las patentes del usuario.
        /// </summary>
        /// <param name="patentesDelUsuario">Lista de patentes que posee el usuario autenticado.</param>
        private void ConfigurarAccesosMenu(List<Patente> patentesDelUsuario)
        {
            // Diccionario que mapea cada opción de menú con su patente requerida
            var menuPatentes = new Dictionary<ToolStripMenuItem, string>
            {
                // PEDIDOS
                { CREARPEDIDOToolStripMenuItem, "CREAR_PEDIDO" },
                { mOSTRARPEDIDOSToolStripMenuItem, "MOSTRAR_PEDIDOS" },

                // CLIENTES
                { CrearClienteToolStripMenuItem, "Crear_cliente" },
                { mostrarClientesToolStripMenuItem, "Mostrar_clientes" },

                // PRODUCTOS
                { addItemToolStripMenuItem, "AGREGAR" },
                { mODIFICARToolStripMenuItem, "MODIFICAR" },
                { eLIMINARToolStripMenuItem, "ELIMINAR" },
                { vERPRODUCTOSToolStripMenuItem, "VER_PRODUCTOS" },

                // STOCK
                { mostrarStockStripMenuItem, "Mostrar_Stock" },

                // PROVEEDORES
                { mostrarProveedoresToolStripMenuItem, "Mostrar_Proveedores" },
                { modificarProveedorToolStripMenuItem, "Modificar_Proveedor" },
                { crearProveedorToolStripMenuItem, "Crear_Proveedor" },

                // GESTIÓN DE USUARIOS
                { btnCrearUsuario, "Crear_usuario" },
                { notepadToolStripMenuItem, "Ver_usuarios" },
                { AsignarRolToolStripMenuItem, "Asignar_rol" },
                { taskManagerToolStripMenuItem, "Modificar_Usuario" },
                { crearRolToolStripMenuItem, "Crear_rol" },
                { crearToolStripMenuItem, "Crear_patente" },

                // BACKUP Y RESTORE
                { generarBackupToolStripMenuItem, "Generar_Backup" },
                { restaurarBackupToolStripMenuItem, "RestaurarBackup" },

                // REPORTES
                { reporteStockBajoToolStripMenuItem, "ReporteStockBajo" },
                { reporteProductosMasVendidosToolStripMenuItem, "ReporteProductosMasVendidos" }
            };

            // Aplicar control de acceso a cada opción
            AccesoService.ConfigureMultipleMenuItems(menuPatentes, patentesDelUsuario);

            // ✅ Los menús ahora se muestran correctamente sin necesidad de OcultarMenusPadresSinHijos()
        }

        /// <summary>
        /// Placeholder para la opción de menú Category (sin implementación actual).
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void cATEGORYToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Placeholder para el clic en label1 (sin implementación actual).
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void label1_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Traduce una clave de mensaje utilizando el servicio de idiomas.
        /// </summary>
        /// <param name="messageKey">Clave que se desea traducir.</param>
        /// <returns>Cadena traducida para la clave indicada.</returns>
        private string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }

        /// <summary>
        /// Maneja el cierre de sesión registrando la auditoría, limpiando la sesión y reiniciando la aplicación.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
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

        /// <summary>
        /// Vuelve a traducir los textos del formulario principal cuando cambia el idioma.
        /// </summary>
        public void UpdateIdioma()
        {
            IdiomaService.TranslateForm(this);
            this.Refresh();
        }

        /// <summary>
        /// Abre el formulario de creación de clientes como ventana hija MDI.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void CrearClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CrearClienteForm();
            form.MdiParent = this;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        /// <summary>
        /// Muestra el formulario de listado de clientes reutilizando instancias abiertas cuando es posible.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void mostrarClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verificar si ya existe una instancia abierta
            foreach (Form form in this.MdiChildren)
            {
                if (form is MostrarClientesForm)
                {
                    form.BringToFront();
                    form.WindowState = FormWindowState.Maximized;
                    return;
                }
            }

            var clientesForm = new MostrarClientesForm();
            clientesForm.MdiParent = this;
            clientesForm.FormBorderStyle = FormBorderStyle.None;
            clientesForm.Dock = DockStyle.Fill;
            clientesForm.WindowState = FormWindowState.Maximized;
            clientesForm.Show();
        }

        /// <summary>
        /// Abre el formulario para crear un nuevo usuario dentro del contenedor MDI.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            var form = new CrearUsuarioForm();
            form.MdiParent = this;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        /// <summary>
        /// Muestra el formulario de administración de usuarios evitando duplicar ventanas abiertas.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
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
            usuariosForm.WindowState = FormWindowState.Maximized;
            usuariosForm.Show();
        }

        /// <summary>
        /// Abre el formulario para asignar roles a los usuarios.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void AsignarRolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AsignarRolForm();
            form.MdiParent = this;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        /// <summary>
        /// Presenta el formulario para habilitar o deshabilitar usuarios.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void taskManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ModificarUsuarioForm();
            form.MdiParent = this;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        /// <summary>
        /// Lanza el formulario de creación de roles en el área MDI.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void crearRolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CrearRolForm();
            form.MdiParent = this;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        /// <summary>
        /// Configura la lista de idiomas disponibles y selecciona el idioma persistido para el usuario.
        /// </summary>
        private void InicializadorDeIdioma()
        {
            listBox1.Items.AddRange(new string[] { "Español", "Inglés", "Portugués" });

            // Seleccionar el idioma actual - USAR LA MISMA FUENTE QUE EN LOGIN
            string currentLanguage = IdiomaService.LoadUserLanguage(); // ✅ Cambiar a IdiomaService
            
            Dictionary<string, string> reverseLanguageMap = new Dictionary<string, string>()
            {
                { "es-ES", "Español" },
                { "en-US", "Inglés" },
                { "pt-PT", "Portugués" }
            };

            // Asignar el idioma seleccionado en la ListBox
            listBox1.SelectedItem = reverseLanguageMap.ContainsKey(currentLanguage) ? reverseLanguageMap[currentLanguage] : "Inglés";
        }

        /// <summary>
        /// Abre el cuadro de diálogo de creación de patentes como ventana MDI.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void crearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CrearPatenteForm();
            form.MdiParent = this;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        /// <summary>
        /// Inicia el formulario para crear productos dentro del contenedor MDI.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CrearProductoForm();
            form.MdiParent = this;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        /// <summary>
        /// Muestra el listado de productos y reutiliza una instancia abierta si ya existe.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
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

        /// <summary>
        /// Abre el formulario para modificar productos dentro del espacio MDI.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void mODIFICARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ModificarProductoForm();
            form.MdiParent = this;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        /// <summary>
        /// Despliega la pantalla de stock como formulario hijo maximizado.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void mostrarStockStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MostrarStockForm();
            form.MdiParent = this;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        /// <summary>
        /// Abre el listado de proveedores evitando crear más de una instancia visible.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void mostrarProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verificar si ya existe una instancia abierta
            foreach (Form form in this.MdiChildren)
            {
                if (form is MostrarProveedoresForm)
                {
                    form.BringToFront();
                    form.WindowState = FormWindowState.Maximized;
                    return;
                }
            }

            var proveedoresForm = new MostrarProveedoresForm();
            proveedoresForm.MdiParent = this;
            proveedoresForm.FormBorderStyle = FormBorderStyle.None;
            proveedoresForm.Dock = DockStyle.Fill;
            proveedoresForm.WindowState = FormWindowState.Maximized;
            proveedoresForm.Show();
        }

        /// <summary>
        /// Muestra el diálogo para crear un nuevo proveedor.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void crearProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CrearProveedorForm(); // ✅ Corregido
            form.Show();
        }

        /// <summary>
        /// Presenta la pantalla de modificación de proveedores integrada al MDI.
        /// Abre el formulario de mostrar proveedores desde donde se puede modificar.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void modificarProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verificar si ya existe una instancia abierta
            foreach (Form form in this.MdiChildren)
            {
                if (form is MostrarProveedoresForm)
                {
                    form.BringToFront();
                    form.WindowState = FormWindowState.Maximized;
                    return;
                }
            }

            var proveedoresForm = new MostrarProveedoresForm();
            proveedoresForm.MdiParent = this;
            proveedoresForm.FormBorderStyle = FormBorderStyle.None;
            proveedoresForm.Dock = DockStyle.Fill;
            proveedoresForm.WindowState = FormWindowState.Maximized;
            proveedoresForm.Show();
        }

        /// <summary>
        /// Abre el formulario para crear pedidos dentro del contenedor MDI.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void CREARPEDIDOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CrearPedidoForm();
            form.MdiParent = this;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        /// <summary>
        /// Muestra el listado de pedidos y reutiliza instancias ya cargadas cuando corresponde.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void mOSTRARPEDIDOSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in this.MdiChildren)
            {
                if (form is MostrarPedidosForm)
                {
                    form.BringToFront();
                    form.WindowState = FormWindowState.Maximized;
                    form.Dock = DockStyle.Fill; // 📌 Para que se ajuste bien
                    return;
                }
            }

            MostrarPedidosForm pedidosForm = new MostrarPedidosForm
            {
                MdiParent = this,
                FormBorderStyle = FormBorderStyle.None, // 📌 Sin bordes para mejor ajuste
                Dock = DockStyle.Fill, // 📌 Ocupará todo el espacio disponible
                WindowState = FormWindowState.Maximized
            };

            pedidosForm.Show();
        }

        /// <summary>
        /// Mantiene las ventanas hijas ajustadas al tamaño del formulario principal.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            foreach (Form form in this.MdiChildren)
            {
                form.Dock = DockStyle.Fill; // 📌 Mantiene el formulario hijo ajustado al tamaño del MDI
                form.WindowState = FormWindowState.Maximized;
            }
        }

        /// <summary>
        /// Placeholder para el menú Tools (sin implementación asociada).
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void tOOLSToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Actualiza y persiste el idioma de la interfaz cuando el usuario selecciona una opción distinta.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;

            Dictionary<string, string> languageMap = new Dictionary<string, string>()
            {
                { "Español", "es-ES" },
                { "Inglés", "en-US" },
                { "Portugués", "pt-PT" }
            };

            string selectedItem = listBox1.SelectedItem.ToString();
            string selectedLanguage = languageMap.ContainsKey(selectedItem) ? languageMap[selectedItem] : "es-ES";

            // Guardar el nuevo idioma
            IdiomaService.SaveUserLanguage(selectedLanguage);

            // Actualizar la cultura del hilo principal
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);

            // Traducir el formulario principal
            IdiomaService.TranslateForm(this);
            this.Refresh();

            // ✅ Notificar a todos los observadores registrados
            // Esto debería traducir automáticamente todos los formularios que implementan IIdiomaObserver
            // No es necesario hacerlo manualmente ya que IdiomaService.SaveUserLanguage debería notificar
        }

        /// <summary>
        /// Abre el formulario de generación de backups manejando errores de inicialización.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void generarBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new BackUpForm();
                form.MdiParent = this;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;
                form.WindowState = FormWindowState.Maximized;
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario de backup: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Abre el formulario de restauración de backups manejando errores de inicialización.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void restaurarBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new RestoreForm();
                form.MdiParent = this;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;
                form.WindowState = FormWindowState.Maximized;
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario de restauración: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Abre el reporte de productos con stock bajo o crítico.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void reporteStockBajoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar si ya existe una instancia abierta
                foreach (Form form in this.MdiChildren)
                {
                    if (form is ReporteStockBajoForm)
                    {
                        form.BringToFront();
                        form.WindowState = FormWindowState.Maximized;
                        return;
                    }
                }

                var reporteForm = new ReporteStockBajoForm();
                reporteForm.MdiParent = this;
                reporteForm.FormBorderStyle = FormBorderStyle.None;
                reporteForm.Dock = DockStyle.Fill;
                reporteForm.WindowState = FormWindowState.Maximized;
                reporteForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Abre el reporte de productos más vendidos con filtros por período.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void reporteProductosMasVendidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar si ya existe una instancia abierta
                foreach (Form form in this.MdiChildren)
                {
                    if (form is ReporteProductosMasVendidosForm)
                    {
                        form.BringToFront();
                        form.WindowState = FormWindowState.Maximized;
                        return;
                    }
                }

                var reporteForm = new ReporteProductosMasVendidosForm();
                reporteForm.MdiParent = this;
                reporteForm.FormBorderStyle = FormBorderStyle.None;
                reporteForm.Dock = DockStyle.Fill;
                reporteForm.WindowState = FormWindowState.Maximized;
                reporteForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Muestra la ayuda del formulario principal cuando se presiona F1.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaMain();
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
