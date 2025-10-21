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
    /// Repositorio SQL Server encargado de administrar los estados de pedido.
    /// </summary>
    public class SqlEstadoPedidoRepository : IEstadoPedidoRepository
    {
        /// <summary>
        /// Inserta un nuevo estado de pedido en el sistema (pendiente de implementación).
        /// </summary>
        /// <param name="obj">Entidad de estado a registrar.</param>
        public void Add(EstadoPedido obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene el catálogo completo de estados disponibles.
        /// </summary>
        /// <returns>Lista con los estados de pedido.</returns>
        public List<EstadoPedido> GetAll()
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

        /// <summary>
        /// Recupera un estado de pedido según su identificador.
        /// </summary>
        /// <param name="idEstadoPedido">Identificador del estado buscado.</param>
        /// <returns>El estado encontrado o <c>null</c> si no existe.</returns>
        public EstadoPedido GetById(Guid idEstadoPedido)
        {
            string query = "SELECT * FROM EstadoPedido WHERE IdEstadoPedido = @IdEstadoPedido";

            SqlParameter[] parameters = { new SqlParameter("@IdEstadoPedido", idEstadoPedido) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text, parameters))
            {
                if (reader.Read())
                {
                    return new EstadoPedido()
                    {
                        IdEstadoPedido = (Guid)reader["IdEstadoPedido"],
                        NombreEstado = reader["NombreEstado"].ToString()
                    };
                }
            }
            return null;
        }


        /// <summary>
        /// Elimina un estado de pedido existente (pendiente de implementación).
        /// </summary>
        /// <param name="id">Identificador del estado a eliminar.</param>
        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Actualiza los datos de un estado de pedido (pendiente de implementación).
        /// </summary>
        /// <param name="obj">Entidad con los datos actualizados.</param>
        public void Update(EstadoPedido obj)
        {
            throw new NotImplementedException();
        }
    }
}
