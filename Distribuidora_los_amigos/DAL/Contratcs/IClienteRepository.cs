using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contracts;
using DOMAIN;

namespace DAL.Contratcs
{
    public interface IClienteRepository : IGenericServiceDAL<Cliente>
    {
        List<Cliente> GetClientesActivos(); //Obtengo solo clientes activos
    }
}
