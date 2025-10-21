using Service.DAL.Contracts;
using Service.DAL.Implementations.SqlServer;
using Service.DOMAIN;
using Service.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Facade
{
    /// <summary>
    /// Fachada para consultar registros de bitácora mediante la lógica correspondiente.
    /// </summary>
    public class BitacoraService
    {

        private static BitacoraLogic _bitacoraLogic;


        /// <summary>
        /// Inicializa la lógica de bitácora con su repositorio concreto.
        /// </summary>
        static BitacoraService()
        {

            IBitacoraRepository repository = new BitacoraRepository();
            _bitacoraLogic = new BitacoraLogic(repository);
        }

        /// <summary>
        /// Devuelve todas las entradas de la bitácora.
        /// </summary>
        public static List<Bitacora> GetAllBitacora()
        {
            return _bitacoraLogic.GetAllBitacora();
        }



    }
}
