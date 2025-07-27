using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Contracts;
using DAL.Contratcs;
using DOMAIN;

namespace DAL.Implementations.SqlServer
{
    public class SqlPedidoRepository : IPedidoRepository
    {
        public void Add(Pedido pedido)
        {
            string query = @"
                INSERT INTO Pedido (IdPedido, IdCliente, FechaPedido, Total, IdEstadoPedido) 
                VALUES (@IdPedido, @IdCliente, @FechaPedido, @Total, @IdEstadoPedido)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdPedido", pedido.IdPedido),
                new SqlParameter("@IdCliente", pedido.IdCliente),
                new SqlParameter("@FechaPedido", pedido.FechaPedido),
                new SqlParameter("@Total", pedido.Total),
                new SqlParameter("@IdEstadoPedido", pedido.IdEstadoPedido) // Referencia a la tabla EstadoPedido
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        public void Update(Pedido pedido)
        {
            string query = @"
                UPDATE Pedido 
                SET IdEstadoPedido = @IdEstadoPedido, 
                    Total = @Total 
                WHERE IdPedido = @IdPedido";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdPedido", pedido.IdPedido),
                new SqlParameter("@IdEstadoPedido", pedido.IdEstadoPedido), // Se actualiza el estado referenciado
                new SqlParameter("@Total", pedido.Total)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        public void Remove(Guid id)
        {
            string query = "DELETE FROM Pedido WHERE IdPedido = @IdPedido";

            SqlParameter[] parameters = { new SqlParameter("@IdPedido", id) };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        public Pedido GetById(Guid idPedido)
        {
            string query = @"
                SELECT p.*, e.NombreEstado 
                FROM Pedido p
                JOIN EstadoPedido e ON p.IdEstadoPedido = e.IdEstadoPedido
                WHERE p.IdPedido = @IdPedido";

            SqlParameter[] parameters = { new SqlParameter("@IdPedido", idPedido) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text, parameters))
            {
                if (reader.Read())
                {
                    return new Pedido()
                    {
                        IdPedido = (Guid)reader["IdPedido"],
                        IdCliente = (Guid)reader["IdCliente"],
                        FechaPedido = (DateTime)reader["FechaPedido"],
                        IdEstadoPedido = (Guid)reader["IdEstadoPedido"],
                        NombreEstado = reader["NombreEstado"].ToString(), // Obtener nombre del estado
                        Total = (decimal)reader["Total"]
                    };
                }
            }
            return null;
        }

        public List<Pedido> GetAll()
        {
            List<Pedido> pedidos = new List<Pedido>();

            string query = @"
                SELECT p.*, e.NombreEstado 
                FROM Pedido p
                JOIN EstadoPedido e ON p.IdEstadoPedido = e.IdEstadoPedido";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text))
            {
                while (reader.Read())
                {
                    pedidos.Add(new Pedido()
                    {
                        IdPedido = (Guid)reader["IdPedido"],
                        IdCliente = (Guid)reader["IdCliente"],
                        FechaPedido = (DateTime)reader["FechaPedido"],
                        IdEstadoPedido = (Guid)reader["IdEstadoPedido"],
                        NombreEstado = reader["NombreEstado"].ToString(),
                        Total = (decimal)reader["Total"]
                    });
                }
            }
            return pedidos;
        }

        public List<Pedido> GetPedidosPorCliente(Guid idCliente)
        {
            List<Pedido> pedidos = new List<Pedido>();

            string query = @"
                SELECT p.*, e.NombreEstado 
                FROM Pedido p
                JOIN EstadoPedido e ON p.IdEstadoPedido = e.IdEstadoPedido
                WHERE p.IdCliente = @IdCliente";

            SqlParameter[] parameters = { new SqlParameter("@IdCliente", idCliente) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text, parameters))
            {
                while (reader.Read())
                {
                    pedidos.Add(new Pedido()
                    {
                        IdPedido = (Guid)reader["IdPedido"],
                        IdCliente = (Guid)reader["IdCliente"],
                        FechaPedido = (DateTime)reader["FechaPedido"],
                        IdEstadoPedido = (Guid)reader["IdEstadoPedido"],
                        NombreEstado = reader["NombreEstado"].ToString(),
                        Total = (decimal)reader["Total"]
                    });
                }
            }
            return pedidos;
        }

        public List<Pedido> GetPedidosPendientes()
        {
            List<Pedido> pedidos = new List<Pedido>();

            string query = @"
                SELECT p.*, e.NombreEstado 
                FROM Pedido p
                JOIN EstadoPedido e ON p.IdEstadoPedido = e.IdEstadoPedido
                WHERE e.NombreEstado = 'Pendiente'";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text))
            {
                while (reader.Read())
                {
                    pedidos.Add(new Pedido()
                    {
                        IdPedido = (Guid)reader["IdPedido"],
                        IdCliente = (Guid)reader["IdCliente"],
                        FechaPedido = (DateTime)reader["FechaPedido"],
                        IdEstadoPedido = (Guid)reader["IdEstadoPedido"],
                        NombreEstado = reader["NombreEstado"].ToString(),
                        Total = (decimal)reader["Total"]
                    });
                }
            }
            return pedidos;
        }

        public List<EstadoPedido> ObtenerEstadosPedido()
        {
            List<EstadoPedido> estados = new List<EstadoPedido>();

            string query = "SELECT * FROM EstadoPedido";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text))
            {
                while (reader.Read())
                {
                    estados.Add(new EstadoPedido()
                    {
                        IdEstadoPedido = (Guid)reader["IdEstadoPedido"],
                        NombreEstado = reader["NombreEstado"].ToString()
                    });
                }
            }
            return estados;
        }
    }
}
