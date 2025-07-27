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
    public class SqlEstadoPedidoRepository : IEstadoPedidoRepository
    {
        public void Add(EstadoPedido obj)
        {
            throw new NotImplementedException();
        }

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


        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(EstadoPedido obj)
        {
            throw new NotImplementedException();
        }
    }
}
