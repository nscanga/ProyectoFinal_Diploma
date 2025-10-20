using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Contracts;
using DAL.Contratcs;
using DOMAIN;

namespace DAL.Implementations.SqlServer
{
    public class SqlStockRepository : IStockRepository
    {
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

        public void Remove(Guid idStock)
        {
            string query = "DELETE FROM Stock WHERE IdStock = @IdStock";

            SqlParameter[] parameters = { new SqlParameter("@IdStock", idStock) };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

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

        public void EliminarStockPorProducto(Guid idProducto)
        {
            string query = "DELETE FROM Stock WHERE IdProducto = @IdProducto";

            SqlParameter[] parameters = {
            new SqlParameter("@IdProducto", idProducto)
        };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }


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
