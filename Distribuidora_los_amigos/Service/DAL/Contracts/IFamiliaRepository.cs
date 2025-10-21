using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Contracts
{
    /// <summary>
    /// Define las operaciones de persistencia para familias y sus patentes asociadas.
    /// </summary>
    public interface IFamiliaRepository
    {
        /// <summary>
        /// Obtiene las patentes vinculadas a una familia.
        /// </summary>
        List<Patente> GetPatentesByFamiliaId(Guid familiaId);

        /// <summary>
        /// Crea una nueva familia.
        /// </summary>
        void CreateFamilia(Familia familia);

        /// <summary>
        /// Actualiza los datos de una familia existente.
        /// </summary>
        void UpdateFamilia(Familia familia);

        /// <summary>
        /// Asocia una familia a un usuario.
        /// </summary>
        void SaveUsuarioFamilia(Guid idUsuario, Guid idFamilia);

        /// <summary>
        /// Reemplaza las familias asignadas a un usuario.
        /// </summary>
        void UpdateUsuarioFamilia(Guid usuarioId, List<Familia> familias);

        /// <summary>
        /// Lista todas las familias.
        /// </summary>
        List<Familia> GetAll();

        /// <summary>
        /// Devuelve todas las patentes disponibles.
        /// </summary>
        List<Patente> GetAllPatentes();

        /// <summary>
        /// Verifica la existencia de una familia por nombre.
        /// </summary>
        bool ExisteFamilia(string nombreFamilia);
        /// <summary>
        /// Indica si un usuario tiene alguna familia asociada.
        /// </summary>
        bool ExisteFamiliaParaUsuario(Guid idUsuario);
    }

}
