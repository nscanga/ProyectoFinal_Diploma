using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Service.Logic.AccesoLogic;

namespace Service.Facade
{
    /// <summary>
    /// Expone utilidades para configurar la visibilidad de elementos según patentes del usuario.
    /// </summary>
    public static class AccesoService
    {
        /// <summary>
        /// Configura la visibilidad del menú de administración según las patentes del usuario.
        /// </summary>
        /// <param name="administradorToolStripMenuItem">Elemento de menú a proteger.</param>
        /// <param name="patentesUsuario">Patentes asociadas al usuario.</param>
        public static void ConfigureMenuItems(ToolStripMenuItem administradorToolStripMenuItem, List<Patente> patentesUsuario)
        {
            // Crear un decorador que solo permita el acceso si es tipo Control
            var menuItemDecorator = new MenuItemDecorator(administradorToolStripMenuItem, TipoAcceso.Control);
            menuItemDecorator.SetAccess(patentesUsuario);
        }

        /// <summary>
        /// Configura la visibilidad de un ítem de menú según una patente específica requerida.
        /// </summary>
        /// <param name="menuItem">Elemento de menú a proteger.</param>
        /// <param name="requiredPatenteName">Nombre exacto de la patente requerida (ej: "Crear_cliente").</param>
        /// <param name="patentesUsuario">Patentes asociadas al usuario.</param>
        public static void ConfigureMenuItemByPatente(ToolStripMenuItem menuItem, string requiredPatenteName, List<Patente> patentesUsuario)
        {
            var decorator = new MenuItemPatenteDecorator(menuItem, requiredPatenteName);
            decorator.SetAccess(patentesUsuario);
        }

        /// <summary>
        /// Configura múltiples ítems de menú de forma automática según un diccionario de nombre-patente.
        /// </summary>
        /// <param name="menuItems">Diccionario donde la clave es el ítem de menú y el valor es el nombre de la patente requerida.</param>
        /// <param name="patentesUsuario">Patentes asociadas al usuario.</param>
        public static void ConfigureMultipleMenuItems(Dictionary<ToolStripMenuItem, string> menuItems, List<Patente> patentesUsuario)
        {
            foreach (var item in menuItems)
            {
                ConfigureMenuItemByPatente(item.Key, item.Value, patentesUsuario);
            }
        }
    }
}

