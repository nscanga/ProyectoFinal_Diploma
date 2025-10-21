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


    }
}

