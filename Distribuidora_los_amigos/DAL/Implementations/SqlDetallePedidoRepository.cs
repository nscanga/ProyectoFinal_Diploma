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
    /// Implementa las operaciones de datos SQL Server para los detalles de pedido.
    /// </summary>
    public class SqlDetallePedidoRepository : IDetallePedidoRepository
    {
        /// <summary>
        /// Inserta un detalle de pedido en la base de datos.
        /// </summary>
        /// <param name="detalle">Entidad con la información del detalle.</param>
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

        /// <summary>
        /// Actualiza los campos editables de un detalle existente.
        /// </summary>
        /// <param name="detalle">Entidad con los datos modificados.</param>
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

        /// <summary>
        /// Elimina físicamente un detalle de pedido según su identificador.
        /// </summary>
        /// <param name="id">Identificador del detalle a borrar.</param>
        public void Remove(Guid id)
        {
            string query = "DELETE FROM DetallePedido WHERE IdDetallePedido = @IdDetallePedido";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdDetallePedido", id)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        /// <summary>
        /// Obtiene un detalle de pedido por su identificador único.
        /// </summary>
        /// <param name="id">Identificador buscado.</param>
        /// <returns>El detalle encontrado o <c>null</c> si no existe.</returns>
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

        /// <summary>
        /// Recupera todos los detalles asociados a un pedido determinado.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Lista de detalles vinculados al pedido.</returns>
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

        /// <summary>
        /// Devuelve la colección completa de detalles almacenados.
        /// </summary>
        /// <returns>Lista con todos los detalles de pedido.</returns>
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

        /// <summary>
        /// Recupera los detalles de un pedido aplicando el mapeo estándar.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido consultado.</param>
        /// <returns>Lista de detalles asociados.</returns>
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

        /// <summary>
        /// Método alternativo para obtener detalles de un pedido (pendiente de implementación).
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>La colección de detalles del pedido.</returns>
        /// <exception cref="NotImplementedException">Siempre, hasta completar la lógica.</exception>
        public List<DetallePedido> GetDetallesPorPedido(Guid idPedido)
        {
            throw new NotImplementedException();
        }
    }
}
