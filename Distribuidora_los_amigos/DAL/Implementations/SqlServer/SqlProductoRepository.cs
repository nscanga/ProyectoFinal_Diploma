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
    public class SqlProductoRepository : IProductoRepository
    {
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

        public void Disable(Guid id)
        {
            string query = "UPDATE Producto SET Activo = 0 WHERE IdProducto = @IdProducto";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdProducto", id)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

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


        public void Remove(Guid id)
        {
            string query = "DELETE FROM Producto WHERE IdProducto = @IdProducto";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdProducto", id)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        public List<Producto> ObtenerProductosActivos()
        {
            throw new NotImplementedException();
        }
    }
}
