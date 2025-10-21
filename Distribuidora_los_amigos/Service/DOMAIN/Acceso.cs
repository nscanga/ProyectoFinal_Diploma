using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DOMAIN
{
    /// <summary>
    /// Componente base del patrón Composite para representar accesos del sistema.
    /// </summary>
    public abstract class Acceso
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }

        /// <summary>
        /// Inicializa el acceso generando un identificador único.
        /// </summary>
        public Acceso()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Agrega un acceso hijo al componente compuesto.
        /// </summary>
        public abstract void Add(Acceso component);
        /// <summary>
        /// Elimina un acceso hijo del componente compuesto.
        /// </summary>
        public abstract void Remove(Acceso component);
        /// <summary>
        /// Obtiene la cantidad de elementos contenidos.
        /// </summary>
        public abstract int GetCount();
    }
}
