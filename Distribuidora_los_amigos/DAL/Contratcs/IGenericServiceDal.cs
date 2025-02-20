using System;
using System.Collections.Generic;


namespace DAL.Contracts
{
    /// <summary>
    /// Define un conjunto de operaciones genéricas (CRUD) para cualquier entidad.
    /// </summary>
    /// <typeparam name="T">El tipo de entidad para la cual se realizan las operaciones.</typeparam>
    public interface IGenericServiceDAL<T>
    {
        /// <summary>
        /// Agrega una nueva entidad.
        /// </summary>
        /// <param name="obj">La entidad que se va a agregar.</param>
        void Add(T obj);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="obj">La entidad que se va a actualizar.</param>
        void Update(T obj);

        /// <summary>
        /// Elimina una entidad por su identificador único (GUID).
        /// </summary>
        /// <param name="id">El identificador de la entidad a eliminar.</param>
        void Remove(Guid id);

        /// <summary>
        /// Obtiene una entidad por su identificador único (GUID).
        /// </summary>
        /// <param name="id">El identificador de la entidad que se desea obtener.</param>
        /// <returns>La entidad correspondiente al identificador.</returns>
        T GetById(Guid id);

        /// <summary>
        /// Obtiene todas las entidades.
        /// </summary>
        /// <returns>Una lista de todas las entidades de tipo <typeparamref name="T"/>.</returns>
        List<T> GetAll();
    }

}

