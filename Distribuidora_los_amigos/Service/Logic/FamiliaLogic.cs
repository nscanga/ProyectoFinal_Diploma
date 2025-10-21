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
    /// Coordina la creación y mantenimiento de familias y sus relaciones con usuarios.
    /// </summary>
    public class FamiliaLogic
    {
        private readonly FamiliaRepository _familiaRepository;

        /// <summary>
        /// Inicializa la lógica con la implementación por defecto del repositorio de familias.
        /// </summary>
        public FamiliaLogic()
        {
            _familiaRepository = new FamiliaRepository();
        }

        /// <summary>
        /// Crea una familia y registra sus patentes asociadas.
        /// </summary>
        public void CrearFamiliaConPatentes(Familia familia)
        {
            _familiaRepository.CreateFamilia(familia);
        }

        /// <summary>
        /// Vincula una familia existente a un usuario.
        /// </summary>
        public void AsignarFamiliaAUsuario(Guid usuarioId, Familia familia)
        {
            _familiaRepository.SaveUsuarioFamilia(usuarioId, familia.Id);
        }

        /// <summary>
        /// Actualiza los datos y patentes de una familia.
        /// </summary>
        public void ActualizarFamilia(Familia familia)
        {
            _familiaRepository.UpdateFamilia(familia);
        }

        /// <summary>
        /// Reemplaza el conjunto de familias asociadas a un usuario.
        /// </summary>
        public void ActualizarFamiliasDeUsuario(Guid usuarioId, List<Familia> familias)
        {
            _familiaRepository.UpdateUsuarioFamilia(usuarioId, familias);
        }
        /// <summary>
        /// Obtiene todas las familias registradas.
        /// </summary>
        public List<Familia> GetAllFamilias()
        {
            return _familiaRepository.GetAll();
        }
        /// <summary>
        /// Recupera todas las patentes disponibles.
        /// </summary>
        public List<Patente> GetAllPatentes()
        {
            return _familiaRepository.GetAllPatentes();
        }

    }
}
