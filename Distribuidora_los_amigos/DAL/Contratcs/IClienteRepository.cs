using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contracts;
using DOMAIN;

namespace DAL.Contratcs
{
    /// <summary>
    /// Define operaciones de persistencia especializadas para la entidad <see cref="Cliente"/>.
    /// </summary>
    public interface IClienteRepository : IGenericServiceDAL<Cliente>
    {
        /// <summary>
        /// Recupera la colección de clientes con el flag de activo establecido.
        /// </summary>
        /// <returns>Una lista con los clientes actualmente activos.</returns>
        List<Cliente> GetClientesActivos(); //Obtengo solo clientes activos
    }
}
