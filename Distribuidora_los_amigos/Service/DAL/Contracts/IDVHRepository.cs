using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Contracts
{
    /// <summary>
    /// Define operaciones para administrar dígitos verificadores horizontales.
    /// </summary>
    public interface IDVHRepository
    {
        /// <summary>
        /// Almacena un DVH para el registro indicado.
        /// </summary>
        void GuardarDVH(Guid idRegistro, string nombreTabla, string dvh);
        /// <summary>
        /// Actualiza el DVH de un registro existente.
        /// </summary>
        void ActualizarDVH(Guid idRegistro, string nombreTabla, string dvh);

        /// <summary>
        /// Obtiene el DVH asociado al registro solicitado.
        /// </summary>
        string ObtenerCodigoVerificador(Guid idRegistro);

        /// <summary>
        /// Persiste el DVH correspondiente a un detalle de cita.
        /// </summary>
        void GuardarDVHCitaDetalle(Guid idCitaDetalle, string nombreTabla ,string dvh);

        /// <summary>
        /// Recupera el DVH asociado a un detalle de cita.
        /// </summary>
        string ObtenerCodigoVerificadorCitaDetalle(Guid idCitaDetalle);
    }
}
