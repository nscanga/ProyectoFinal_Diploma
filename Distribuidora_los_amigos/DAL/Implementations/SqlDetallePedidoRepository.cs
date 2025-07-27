using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Contracts;
using DAL.Contratcs;
using DOMAIN;

namespace DAL.Implementations.SqlServer
{
    public class SqlDetallePedidoRepository : IDetallePedidoRepository
    {
        public void Add(DetallePedido detalle)
        {
            string query = @"
                INSERT INTO DetallePedido (IdDetallePedido, IdPedido, IdProducto, Cantidad, PrecioUnitario, Subtotal
) 
                VALUES (@IdDetallePedido, @IdPedido, @IdProducto, @Cantidad, @PrecioUnitario, @Subtotal)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdDetallePedido", detalle.IdDetallePedido),
                new SqlParameter("@IdPedido", detalle.IdPedido),
                new SqlParameter("@IdProducto", detalle.IdProducto),
                new SqlParameter("@Cantidad", detalle.Cantidad),
                new SqlParameter("@PrecioUnitario", detalle.PrecioUnitario),
                new SqlParameter("@Subtotal", detalle.Subtotal)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        public void Update(DetallePedido detalle)
        {
            string query = @"
                UPDATE DetallePedido 
                SET Cantidad = @Cantidad, 
                    PrecioUnitario = @PrecioUnitario, 
                    Subtotal = @Subtotal
                WHERE IdDetallePedido = @IdDetallePedido";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdDetallePedido", detalle.IdDetallePedido),
                new SqlParameter("@Cantidad", detalle.Cantidad),
                new SqlParameter("@PrecioUnitario", detalle.PrecioUnitario),
                new SqlParameter("@Subtotal", detalle.Subtotal)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        public void Remove(Guid id)
        {
            string query = "DELETE FROM DetallePedido WHERE IdDetallePedido = @IdDetallePedido";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdDetallePedido", id)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        public DetallePedido GetById(Guid id)
        {
            string query = "SELECT * FROM DetallePedido WHERE IdDetallePedido = @IdDetallePedido";

            SqlParameter[] parameters = { new SqlParameter("@IdDetallePedido", id) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text, parameters))
            {
                if (reader.Read())
                {
                    return new DetallePedido()
                    {
                        IdDetallePedido = (Guid)reader["IdDetallePedido"],
                        IdPedido = (Guid)reader["IdPedido"],
                        IdProducto = (Guid)reader["IdProducto"],
                        Cantidad = (int)reader["Cantidad"],
                        PrecioUnitario = (decimal)reader["PrecioUnitario"],
                        Subtotal = (decimal)reader["Subtotal"]
                    };
                }
            }
            return null;
        }

        public List<DetallePedido> GetByPedido(Guid idPedido)
        {
            List<DetallePedido> detalles = new List<DetallePedido>();
            string query = "SELECT * FROM DetallePedido WHERE IdPedido = @IdPedido";

            SqlParameter[] parameters = { new SqlParameter("@IdPedido", idPedido) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text, parameters))
            {
                while (reader.Read())
                {
                    detalles.Add(new DetallePedido
                    {
                        IdDetallePedido = (Guid)reader["IdDetallePedido"],
                        IdPedido = (Guid)reader["IdPedido"],
                        IdProducto = (Guid)reader["IdProducto"],
                        Cantidad = (int)reader["Cantidad"],
                        PrecioUnitario = (decimal)reader["PrecioUnitario"],
                        Subtotal = (decimal)reader["Subtotal"]
                    });
                }
            }
            return detalles;
        }

        public List<DetallePedido> GetAll()
        {
            List<DetallePedido> detalles = new List<DetallePedido>();

            string query = "SELECT * FROM DetallePedido";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text))
            {
                while (reader.Read())
                {
                    detalles.Add(new DetallePedido()
                    {
                        IdDetallePedido = (Guid)reader["IdDetallePedido"],
                        IdPedido = (Guid)reader["IdPedido"],
                        IdProducto = (Guid)reader["IdProducto"],
                        Cantidad = (int)reader["Cantidad"],
                        PrecioUnitario = (decimal)reader["PrecioUnitario"],
                        Subtotal = (decimal)reader["Subtotal"]
                    });
                }
            }

            return detalles;
        }

        public List<DetallePedido> ObtenerDetallesPorPedido(Guid idPedido)
        {
            List<DetallePedido> detalles = new List<DetallePedido>();

            string query = "SELECT * FROM DetallePedido WHERE IdPedido = @IdPedido";

            SqlParameter[] parameters = { new SqlParameter("@IdPedido", idPedido) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text, parameters))
            {
                while (reader.Read())
                {
                    detalles.Add(new DetallePedido()
                    {
                        IdDetallePedido = (Guid)reader["IdDetallePedido"],
                        IdPedido = (Guid)reader["IdPedido"],
                        IdProducto = (Guid)reader["IdProducto"],
                        Cantidad = (int)reader["Cantidad"],
                        PrecioUnitario = (decimal)reader["PrecioUnitario"],
                        Subtotal = (decimal)reader["Subtotal"]
                    });
                }
            }

            return detalles;
        }

        public List<DetallePedido> GetDetallesPorPedido(Guid idPedido)
        {
            throw new NotImplementedException();
        }
    }
}
