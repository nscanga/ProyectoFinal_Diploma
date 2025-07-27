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
    public class BitacoraService
    {

        private static BitacoraLogic _bitacoraLogic;

        
        static BitacoraService()
        {
            
            IBitacoraRepository repository = new BitacoraRepository();
            _bitacoraLogic = new BitacoraLogic(repository);
        }

        public static List<Bitacora> GetAllBitacora()
        {
            return _bitacoraLogic.GetAllBitacora();
        }



    }
}
