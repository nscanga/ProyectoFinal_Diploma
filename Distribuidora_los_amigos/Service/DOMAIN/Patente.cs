using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DOMAIN
{
    /// <summary>
    /// Representa un permiso indivisible dentro del sistema.
    /// </summary>
    public class Patente : Acceso
    {

        public TipoAcceso TipoAcceso { get; set; }

        public string DataKey { get; set; }

        /// <summary>
        /// Inicializa la patente indicando el tipo de acceso que representa.
        /// </summary>
        public Patente(TipoAcceso tipoAcceso = TipoAcceso.UI)
        {
            this.TipoAcceso = tipoAcceso;
        }

        ///
        /// <param name="component"></param>
        /// <summary>
        /// Operación no permitida en hojas del patrón Composite.
        /// </summary>
        public override void Add(Acceso component)
        {

            throw new Exception("No se puede agregar un elemento");

        }

        ///
        /// <param name="component"></param>
        /// <summary>
        /// Operación no permitida en hojas del patrón Composite.
        /// </summary>
        public override void Remove(Acceso component)
        {

            throw new Exception("No se puede quitar un elemento");

        }

        /// <summary>
        /// Indica que la patente no contiene elementos hijos.
        /// </summary>
        public override int GetCount()
        {
            return 0;
        }
    }

}
