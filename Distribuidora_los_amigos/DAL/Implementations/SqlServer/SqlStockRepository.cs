using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Contracts;
using DAL.Contratcs;
using DOMAIN;

namespace DAL.Implementations.SqlServer
{
    /// <summary>
    /// Repositorio SQL Server para las operaciones de inventario.
    /// </summary>
    public class SqlStockRepository : IStockRepository
    {
        /// <summary>
        /// Inserta un registro de stock para un producto.
        /// </summary>
        /// <param name="stock">Entidad de stock a crear.</param>
        public void Add(Stock stock)
        {
            string query = "INSERT INTO Stock (IdStock, IdProducto, Cantidad, Tipo) VALUES (@IdStock, @IdProducto, @Cantidad, @Tipo)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdStock", stock.IdStock),
                new SqlParameter("@IdProducto", stock.IdProducto),
                new SqlParameter("@Cantidad", stock.Cantidad),
                new SqlParameter("@Tipo", stock.Tipo)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        /// <summary>
        /// Actualiza la cantidad o tipo de un registro de stock.
        /// </summary>
        /// <param name="stock">Entidad con los datos actualizados.</param>
        public void Update(Stock stock)
        {
            string query = "UPDATE Stock SET Cantidad = @Cantidad, Tipo = @Tipo WHERE IdStock = @IdStock";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdStock", stock.IdStock),
                new SqlParameter("@Cantidad", stock.Cantidad),
                new SqlParameter("@Tipo", stock.Tipo)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        /// <summary>
        /// Elimina un registro de stock por su identificador.
        /// </summary>
        /// <param name="idStock">Identificador del stock.</param>
        public void Remove(Guid idStock)
        {
            string query = "DELETE FROM Stock WHERE IdStock = @IdStock";

            SqlParameter[] parameters = { new SqlParameter("@IdStock", idStock) };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        /// <summary>
        /// Recupera todos los registros de stock disponibles.
        /// </summary>
        /// <returns>Lista completa de existencias.</returns>
        public List<Stock> GetAll()
        {
            List<Stock> stockList = new List<Stock>();
            string query = "SELECT * FROM Stock";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text))
            {
                while (reader.Read())
                {
                    stockList.Add(new Stock()
                    {
                        IdStock = (Guid)reader["IdStock"],
                        IdProducto = (Guid)reader["IdProducto"],
                        Cantidad = (int)reader["Cantidad"],
                        Tipo = reader["Tipo"].ToString()
                    });
                }
            }
            return stockList;
        }

        /// <summary>
        /// Obtiene un registro de stock por su identificador único.
        /// </summary>
        /// <param name="idStock">Identificador buscado.</param>
        /// <returns>El registro encontrado o <c>null</c> si no existe.</returns>
        public Stock GetById(Guid idStock)
        {
            string query = "SELECT * FROM Stock WHERE IdStock = @IdStock";
            SqlParameter[] parameters = { new SqlParameter("@IdStock", idStock) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text, parameters))
            {
                if (reader.Read())
                {
                    return new Stock()
                    {
                        IdStock = (Guid)reader["IdStock"],
                        IdProducto = (Guid)reader["IdProducto"],
                        Cantidad = (int)reader["Cantidad"],
                        Tipo = reader["Tipo"].ToString()
                    };
                }
            }
            return null;
        }

        /// <summary>
        /// Elimina todos los registros de stock asociados a un producto.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        public void EliminarStockPorProducto(Guid idProducto)
        {
            string query = "DELETE FROM Stock WHERE IdProducto = @IdProducto";

            SqlParameter[] parameters = {
            new SqlParameter("@IdProducto", idProducto)
        };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }


        /// <summary>
        /// Obtiene los registros de stock para un producto determinado.
        /// </summary>
        /// <param name="idProducto">Identificador del producto.</param>
        /// <returns>Lista de existencias del producto.</returns>
        public List<Stock> GetByProducto(Guid idProducto)
        {
            List<Stock> stockList = new List<Stock>();
            string query = "SELECT * FROM Stock WHERE IdProducto = @IdProducto";
            SqlParameter[] parameters = { new SqlParameter("@IdProducto", idProducto) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text, parameters))
            {
                while (reader.Read())
                {
                    stockList.Add(new Stock()
                    {
                        IdStock = (Guid)reader["IdStock"],
                        IdProducto = (Guid)reader["IdProducto"],
                        Cantidad = (int)reader["Cantidad"],
                        Tipo = reader["Tipo"].ToString()
                    });
                }
            }
            return stockList;
        }

        /// <summary>
        /// Resta unidades del stock de un producto con validaciones.
        /// </summary>
        /// <param name="idProducto">Producto sobre el que se descuenta.</param>
        /// <param name="cantidad">Cantidad a restar.</param>
        public void DescontarStock(Guid idProducto, int cantidad)
        {
            try
            {
                string query = @"
                    UPDATE Stock 
                    SET Cantidad = Cantidad - @Cantidad
                    WHERE IdProducto = @IdProducto AND Cantidad >= @Cantidad";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@IdProducto", idProducto),
                    new SqlParameter("@Cantidad", cantidad)
                };

                int rowsAffected = SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
                
                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("No se pudo descontar el stock. Stock insuficiente o producto no encontrado.");
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al descontar stock: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Incrementa la cantidad disponible de un producto.
        /// </summary>
        /// <param name="idProducto">Producto cuyo stock se aumenta.</param>
        /// <param name="cantidad">Unidades a sumar.</param>
        public void AumentarStock(Guid idProducto, int cantidad)
        {
            string query = @"UPDATE Stock 
                             SET Cantidad = Cantidad + @cantidad 
                             WHERE IdProducto = @idProducto";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@idProducto", idProducto),
                new SqlParameter("@cantidad", cantidad)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

    }
}
