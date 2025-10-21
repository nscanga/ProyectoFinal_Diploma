using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DAL.Contracts;
using DAL.Contratcs;
using DOMAIN;

namespace DAL.Implementations.SqlServer
{
    /// <summary>
    /// Implementación SQL Server de las operaciones de persistencia para pedidos.
    /// </summary>
    public class SqlPedidoRepository : IPedidoRepository
    {
        /// <summary>
        /// Registra un nuevo pedido junto con su estado asociado.
        /// </summary>
        /// <param name="pedido">Entidad de pedido a insertar.</param>
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

        /// <summary>
        /// Actualiza el total y el estado de un pedido existente.
        /// </summary>
        /// <param name="pedido">Pedido con la información actualizada.</param>
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

        /// <summary>
        /// Elimina un pedido según su identificador.
        /// </summary>
        /// <param name="id">Identificador del pedido a eliminar.</param>
        public void Remove(Guid id)
        {
            string query = "DELETE FROM Pedido WHERE IdPedido = @IdPedido";

            SqlParameter[] parameters = { new SqlParameter("@IdPedido", id) };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        /// <summary>
        /// Recupera un pedido y su estado a partir del identificador.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido buscado.</param>
        /// <returns>El pedido encontrado o <c>null</c> si no existe.</returns>
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

        /// <summary>
        /// Obtiene todos los pedidos con su estado asociado.
        /// </summary>
        /// <returns>Lista de pedidos almacenados.</returns>
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

        /// <summary>
        /// Recupera los pedidos pertenecientes a un cliente en particular.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente.</param>
        /// <returns>Lista de pedidos del cliente indicado.</returns>
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

        /// <summary>
        /// Devuelve los pedidos cuyo estado es "Pendiente".
        /// </summary>
        /// <returns>Lista de pedidos en estado pendiente.</returns>
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

        /// <summary>
        /// Obtiene el catálogo completo de estados de pedido.
        /// </summary>
        /// <returns>Lista de estados disponibles.</returns>
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

        // Agrega este método privado para obtener la conexión
        /// <summary>
        /// Crea una conexión SQL Server utilizando la cadena configurada.
        /// </summary>
        /// <returns>Instancia de <see cref="SqlConnection"/> preparada para abrirse.</returns>
        private SqlConnection GetConnection()
        {
            // Reemplaza "DefaultConnection" por el nombre correcto de tu cadena de conexión
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Actualiza solamente el estado de un pedido existente.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido a modificar.</param>
        /// <param name="nuevoEstadoId">Nuevo estado que se asignará.</param>
        public void UpdateEstado(Guid idPedido, Guid nuevoEstadoId)
        {
            string query = @"
                UPDATE Pedido 
                SET IdEstadoPedido = @nuevoEstadoId
                WHERE IdPedido = @idPedido";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@idPedido", idPedido),
                new SqlParameter("@nuevoEstadoId", nuevoEstadoId)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }
    }
}
