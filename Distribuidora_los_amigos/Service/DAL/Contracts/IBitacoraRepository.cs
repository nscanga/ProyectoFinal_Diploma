using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Contracts
{
    /// <summary>
    /// Expone los métodos para consultar registros de bitácora.
    /// </summary>
    public interface IBitacoraRepository
    {
        /// <summary>
        /// Obtiene todas las entradas de bitácora registradas.
        /// </summary>
        List<Bitacora> GetAll();
    }
}
