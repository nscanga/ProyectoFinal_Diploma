using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contratcs;
using DOMAIN;

namespace DAL.Implementations.SqlServer
{
    /// <summary>
    /// Implementación SQL Server del repositorio de proveedores.
    /// </summary>
    public class SqlProveedorRepository : IProveedorRepository
    {
        /// <summary>
        /// Inserta un nuevo proveedor en la base de datos.
        /// </summary>
        /// <param name="proveedor">Entidad con la información a registrar.</param>
        public void Add(Proveedor proveedor)
        {
            string query = @"
                INSERT INTO Proveedor (IdProveedor, Nombre, Direccion, Email, Telefono, Categoria, Activo)
                VALUES (@IdProveedor, @Nombre, @Direccion, @Email, @Telefono, @Categoria, @Activo)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdProveedor", proveedor.IdProveedor),
                new SqlParameter("@Nombre", proveedor.Nombre),
                new SqlParameter("@Direccion", proveedor.Direccion),
                new SqlParameter("@Email", proveedor.Email),
                new SqlParameter("@Telefono", proveedor.Telefono),
                new SqlParameter("@Categoria", proveedor.Categoria),
                new SqlParameter("@Activo", proveedor.Activo)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        /// <summary>
        /// Actualiza los datos de un proveedor existente.
        /// </summary>
        /// <param name="proveedor">Entidad con los cambios aplicados.</param>
        public void Update(Proveedor proveedor)
        {
            string query = @"
                UPDATE Proveedor 
                SET Nombre = @Nombre, Direccion = @Direccion, Email = @Email, 
                    Telefono = @Telefono, Categoria = @Categoria, Activo = @Activo 
                WHERE IdProveedor = @IdProveedor";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdProveedor", proveedor.IdProveedor),
                new SqlParameter("@Nombre", proveedor.Nombre),
                new SqlParameter("@Direccion", proveedor.Direccion),
                new SqlParameter("@Email", proveedor.Email),
                new SqlParameter("@Telefono", proveedor.Telefono),
                new SqlParameter("@Categoria", proveedor.Categoria),
                new SqlParameter("@Activo", proveedor.Activo)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        /// <summary>
        /// Elimina físicamente un proveedor mediante su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor.</param>
        public void Remove(Guid id)
        {
            string query = "DELETE FROM Proveedor WHERE IdProveedor = @IdProveedor";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdProveedor", id)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        /// <summary>
        /// Recupera un proveedor por su identificador único.
        /// </summary>
        /// <param name="id">Identificador del proveedor buscado.</param>
        /// <returns>El proveedor encontrado o <c>null</c> si no existe.</returns>
        public Proveedor GetById(Guid id)
        {
            string query = "SELECT * FROM Proveedor WHERE IdProveedor = @IdProveedor";

            SqlParameter[] parameters = { new SqlParameter("@IdProveedor", id) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text, parameters))
            {
                if (reader.Read())
                {
                    return new Proveedor()
                    {
                        IdProveedor = (Guid)reader["IdProveedor"],
                        Nombre = reader["Nombre"].ToString(),
                        Direccion = reader["Direccion"].ToString(),
                        Email = reader["Email"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Categoria = reader["Categoria"].ToString(),
                        Activo = (bool)reader["Activo"]
                    };
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene la lista completa de proveedores.
        /// </summary>
        /// <returns>Colección de proveedores almacenados.</returns>
        public List<Proveedor> GetAll()
        {
            List<Proveedor> proveedores = new List<Proveedor>();

            string query = "SELECT * FROM Proveedor";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text))
            {
                while (reader.Read())
                {
                    proveedores.Add(new Proveedor()
                    {
                        IdProveedor = (Guid)reader["IdProveedor"],
                        Nombre = reader["Nombre"].ToString(),
                        Direccion = reader["Direccion"].ToString(),
                        Email = reader["Email"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Categoria = reader["Categoria"].ToString(),
                        Activo = (bool)reader["Activo"]
                    });
                }
            }

            return proveedores;
        }
    }
}
