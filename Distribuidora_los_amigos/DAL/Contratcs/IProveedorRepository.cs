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
    /// Contrato para las operaciones de datos asociadas a la entidad <see cref="Proveedor"/>.
    /// </summary>
    public interface IProveedorRepository : IGenericServiceDAL<Proveedor>
    {
    }
}
