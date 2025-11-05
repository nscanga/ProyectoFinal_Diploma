using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service.DAL.Implementations.SqlServer
{
    /// <summary>
    /// Gestiona las rutas y apertura de archivos de ayuda contextual según el idioma configurado.
    /// </summary>
    public class ManualRepository
    {
        private string helpFilePath;

        /// <summary>
        /// Crea el repositorio seleccionando la ruta de ayuda acorde al idioma indicado.
        /// </summary>
        /// <param name="languageCode">Código de idioma para resolver la ruta del archivo CHM.</param>
        public ManualRepository(string languageCode)
        {
            // Inicializa el helpFilePath basado en el idioma
            helpFilePath = GetHelpFilePath(languageCode);
        }

        /// <summary>
        /// Obtiene la ruta del archivo de ayuda correspondiente al idioma solicitado.
        /// </summary>
        /// <param name="languageCode">Código de idioma configurado.</param>
        /// <returns>Ruta absoluta del archivo de ayuda.</returns>
        public static string GetHelpFilePath(string languageCode)
        {
            string key = $"HelpFilePath_{languageCode}";
            string helpFilePath = ConfigurationManager.AppSettings[key];

            // Si no existe la clave para el idioma especificado, se usa la ruta por defecto
            if (string.IsNullOrEmpty(helpFilePath))
            {
                helpFilePath = ConfigurationManager.AppSettings["HelpFilePath_Default"];
            }

            return helpFilePath;
        }

        #region Ayuda General

        /// <summary>
        /// Despliega la ayuda de la pantalla principal.
        /// </summary>
        public void AbrirAyudaMain()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "20");
        }

        #endregion

        #region Ayuda Login y Seguridad

        /// <summary>
        /// Abre la ayuda correspondiente al inicio de sesión.
        /// </summary>
        public void AbrirAyudaLogin()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "31");
        }

        /// <summary>
        /// Muestra la guía para cambiar la contraseña.
        /// </summary>
        public void AbrirAyudaCambiarPass()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "33");
        }

        /// <summary>
        /// Presenta la ayuda sobre el proceso de recuperación de contraseña.
        /// </summary>
        /// <param name="owner">Formulario propietario (opcional, necesario para formularios modales)</param>
        public void AbrirAyudaRecuperoPass(Control owner = null)
        {
            Help.ShowHelp(owner, helpFilePath, HelpNavigator.TopicId, "32");
        }

        #endregion

        #region Ayuda Gestión de Usuarios

        /// <summary>
        /// Abre la ayuda para registrar nuevos usuarios.
        /// </summary>
        public void AbrirAyudaCrearUsuario()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "23");
        }

        /// <summary>
        /// Muestra la ayuda asociada a la modificación de usuarios.
        /// </summary>
        public void AbrirAyudaModUsuario()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "28");
        }

        /// <summary>
        /// Despliega la ayuda para visualizar usuarios existentes.
        /// </summary>
        public void AbrirAyudaMostrarUsuario()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "29");
        }

        #endregion

        #region Ayuda Roles y Permisos

        /// <summary>
        /// Despliega la ayuda para asignar roles a los usuarios.
        /// </summary>
        public void AbrirAyudaAsignarRol()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "24");
        }

        /// <summary>
        /// Despliega la ayuda para crear roles.
        /// </summary>
        public void AbrirAyudaCrearRol()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "26");
        }

        /// <summary>
        /// Presenta la guía para la creación de patentes.
        /// </summary>
        public void AbrirAyudaCrearPatente()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "27");
        }

        #endregion

        #region Ayuda Backup y Restore

        /// <summary>
        /// Abre la sección de ayuda sobre la generación de respaldos.
        /// </summary>
        public void AbrirAyudaBackUp()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "22");
        }

        /// <summary>
        /// Muestra la ayuda sobre restauración de respaldos.
        /// </summary>
        public void AbrirAyudaRestore()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "30");
        }

        #endregion

        #region Ayuda Bitácora

        /// <summary>
        /// Muestra la ayuda relativa a la consulta de la bitácora.
        /// </summary>
        public void AbrirAyudaBitacora()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "25");
        }

        #endregion

        #region Ayuda Clientes

        /// <summary>
        /// Abre la ayuda para crear clientes.
        /// </summary>
        public void AbrirAyudaCrearCliente()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "40");
        }

        /// <summary>
        /// Muestra la ayuda para modificar clientes.
        /// </summary>
        public void AbrirAyudaModificarCliente()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "41");
        }

        /// <summary>
        /// Despliega la ayuda para visualizar clientes.
        /// </summary>
        public void AbrirAyudaMostrarClientes()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "42");
        }

        #endregion

        #region Ayuda Productos

        /// <summary>
        /// Abre la ayuda para crear productos.
        /// </summary>
        public void AbrirAyudaCrearProducto()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "50");
        }

        /// <summary>
        /// Muestra la ayuda para modificar productos.
        /// </summary>
        public void AbrirAyudaModificarProducto()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "51");
        }

        /// <summary>
        /// Despliega la ayuda para visualizar productos.
        /// </summary>
        public void AbrirAyudaMostrarProductos()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "52");
        }

        /// <summary>
        /// Abre la ayuda para eliminar productos.
        /// </summary>
        public void AbrirAyudaEliminarProducto()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "53");
        }

        #endregion

        #region Ayuda Proveedores

        /// <summary>
        /// Abre la ayuda para crear proveedores.
        /// </summary>
        public void AbrirAyudaCrearProveedor()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "60");
        }

        /// <summary>
        /// Muestra la ayuda para modificar proveedores.
        /// </summary>
        public void AbrirAyudaModificarProveedor()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "61");
        }

        /// <summary>
        /// Despliega la ayuda para visualizar proveedores.
        /// </summary>
        public void AbrirAyudaMostrarProveedores()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "62");
        }

        #endregion

        #region Ayuda Stock

        /// <summary>
        /// Muestra la ayuda para visualizar el stock.
        /// </summary>
        public void AbrirAyudaMostrarStock()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "70");
        }

        /// <summary>
        /// Abre la ayuda para modificar el stock.
        /// </summary>
        public void AbrirAyudaModificarStock()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "71");
        }

        #endregion

        #region Ayuda Pedidos

        /// <summary>
        /// Abre la ayuda para crear pedidos.
        /// </summary>
        public void AbrirAyudaCrearPedido()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "80");
        }

        /// <summary>
        /// Muestra la ayuda para modificar pedidos.
        /// </summary>
        public void AbrirAyudaModificarPedido()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "81");
        }

        /// <summary>
        /// Despliega la ayuda para visualizar pedidos.
        /// </summary>
        public void AbrirAyudaMostrarPedidos()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "82");
        }

        /// <summary>
        /// Abre la ayuda para visualizar el detalle de un pedido.
        /// </summary>
        public void AbrirAyudaDetallePedido()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "83");
        }

        #endregion

        #region Ayuda Reportes

        /// <summary>
        /// Muestra la ayuda para el reporte de stock bajo.
        /// </summary>
        public void AbrirAyudaReporteStockBajo()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "90");
        }

        /// <summary>
        /// Despliega la ayuda para el reporte de productos más vendidos.
        /// </summary>
        public void AbrirAyudaReporteProductosMasVendidos()
        {
            Help.ShowHelp(null, helpFilePath, HelpNavigator.TopicId, "91");
        }

        #endregion
    }
}

