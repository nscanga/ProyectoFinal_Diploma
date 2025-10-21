using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Contracts
{
    /// <summary>
    /// Contrato para componentes que reaccionan a cambios de idioma.
    /// </summary>
    public interface IIdiomaObserver
    {
        /// <summary>
        /// Ejecuta la actualización del contenido cuando el idioma se modifica.
        /// </summary>
        void UpdateIdioma();
    }
}
