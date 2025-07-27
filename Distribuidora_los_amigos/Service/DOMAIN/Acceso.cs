using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DOMAIN
{
    public abstract class Acceso
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }

        public Acceso()
        {
            Id = Guid.NewGuid();
        }

        public abstract void Add(Acceso component);
        public abstract void Remove(Acceso component);
        public abstract int GetCount();
    }
}
