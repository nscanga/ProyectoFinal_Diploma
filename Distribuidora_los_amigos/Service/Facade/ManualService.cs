using Service.DAL.Implementations.SqlServer;
using Service.Logic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service.Facade
{
    /// <summary>
    /// Servicio que expone métodos para abrir secciones del manual contextual según el idioma.
    /// </summary>
    public class ManualService
    {
        /// <summary>
        /// Obtiene el repositorio del manual según el idioma actual del usuario.
        /// </summary>
        private ManualRepository GetManualRepository()
        {
            // Siempre obtener el idioma actual en el momento de abrir la ayuda
            string languageCode = IdiomaLogic.LoadUserLanguage();
            return new ManualRepository(languageCode);
        }

        #region Ayuda General

        /// <summary>
        /// Despliega la ayuda de la pantalla principal.
        /// </summary>
        public void AbrirAyudaMain()
        {
            GetManualRepository().AbrirAyudaMain();
        }

        #endregion

        #region Ayuda Login y Seguridad

        /// <summary>
        /// Abre la ayuda para el inicio de sesión.
        /// </summary>
        public void AbrirAyudaLogin()
        {
            GetManualRepository().AbrirAyudaLogin();
        }

        /// <summary>
        /// Muestra la ayuda para cambiar contraseñas.
        /// </summary>
        public void AbrirAyudaCambiarPass()
        {
            GetManualRepository().AbrirAyudaCambiarPass();
        }

        /// <summary>
        /// Despliega la guía de recuperación de contraseña.
        /// </summary>
        /// <param name="owner">Formulario propietario (necesario para formularios modales)</param>
        public void AbrirAyudaRecuperoPass(Control owner = null)
        {
            GetManualRepository().AbrirAyudaRecuperoPass(owner);
        }

        #endregion

        #region Ayuda Gestión de Usuarios

        /// <summary>
        /// Despliega la ayuda para crear usuarios.
        /// </summary>
        public void AbrirAyudaCrearUsuario()
        {
            GetManualRepository().AbrirAyudaCrearUsuario();
        }

        /// <summary>
        /// Presenta la ayuda para modificar usuarios.
        /// </summary>
        public void AbrirAyudaModUsuario()
        {
            GetManualRepository().AbrirAyudaModUsuario();
        }

        /// <summary>
        /// Abre la ayuda para visualizar usuarios registrados.
        /// </summary>
        public void AbrirAyudaMostrarUsuario()
        {
            GetManualRepository().AbrirAyudaMostrarUsuario();
        }

        #endregion

        #region Ayuda Roles y Permisos

        /// <summary>
        /// Abre la ayuda para asignar roles.
        /// </summary>
        public void AbrirAyudaAsignarRol()
        {
            GetManualRepository().AbrirAyudaAsignarRol();
        }

        /// <summary>
        /// Despliega la ayuda para crear roles.
        /// </summary>
        public void AbrirAyudaCrearRol()
        {
            GetManualRepository().AbrirAyudaCrearRol();
        }

        /// <summary>
        /// Presenta la guía para crear patentes.
        /// </summary>
        public void AbrirAyudaCrearPatente()
        {
            GetManualRepository().AbrirAyudaCrearPatente();
        }

        #endregion

        #region Ayuda Backup y Restore

        /// <summary>
        /// Abre la sección de ayuda sobre la generación de respaldos.
        /// </summary>
        public void AbrirAyudaBackUp()
        {
            GetManualRepository().AbrirAyudaBackUp();
        }

        /// <summary>
        /// Muestra la ayuda sobre restauración de respaldos.
        /// </summary>
        public void AbrirAyudaRestore()
        {
            GetManualRepository().AbrirAyudaRestore();
        }

        #endregion

        #region Ayuda Bitácora

        /// <summary>
        /// Muestra la ayuda relativa a la consulta de la bitácora.
        /// </summary>
        public void AbrirAyudaBitacora()
        {
            GetManualRepository().AbrirAyudaBitacora();
        }

        #endregion

        #region Ayuda Clientes

        /// <summary>
        /// Abre la ayuda para crear clientes.
        /// </summary>
        public void AbrirAyudaCrearCliente()
        {
            GetManualRepository().AbrirAyudaCrearCliente();
        }

        /// <summary>
        /// Muestra la ayuda para modificar clientes.
        /// </summary>
        public void AbrirAyudaModificarCliente()
        {
            GetManualRepository().AbrirAyudaModificarCliente();
        }

        /// <summary>
        /// Despliega la ayuda para visualizar clientes.
        /// </summary>
        public void AbrirAyudaMostrarClientes()
        {
            GetManualRepository().AbrirAyudaMostrarClientes();
        }

        #endregion

        #region Ayuda Productos

        /// <summary>
        /// Abre la ayuda para crear productos.
        /// </summary>
        public void AbrirAyudaCrearProducto()
        {
            GetManualRepository().AbrirAyudaCrearProducto();
        }

        /// <summary>
        /// Muestra la ayuda para modificar productos.
        /// </summary>
        public void AbrirAyudaModificarProducto()
        {
            GetManualRepository().AbrirAyudaModificarProducto();
        }

        /// <summary>
        /// Despliega la ayuda para visualizar productos.
        /// </summary>
        public void AbrirAyudaMostrarProductos()
        {
            GetManualRepository().AbrirAyudaMostrarProductos();
        }

        /// <summary>
        /// Abre la ayuda para eliminar productos.
        /// </summary>
        public void AbrirAyudaEliminarProducto()
        {
            GetManualRepository().AbrirAyudaEliminarProducto();
        }

        #endregion

        #region Ayuda Proveedores

        /// <summary>
        /// Abre la ayuda para crear proveedores.
        /// </summary>
        public void AbrirAyudaCrearProveedor()
        {
            GetManualRepository().AbrirAyudaCrearProveedor();
        }

        /// <summary>
        /// Muestra la ayuda para modificar proveedores.
        /// </summary>
        public void AbrirAyudaModificarProveedor()
        {
            GetManualRepository().AbrirAyudaModificarProveedor();
        }

        /// <summary>
        /// Despliega la ayuda para visualizar proveedores.
        /// </summary>
        public void AbrirAyudaMostrarProveedores()
        {
            GetManualRepository().AbrirAyudaMostrarProveedores();
        }

        #endregion

        #region Ayuda Stock

        /// <summary>
        /// Muestra la ayuda para visualizar el stock.
        /// </summary>
        public void AbrirAyudaMostrarStock()
        {
            GetManualRepository().AbrirAyudaMostrarStock();
        }

        /// <summary>
        /// Abre la ayuda para modificar el stock.
        /// </summary>
        public void AbrirAyudaModificarStock()
        {
            GetManualRepository().AbrirAyudaModificarStock();
        }

        #endregion

        #region Ayuda Pedidos

        /// <summary>
        /// Abre la ayuda para crear pedidos.
        /// </summary>
        public void AbrirAyudaCrearPedido()
        {
            GetManualRepository().AbrirAyudaCrearPedido();
        }

        /// <summary>
        /// Muestra la ayuda para modificar pedidos.
        /// </summary>
        public void AbrirAyudaModificarPedido()
        {
            GetManualRepository().AbrirAyudaModificarPedido();
        }

        /// <summary>
        /// Despliega la ayuda para visualizar pedidos.
        /// </summary>
        public void AbrirAyudaMostrarPedidos()
        {
            GetManualRepository().AbrirAyudaMostrarPedidos();
        }

        /// <summary>
        /// Abre la ayuda para visualizar el detalle de un pedido.
        /// </summary>
        public void AbrirAyudaDetallePedido()
        {
            GetManualRepository().AbrirAyudaDetallePedido();
        }

        #endregion

        #region Ayuda Reportes

        /// <summary>
        /// Muestra la ayuda para el reporte de stock bajo.
        /// </summary>
        public void AbrirAyudaReporteStockBajo()
        {
            GetManualRepository().AbrirAyudaReporteStockBajo();
        }

        /// <summary>
        /// Despliega la ayuda para el reporte de productos más vendidos.
        /// </summary>
        public void AbrirAyudaReporteProductosMasVendidos()
        {
            GetManualRepository().AbrirAyudaReporteProductosMasVendidos();
        }

        #endregion
    }
}
