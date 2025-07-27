using Service.DAL.Implementations.SqlServer.Helpers;
using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Logic
{
    public class FamiliaLogic
    {
        private readonly FamiliaRepository _familiaRepository;

        public FamiliaLogic()
        {
            _familiaRepository = new FamiliaRepository();
        }

        public void CrearFamiliaConPatentes(Familia familia)
        {
            _familiaRepository.CreateFamilia(familia);
        }

        public void AsignarFamiliaAUsuario(Guid usuarioId, Familia familia)
        {
            _familiaRepository.SaveUsuarioFamilia(usuarioId, familia.Id);
        }

        public void ActualizarFamilia(Familia familia)
        {
            _familiaRepository.UpdateFamilia(familia);
        }

        public void ActualizarFamiliasDeUsuario(Guid usuarioId, List<Familia> familias)
        {
            _familiaRepository.UpdateUsuarioFamilia(usuarioId, familias);
        }
        public List<Familia> GetAllFamilias()
        {
            return _familiaRepository.GetAll();
        }
        public List<Patente> GetAllPatentes()
        {
            return _familiaRepository.GetAllPatentes();
        }

    }
}
