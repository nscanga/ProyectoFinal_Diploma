﻿using Service.DAL.Contracts;
using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service.Logic
{
    /// <summary>
    /// Proporciona decoradores para controlar la visibilidad de elementos según las patentes del usuario.
    /// </summary>
    public static class AccesoLogic
    {
        //El decorador encapsula la lógica para habilitar o deshabilitar según las patentes del usuario, agrra la interfaz IControlAcces y la implementa en la clase Decorador
        //crear decoradores para varios tipos de controles, como botones, checkboxes, Menus.
        /* ControlDecorator puede manejar cualquier control que herede de Control (por ejemplo, Button, CheckBox, TextBox, etc.).
        El método SetAccess habilita o deshabilita el control dependiendo de si el usuario tiene la patente requerida.*/

        //Decorador para los controles
        /// <summary>
        /// Decorador para controles estándar que ajusta su visibilidad según un tipo de acceso requerido.
        /// </summary>
        public class ControlDecorator : IControlAccess
        {
            private readonly Control _control;
            private readonly TipoAcceso _requiredAccess;

            /// <summary>
            /// Inicializa el decorador con el control objetivo y el tipo de acceso necesario.
            /// </summary>
            /// <param name="control">Control de Windows Forms a proteger.</param>
            /// <param name="requiredAccess">Tipo de acceso requerido para mostrarlo.</param>
            public ControlDecorator(Control control, TipoAcceso requiredAccess)
            {
                _control = control;
                _requiredAccess = requiredAccess;
            }

            /// <summary>
            /// Evalúa las patentes del usuario y define si el control debe mostrarse.
            /// </summary>
            /// <param name="patentesUsuario">Listado de patentes del usuario autenticado.</param>
            public void SetAccess(List<Patente> patentesUsuario)
            {
                // Verificar si el usuario tiene el tipo de acceso requerido
                var hasAccess = patentesUsuario.Any(p => p.TipoAcceso == _requiredAccess);

                // Mostrar o esconder el control basado en el tipo de acceso
                _control.Visible = hasAccess;
            }
        }

        //Decorador para los item de tippo MENU
        /// <summary>
        /// Decorador específico para ítems de menú que requieren un tipo de acceso determinado.
        /// </summary>
        public class MenuItemDecorator : IControlAccess
        {
            private readonly ToolStripMenuItem _menuItem;
            private readonly TipoAcceso _requiredAccess;

            /// <summary>
            /// Configura el decorador con el ítem de menú objetivo.
            /// </summary>
            /// <param name="menuItem">Elemento de menú a gestionar.</param>
            /// <param name="requiredAccess">Tipo de acceso necesario para visualizarlo.</param>
            public MenuItemDecorator(ToolStripMenuItem menuItem, TipoAcceso requiredAccess)
            {
                _menuItem = menuItem;
                _requiredAccess = requiredAccess;
            }

            /// <summary>
            /// Determina la visibilidad del ítem de menú según las patentes proporcionadas.
            /// </summary>
            /// <param name="patentesUsuario">Patentes del usuario.</param>
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
