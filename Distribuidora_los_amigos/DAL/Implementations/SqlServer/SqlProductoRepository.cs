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
    /// Repositorio SQL Server responsable de la persistencia de productos.
    /// </summary>
    public class SqlProductoRepository : IProductoRepository
    {
        /// <summary>
        /// Inserta un nuevo producto en la base de datos.
        /// </summary>
        /// <param name="producto">Entidad de producto a guardar.</param>
        public void Add(Producto producto)
        {
            string query = @"
                INSERT INTO Producto (IdProducto, Nombre, Precio, Fecha_Ingreso, Vencimiento, Categoria, Activo) 
                VALUES (@IdProducto, @Nombre, @Precio, @FechaIngreso, @Vencimiento, @Categoria, @Activo)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdProducto", producto.IdProducto),
                new SqlParameter("@Nombre", producto.Nombre),
                new SqlParameter("@Precio", producto.Precio),
                new SqlParameter("@FechaIngreso", producto.FechaIngreso),
                new SqlParameter("@Vencimiento", (object)producto.Vencimiento ?? DBNull.Value),
                new SqlParameter("@Categoria", producto.Categoria),
                new SqlParameter("@Activo", producto.Activo)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        /// <summary>
        /// Actualiza los campos editables de un producto.
        /// </summary>
        /// <param name="producto">Entidad con los datos modificados.</param>
        public void Update(Producto producto)
        {
            string query = @"
                UPDATE Producto 
                SET Nombre = @Nombre, 
                    Precio = @Precio, 
                    Fecha_Ingreso = @FechaIngreso, 
                    Vencimiento = @Vencimiento, 
                    Categoria = @Categoria, 
                    Activo = @Activo 
                WHERE IdProducto = @IdProducto";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdProducto", producto.IdProducto),
                new SqlParameter("@Nombre", producto.Nombre),
                new SqlParameter("@Precio", producto.Precio),
                new SqlParameter("@FechaIngreso", producto.FechaIngreso),
                new SqlParameter("@Vencimiento", (object)producto.Vencimiento ?? DBNull.Value),
                new SqlParameter("@Categoria", producto.Categoria),
                new SqlParameter("@Activo", producto.Activo)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        /// <summary>
        /// Deshabilita lógicamente un producto.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        public void Disable(Guid id)
        {
            string query = "UPDATE Producto SET Activo = 0 WHERE IdProducto = @IdProducto";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdProducto", id)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        /// <summary>
        /// Obtiene todos los productos almacenados.
        /// </summary>
        /// <returns>Lista completa de productos.</returns>
        public List<Producto> GetAll()
        {
            List<Producto> productos = new List<Producto>();

            string query = "SELECT * FROM Producto";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text))
            {
                while (reader.Read())
                {
                    productos.Add(new Producto()
                    {
                        IdProducto = (Guid)reader["IdProducto"],
                        Nombre = reader["Nombre"].ToString(),
                        Precio = (decimal)reader["Precio"],
                        FechaIngreso = (DateTime)reader["Fecha_Ingreso"],
                        Vencimiento = reader["Vencimiento"] != DBNull.Value ? (DateTime?)reader["Vencimiento"] : null,
                        Categoria = reader["Categoria"].ToString(),
                        Activo = (bool)reader["Activo"]
                    });
                }
            }

            return productos;
        }

        /// <summary>
        /// Recupera un producto según su identificador único.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>El producto encontrado o <c>null</c> si no existe.</returns>
        public Producto GetById(Guid id)
        {
            string query = "SELECT * FROM Producto WHERE IdProducto = @IdProducto";

            SqlParameter[] parameters = { new SqlParameter("@IdProducto", id) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text, parameters))
            {
                if (reader.Read())
                {
                    return new Producto()
                    {
                        IdProducto = (Guid)reader["IdProducto"],
                        Nombre = reader["Nombre"].ToString(),
                        Precio = (decimal)reader["Precio"],
                        FechaIngreso = (DateTime)reader["Fecha_Ingreso"],
                        Vencimiento = reader["Vencimiento"] != DBNull.Value ? (DateTime?)reader["Vencimiento"] : null,
                        Categoria = reader["Categoria"].ToString(),
                        Activo = (bool)reader["Activo"]
                    };
                }
            }
            return null;
        }


        /// <summary>
        /// Obtiene los productos pertenecientes a una categoría específica.
        /// </summary>
        /// <param name="categoria">Nombre de la categoría.</param>
        /// <returns>Lista de productos filtrados por categoría.</returns>
        public List<Producto> GetByCategoria(string categoria)
        {
            List<Producto> productos = new List<Producto>();

            string query = "SELECT * FROM Producto WHERE Categoria = @Categoria";

            SqlParameter[] parameters = { new SqlParameter("@Categoria", categoria) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text, parameters))
            {
                while (reader.Read())
                {
                    productos.Add(new Producto()
                    {
                        IdProducto = (Guid)reader["IdProducto"],
                        Nombre = reader["Nombre"].ToString(),
                        Precio = (decimal)reader["Precio"],
                        FechaIngreso = (DateTime)reader["Fecha_Ingreso"],
                        Vencimiento = reader["Vencimiento"] != DBNull.Value ? (DateTime?)reader["Vencimiento"] : null,
                        Categoria = reader["Categoria"].ToString(),
                        Activo = (bool)reader["Activo"]
                    });
                }
            }
            return productos;
        }


        /// <summary>
        /// Elimina físicamente un producto del sistema.
        /// </summary>
        /// <param name="id">Identificador del producto a eliminar.</param>
        public void Remove(Guid id)
        {
            string query = "DELETE FROM Producto WHERE IdProducto = @IdProducto";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdProducto", id)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        /// <summary>
        /// Obtiene únicamente los productos activos (pendiente de implementación).
        /// </summary>
        /// <returns>Lista de productos activos.</returns>
        /// <exception cref="NotImplementedException">Siempre, hasta que se implemente.</exception>
        public List<Producto> ObtenerProductosActivos()
        {
            throw new NotImplementedException();
        }
    }
}
