using Service.DAL.Contracts;
using Service.DAL.Implementations.SqlServer.Helpers;
using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Logic
{
    /// <summary>
    /// Expone operaciones de negocio relacionadas con la bitácora del sistema.
    /// </summary>
    public class BitacoraLogic
    {

        private readonly IBitacoraRepository _bitacoraRepository;
        /// <summary>
        /// Crea la lógica de bitácora con el repositorio indicado.
        /// </summary>
        /// <param name="bitacoraRepository">Repositorio encargado de las lecturas.</param>
        public BitacoraLogic(IBitacoraRepository bitacoraRepository)
        {
            _bitacoraRepository = bitacoraRepository;
        }
        /// <summary>
        /// Recupera todos los registros de bitácora disponibles.
        /// </summary>
        public List<Bitacora> GetAllBitacora()
        {
            return _bitacoraRepository.GetAll();
        }


    }
}
