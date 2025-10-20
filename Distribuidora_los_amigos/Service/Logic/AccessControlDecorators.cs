using Service.DAL.Contracts;
using Service.DOMAIN;
using Services.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Service.Logic
{
    /// <summary>
    /// Decorador base para controles de acceso
    /// </summary>
    public abstract class AccessControlDecorator : IControlAccess
    {
        protected readonly IControlAccess _component;
        protected readonly TipoAcceso _requiredAccess;

        protected AccessControlDecorator(IControlAccess component, TipoAcceso requiredAccess)
        {
            _component = component;
            _requiredAccess = requiredAccess;
        }

        public virtual void SetAccess(List<Patente> patentesUsuario)
        {
            _component?.SetAccess(patentesUsuario);
        }
    }

    /// <summary>
    /// Control básico que implementa IControlAccess
    /// </summary>
    public class BasicControlAccess : IControlAccess
    {
        private readonly Control _control;

        public BasicControlAccess(Control control)
        {
            _control = control;
        }

        public void SetAccess(List<Patente> patentesUsuario)
        {
            // Comportamiento base - siempre visible si no hay restricciones
            _control.Visible = true;
        }
    }

    /// <summary>
    /// Decorador específico para controles generales
    /// </summary>
    public class ControlAccessDecorator : AccessControlDecorator
    {
        private readonly Control _control;

        public ControlAccessDecorator(Control control, TipoAcceso requiredAccess) 
            : base(new BasicControlAccess(control), requiredAccess)
        {
            _control = control;
        }

        public override void SetAccess(List<Patente> patentesUsuario)
        {
            var hasAccess = patentesUsuario.Any(p => p.TipoAcceso == _requiredAccess);
            _control.Visible = hasAccess;
            
            // Llamar al componente base si es necesario
            base.SetAccess(patentesUsuario);
        }
    }

    /// <summary>
    /// Decorador específico para elementos de menú
    /// </summary>
    public class MenuItemAccessDecorator : AccessControlDecorator
    {
        private readonly ToolStripMenuItem _menuItem;

        public MenuItemAccessDecorator(ToolStripMenuItem menuItem, TipoAcceso requiredAccess)
            : base(null, requiredAccess)
        {
            _menuItem = menuItem;
        }

        public override void SetAccess(List<Patente> patentesUsuario)
        {
            var hasAccess = patentesUsuario.Any(p => p.TipoAcceso == _requiredAccess && _requiredAccess == TipoAcceso.Control);
            _menuItem.Visible = hasAccess;
            
            base.SetAccess(patentesUsuario);
        }
    }

    /// <summary>
    /// Decorador que agrega logging a las verificaciones de acceso
    /// </summary>
    public class LoggingAccessDecorator : AccessControlDecorator
    {
        private readonly string _controlName;

        public LoggingAccessDecorator(IControlAccess component, TipoAcceso requiredAccess, string controlName)
            : base(component, requiredAccess)
        {
            _controlName = controlName;
        }

        public override void SetAccess(List<Patente> patentesUsuario)
        {
            var hasAccess = patentesUsuario.Any(p => p.TipoAcceso == _requiredAccess);
            
            // Log del acceso
            LoggerService.WriteLog(
                $"Control '{_controlName}' - Acceso {(_requiredAccess)} - Resultado: {(hasAccess ? "Permitido" : "Denegado")}", 
                System.Diagnostics.TraceLevel.Info);

            base.SetAccess(patentesUsuario);
        }
    }
}