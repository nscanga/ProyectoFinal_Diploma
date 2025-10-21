using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Contracts
{
    /// <summary>
    /// Contrato para componentes que definen permisos de acceso.
    /// </summary>
    public interface IControlAccess
    {
        /// <summary>
        /// Configura las patentes habilitadas para el usuario actual.
        /// </summary>
        /// <param name="patentesUsuario">Colección de patentes asignadas.</param>
        void SetAccess(List<Patente> patentesUsuario);
    }
}
