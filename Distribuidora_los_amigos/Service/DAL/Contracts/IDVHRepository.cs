using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Contracts
{
    public interface IDVHRepository
    {
        void GuardarDVH(Guid idRegistro, string nombreTabla, string dvh);
        void ActualizarDVH(Guid idRegistro, string nombreTabla, string dvh);

        string ObtenerCodigoVerificador(Guid idRegistro);

        void GuardarDVHCitaDetalle(Guid idCitaDetalle, string nombreTabla ,string dvh);

        string ObtenerCodigoVerificadorCitaDetalle(Guid idCitaDetalle);
    }
}
