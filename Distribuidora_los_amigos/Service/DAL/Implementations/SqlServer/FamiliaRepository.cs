using Service.DAL.Contracts;
using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Implementations.SqlServer.Helpers
{
    /// <summary>
    /// Repositorio encargado de administrar familias y sus patentes relacionadas en la base de datos.
    /// </summary>
    public class FamiliaRepository : IFamiliaRepository
    {
        /// <summary>
        /// Obtiene todas las patentes asociadas a una familia específica.
        /// </summary>
        /// <param name="familiaId">Identificador de la familia.</param>
        /// <returns>Lista de patentes vinculadas.</returns>
        public List<Patente> GetPatentesByFamiliaId(Guid familiaId)
        {
            List<Patente> patentes = new List<Patente>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(
                "SELECT p.IdPatente, p.Nombre, p.DataKey, p.TipoAcceso FROM Patente p " +
                "INNER JOIN Familia_Patente fp ON p.IdPatente = fp.IdPatente " +
                "WHERE fp.IdFamilia = @IdFamilia",
                CommandType.Text,
                new SqlParameter("@IdFamilia", familiaId)))
            {
                while (reader.Read())
                {
                    patentes.Add(new Patente
                    {
                        Id = reader.GetGuid(0),
                        Nombre = reader.GetString(1),
                        DataKey = reader.GetString(2),
                        TipoAcceso = (TipoAcceso)reader.GetInt32(3)
                    });
                }
            }

            return patentes;
        }
        /// <summary>
        /// Crea una nueva familia y vincula sus patentes.
        /// </summary>
        /// <param name="familia">Familia a persistir.</param>
        public void CreateFamilia(Familia familia)
        {
            SqlHelper.ExecuteNonQuery(
                "INSERT INTO Familia (IdFamilia, Nombre) VALUES (@IdFamilia, @Nombre)",
                CommandType.Text,
                new SqlParameter("@IdFamilia", familia.Id),
                new SqlParameter("@Nombre", familia.Nombre)
            );

            foreach (var patente in familia.Accesos.OfType<Patente>())
            {
                SqlHelper.ExecuteNonQuery(
                    "INSERT INTO Familia_Patente (IdFamilia, IdPatente) VALUES (@IdFamilia, @IdPatente)",
                    CommandType.Text,
                    new SqlParameter("@IdFamilia", familia.Id),
                    new SqlParameter("@IdPatente", patente.Id)
                );
            }
        }

        /// <summary>
        /// Actualiza el nombre de la familia y redefine sus patentes asociadas.
        /// </summary>
        /// <param name="familia">Familia con los datos actualizados.</param>
        public void UpdateFamilia(Familia familia)
        {
            // Actualizar el nombre de la familia
            SqlHelper.ExecuteNonQuery(
                "UPDATE Familia SET Nombre = @Nombre WHERE IdFamilia = @IdFamilia",
                CommandType.Text,
                new SqlParameter("@IdFamilia", familia.Id),
                new SqlParameter("@Nombre", familia.Nombre)
            );

            // Eliminar patentes actuales de la familia en Familia_Patente
            SqlHelper.ExecuteNonQuery(
                "DELETE FROM Familia_Patente WHERE IdFamilia = @IdFamilia",
                CommandType.Text,
                new SqlParameter("@IdFamilia", familia.Id)
            );

            // Reinsertar las patentes actualizadas
            foreach (var patente in familia.Accesos.OfType<Patente>())
            {
                SqlHelper.ExecuteNonQuery(
                    "INSERT INTO Familia_Patente (IdFamilia, IdPatente) VALUES (@IdFamilia, @IdPatente)",
                    CommandType.Text,
                    new SqlParameter("@IdFamilia", familia.Id),
                    new SqlParameter("@IdPatente", patente.Id)
                );
            }
        }

        /// <summary>
        /// Asocia una familia a un usuario determinado.
        /// </summary>
        /// <param name="IdUsuario">Identificador del usuario.</param>
        /// <param name="IdFamilia">Identificador de la familia.</param>
        public void SaveUsuarioFamilia(Guid IdUsuario, Guid IdFamilia)
        {
            SqlHelper.ExecuteNonQuery(
                "INSERT INTO Usuario_Familia (IdUsuario, IdFamilia) VALUES (@IdUsuario, @IdFamilia)",
                CommandType.Text,
                new SqlParameter("@IdUsuario", IdUsuario),
                new SqlParameter("@IdFamilia", IdFamilia)
            );
        }
        /// <summary>
        /// Indica si un usuario tiene al menos una familia asignada.
        /// </summary>
        /// <param name="idUsuario">Usuario a evaluar.</param>
        /// <returns><c>true</c> cuando existen asociaciones.</returns>
        public bool ExisteFamiliaParaUsuario(Guid idUsuario)
        {
            var query = "SELECT COUNT(*) FROM Usuario_Familia WHERE IdUsuario = @IdUsuario";
            var count = (int)SqlHelper.ExecuteScalar(query, CommandType.Text, new SqlParameter("@IdUsuario", idUsuario));
            return count > 0;
        }
        /// <summary>
        /// Reemplaza todas las familias asignadas a un usuario por un nuevo conjunto.
        /// </summary>
        /// <param name="usuarioId">Usuario a actualizar.</param>
        /// <param name="familias">Familias que quedarán vinculadas.</param>
        public void UpdateUsuarioFamilia(Guid usuarioId, List<Familia> familias)
        {
            // Eliminar relaciones actuales
            SqlHelper.ExecuteNonQuery(
                "DELETE FROM Usuario_Familia WHERE IdUsuario = @IdUsuario",
                CommandType.Text,
                new SqlParameter("@IdUsuario", usuarioId)
            );

            // Reinsertar relaciones actualizadas
            foreach (var familia in familias)
            {
                SaveUsuarioFamilia(usuarioId, familia.Id);
            }
        }
        /// <summary>
        /// Obtiene todas las familias disponibles.
        /// </summary>
        /// <returns>Lista de familias.</returns>
        public List<Familia> GetAll()
        {
            List<Familia> familias = new List<Familia>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(
                "SELECT IdFamilia, Nombre FROM Familia",
                CommandType.Text))
            {
                while (reader.Read())
                {
                    familias.Add(new Familia
                    {
                        Id = reader.GetGuid(0),
                        Nombre = reader.GetString(1)
                    });
                }
            }

            return familias;
        }
        /// <summary>
        /// Devuelve todas las patentes registradas en el sistema.
        /// </summary>
        /// <returns>Listado de patentes.</returns>
        public List<Patente> GetAllPatentes()
        {
            List<Patente> patentes = new List<Patente>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(
                "SELECT IdPatente, Nombre, DataKey, TipoAcceso FROM Patente",
                CommandType.Text))
            {
                while (reader.Read())
                {
                    patentes.Add(new Patente
                    {
                        Id = reader.GetGuid(0),
                        Nombre = reader.GetString(1),
                        DataKey = reader.GetString(2),
                        TipoAcceso = (TipoAcceso)reader.GetInt32(3)
                    });
                }
            }

            return patentes;
        }
        /// <summary>
        /// Verifica si una familia existe buscando por nombre.
        /// </summary>
        /// <param name="nombreFamilia">Nombre a comprobar.</param>
        /// <returns><c>true</c> si se encuentra la familia.</returns>
        public bool ExisteFamilia(string nombreFamilia)
        {
            var query = "SELECT COUNT(*) FROM Familia WHERE Nombre = @Nombre";
            var count = (int)SqlHelper.ExecuteScalar(query, CommandType.Text, new SqlParameter("@Nombre", nombreFamilia));
            return count > 0;
        }

    }
}
