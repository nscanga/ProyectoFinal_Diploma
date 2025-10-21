using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DOMAIN
{
    /// <summary>
    /// Representa un conjunto de accesos agrupados que implementa el patrón Composite.
    /// </summary>
    public class Familia : Acceso
    {
        private List<Acceso> accesos = new List<Acceso>();

        public string Descripcion { get; set; }

        /// <summary>
        /// Inicializa una familia generando un nuevo identificador y opcionalmente agregando un acceso inicial.
        /// </summary>
        public Familia(Acceso acceso = null)
        {
            Id = Guid.NewGuid();
            if (acceso != null)
                //acceso no debe ser null
                accesos.Add(acceso);
        }

        ///
        /// <param name="component"></param>
        /// <summary>
        /// Agrega un acceso hijo a la familia.
        /// </summary>
        public override void Add(Acceso component)
        {
            accesos.Add(component);
        }

        ///
        /// <param name="component"></param>
        /// <summary>
        /// Elimina un acceso hijo filtrando por identificador.
        /// </summary>
        public override void Remove(Acceso component)
        {
            //Ver que no puedo quedarme sin hijos...

            //accesos.Remove(component);
            accesos.RemoveAll(o => o.Id == component.Id);//Linq -> lambda exp. se ve más adelante
        }

        /// <summary>
        /// Devuelve la cantidad de accesos contenidos en la familia.
        /// </summary>
        public override int GetCount()
        {
            return Accesos.Count;
        }

        /// <summary>
        /// Colección de accesos hijos asociados a la familia.
        /// </summary>
        public List<Acceso> Accesos
        {
            get
            {
                return accesos;
            }
        }
    }
}
