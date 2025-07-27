using Service.DAL.Contracts;
using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service.Logic
{
    public static class AccesoLogic
    {
        //El decorador encapsula la lógica para habilitar o deshabilitar según las patentes del usuario, agrra la interfaz IControlAcces y la implementa en la clase Decorador
        //crear decoradores para varios tipos de controles, como botones, checkboxes, Menus.
        /* ControlDecorator puede manejar cualquier control que herede de Control (por ejemplo, Button, CheckBox, TextBox, etc.).
        El método SetAccess habilita o deshabilita el control dependiendo de si el usuario tiene la patente requerida.*/

        //Decorador para los controles
        public class ControlDecorator : IControlAccess
        {
            private readonly Control _control;
            private readonly TipoAcceso _requiredAccess;

            public ControlDecorator(Control control, TipoAcceso requiredAccess)
            {
                _control = control;
                _requiredAccess = requiredAccess;
            }

            public void SetAccess(List<Patente> patentesUsuario)
            {
                // Verificar si el usuario tiene el tipo de acceso requerido
                var hasAccess = patentesUsuario.Any(p => p.TipoAcceso == _requiredAccess);

                // Mostrar o esconder el control basado en el tipo de acceso
                _control.Visible = hasAccess;
            }
        }

        //Decorador para los item de tippo MENU
        public class MenuItemDecorator : IControlAccess
        {
            private readonly ToolStripMenuItem _menuItem;
            private readonly TipoAcceso _requiredAccess;

            public MenuItemDecorator(ToolStripMenuItem menuItem, TipoAcceso requiredAccess)
            {
                _menuItem = menuItem;
                _requiredAccess = requiredAccess;
            }

            public void SetAccess(List<Patente> patentesUsuario)
            {
                // Verificar si el usuario tiene el tipo de acceso `Control` y NO `UI`
                var hasAccess = patentesUsuario.Any(p => p.TipoAcceso == _requiredAccess && _requiredAccess == TipoAcceso.Control);

                // Mostrar o esconder el menú basado en el tipo de acceso
                _menuItem.Visible = hasAccess;
            }
        }

    }
}
