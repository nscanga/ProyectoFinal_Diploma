using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Contracts
{
    /// <summary>
    /// Define operaciones CRUD genéricas para las entidades de la capa de datos.
    /// </summary>
    public interface IGenericServiceDAL<T>
    {
        //Pensamos un CRUD para cualquier entidad
        /// <summary>
        /// Inserta una nueva entidad en el origen de datos.
        /// </summary>
        /// <param name="obj">Entidad a agregar.</param>
        void Add(T obj);

        /// <summary>
        /// Actualiza la información de una entidad existente.
        /// </summary>
        /// <param name="obj">Entidad con los cambios aplicados.</param>
        void Update(T obj);

        /// <summary>
        /// Elimina una entidad a partir de su identificador.
        /// </summary>
        void Remove(Guid id);

        /// <summary>
        /// Recupera una entidad por su identificador.
        /// </summary>
        T GetById(Guid id);

        /// <summary>
        /// Devuelve todas las entidades disponibles.
        /// </summary>
        List<T> GetAll();
    }
}
