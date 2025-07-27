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
    public class BitacoraLogic
    {

        private readonly IBitacoraRepository _bitacoraRepository;
        public BitacoraLogic(IBitacoraRepository bitacoraRepository)
        {
            _bitacoraRepository = bitacoraRepository;
        }
        public List<Bitacora> GetAllBitacora()
        {
            return _bitacoraRepository.GetAll();
        }


    }
}
