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
        private ManualRepository manualRepository;

        /// <summary>
        /// Inicializa el servicio cargando el repositorio correspondiente al idioma del usuario.
        /// </summary>
        public ManualService()
        {
            // Obtener el código de idioma de la configuración o de otro lugar
            string languageCode = IdiomaLogic.LoadUserLanguage();
            manualRepository = new ManualRepository(languageCode);
        }

        #region Ayuda General

        /// <summary>
        /// Despliega la ayuda de la pantalla principal.
        /// </summary>
        public void AbrirAyudaMain()
        {
            manualRepository.AbrirAyudaMain();
        }

        #endregion

        #region Ayuda Login y Seguridad

        /// <summary>
        /// Abre la ayuda para el inicio de sesión.
        /// </summary>
        public void AbrirAyudaLogin()
        {
            manualRepository.AbrirAyudaLogin();
        }

        /// <summary>
        /// Muestra la ayuda para cambiar contraseñas.
        /// </summary>
        public void AbrirAyudaCambiarPass()
        {
            manualRepository.AbrirAyudaCambiarPass();
        }

        /// <summary>
        /// Despliega la guía de recuperación de contraseña.
        /// </summary>
        /// <param name="owner">Formulario propietario (necesario para formularios modales)</param>
        public void AbrirAyudaRecuperoPass(Control owner = null)
        {
            manualRepository.AbrirAyudaRecuperoPass(owner);
        }

        #endregion

        #region Ayuda Gestión de Usuarios

        /// <summary>
        /// Despliega la ayuda para crear usuarios.
        /// </summary>
        public void AbrirAyudaCrearUsuario()
        {
            manualRepository.AbrirAyudaCrearUsuario();
        }

        /// <summary>
        /// Presenta la ayuda para modificar usuarios.
        /// </summary>
        public void AbrirAyudaModUsuario()
        {
            manualRepository.AbrirAyudaModUsuario();
        }

        /// <summary>
        /// Abre la ayuda para visualizar usuarios registrados.
        /// </summary>
        public void AbrirAyudaMostrarUsuario()
        {
            manualRepository.AbrirAyudaMostrarUsuario();
        }

        #endregion

        #region Ayuda Roles y Permisos

        /// <summary>
        /// Abre la ayuda para asignar roles.
        /// </summary>
        public void AbrirAyudaAsignarRol()
        {
            manualRepository.AbrirAyudaAsignarRol();
        }

        /// <summary>
        /// Muestra la ayuda para crear roles.
        /// </summary>
        public void AbrirAyudaCrearRol()
        {
            manualRepository.AbrirAyudaCrearRol();
        }

        /// <summary>
        /// Abre la ayuda para crear patentes.
        /// </summary>
        public void AbrirAyudaCrearPatente()
        {
            manualRepository.AbrirAyudaCrearPatente();
        }

        #endregion

        #region Ayuda Backup y Restore

        /// <summary>
        /// Muestra la ayuda sobre generación de respaldos.
        /// </summary>
        public void AbrirAyudaBackUp()
        {
            manualRepository.AbrirAyudaBackUp();
        }

        /// <summary>
        /// Abre la ayuda para restauración de respaldos.
        /// </summary>
        public void AbrirAyudaRestore()
        {
            manualRepository.AbrirAyudaRestore();
        }

        #endregion

        #region Ayuda Bitácora

        /// <summary>
        /// Presenta la ayuda para consultar bitácoras.
        /// </summary>
        public void AbrirAyudaBitacora()
        {
            manualRepository.AbrirAyudaBitacora();
        }

        #endregion

        #region Ayuda Clientes

        /// <summary>
        /// Abre la ayuda para crear clientes.
        /// </summary>
        public void AbrirAyudaCrearCliente()
        {
            manualRepository.AbrirAyudaCrearCliente();
        }

        /// <summary>
        /// Muestra la ayuda para modificar clientes.
        /// </summary>
        public void AbrirAyudaModificarCliente()
        {
            manualRepository.AbrirAyudaModificarCliente();
        }

        /// <summary>
        /// Despliega la ayuda para visualizar clientes.
        /// </summary>
        public void AbrirAyudaMostrarClientes()
        {
            manualRepository.AbrirAyudaMostrarClientes();
        }

        #endregion

        #region Ayuda Productos

        /// <summary>
        /// Abre la ayuda para crear productos.
        /// </summary>
        public void AbrirAyudaCrearProducto()
        {
            manualRepository.AbrirAyudaCrearProducto();
        }

        /// <summary>
        /// Muestra la ayuda para modificar productos.
        /// </summary>
        public void AbrirAyudaModificarProducto()
        {
            manualRepository.AbrirAyudaModificarProducto();
        }

        /// <summary>
        /// Despliega la ayuda para visualizar productos.
        /// </summary>
        public void AbrirAyudaMostrarProductos()
        {
            manualRepository.AbrirAyudaMostrarProductos();
        }

        /// <summary>
        /// Abre la ayuda para eliminar productos.
        /// </summary>
        public void AbrirAyudaEliminarProducto()
        {
            manualRepository.AbrirAyudaEliminarProducto();
        }

        #endregion

        #region Ayuda Proveedores

        /// <summary>
        /// Abre la ayuda para crear proveedores.
        /// </summary>
        public void AbrirAyudaCrearProveedor()
        {
            manualRepository.AbrirAyudaCrearProveedor();
        }

        /// <summary>
        /// Muestra la ayuda para modificar proveedores.
        /// </summary>
        public void AbrirAyudaModificarProveedor()
        {
            manualRepository.AbrirAyudaModificarProveedor();
        }

        /// <summary>
        /// Despliega la ayuda para visualizar proveedores.
        /// </summary>
        public void AbrirAyudaMostrarProveedores()
        {
            manualRepository.AbrirAyudaMostrarProveedores();
        }

        #endregion

        #region Ayuda Stock

        /// <summary>
        /// Muestra la ayuda para visualizar el stock.
        /// </summary>
        public void AbrirAyudaMostrarStock()
        {
            manualRepository.AbrirAyudaMostrarStock();
        }

        /// <summary>
        /// Abre la ayuda para modificar el stock.
        /// </summary>
        public void AbrirAyudaModificarStock()
        {
            manualRepository.AbrirAyudaModificarStock();
        }

        #endregion

        #region Ayuda Pedidos

        /// <summary>
        /// Abre la ayuda para crear pedidos.
        /// </summary>
        public void AbrirAyudaCrearPedido()
        {
            manualRepository.AbrirAyudaCrearPedido();
        }

        /// <summary>
        /// Muestra la ayuda para modificar pedidos.
        /// </summary>
        public void AbrirAyudaModificarPedido()
        {
            manualRepository.AbrirAyudaModificarPedido();
        }

        /// <summary>
        /// Despliega la ayuda para visualizar pedidos.
        /// </summary>
        public void AbrirAyudaMostrarPedidos()
        {
            manualRepository.AbrirAyudaMostrarPedidos();
        }

        /// <summary>
        /// Abre la ayuda para visualizar el detalle de un pedido.
        /// </summary>
        public void AbrirAyudaDetallePedido()
        {
            manualRepository.AbrirAyudaDetallePedido();
        }

        #endregion

        #region Ayuda Reportes

        /// <summary>
        /// Muestra la ayuda para el reporte de stock bajo.
        /// </summary>
        public void AbrirAyudaReporteStockBajo()
        {
            manualRepository.AbrirAyudaReporteStockBajo();
        }

        /// <summary>
        /// Despliega la ayuda para el reporte de productos más vendidos.
        /// </summary>
        public void AbrirAyudaReporteProductosMasVendidos()
        {
            manualRepository.AbrirAyudaReporteProductosMasVendidos();
        }

        #endregion
    }
}
