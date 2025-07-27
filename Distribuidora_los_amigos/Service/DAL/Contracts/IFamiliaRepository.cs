using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Contracts
{
    public interface IFamiliaRepository
    {
        List<Patente> GetPatentesByFamiliaId(Guid familiaId);

        void CreateFamilia(Familia familia);

        void UpdateFamilia(Familia familia);

        void SaveUsuarioFamilia(Guid idUsuario, Guid idFamilia);

        void UpdateUsuarioFamilia(Guid usuarioId, List<Familia> familias);

        List<Familia> GetAll();

        List<Patente> GetAllPatentes();

        bool ExisteFamilia(string nombreFamilia);
        bool ExisteFamiliaParaUsuario(Guid idUsuario);
    }

}
