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
    public static class AccesoService
    {   
    public static void ConfigureMenuItems(ToolStripMenuItem administradorToolStripMenuItem, List<Patente> patentesUsuario)
    {
    // Crear un decorador que solo permita el acceso si es tipo Control
    var menuItemDecorator = new MenuItemDecorator(administradorToolStripMenuItem, TipoAcceso.Control);
    menuItemDecorator.SetAccess(patentesUsuario);
    }

       
    }
}

