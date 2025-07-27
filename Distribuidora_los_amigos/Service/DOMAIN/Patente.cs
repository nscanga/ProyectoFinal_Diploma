using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DOMAIN
{
    public class Patente : Acceso
    {

        public TipoAcceso TipoAcceso { get; set; }

        public string DataKey { get; set; }

        public Patente(TipoAcceso tipoAcceso = TipoAcceso.UI)
        {
            this.TipoAcceso = tipoAcceso;
        }

        /// 
        /// <param name="component"></param>
        public override void Add(Acceso component)
        {

            throw new Exception("No se puede agregar un elemento");

        }

        /// 
        /// <param name="component"></param>
        public override void Remove(Acceso component)
        {

            throw new Exception("No se puede quitar un elemento");

        }

        public override int GetCount()
        {
            return 0;
        }
    }

}
