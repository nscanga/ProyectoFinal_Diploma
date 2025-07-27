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
    public class SqlClienteRepository : IClienteRepository
    {
        public void Add(Cliente cliente)
        {
            string query = @"
                INSERT INTO Cliente (IdCliente, Nombre, Direccion, Telefono, Email, CUIT, Estado) 
                VALUES (@IdCliente, @Nombre, @Direccion, @Telefono, @Email, @CUIT, @Estado)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdCliente", cliente.IdCliente),
                new SqlParameter("@Nombre", cliente.Nombre),
                new SqlParameter("@Direccion", cliente.Direccion),
                new SqlParameter("@Telefono", cliente.Telefono),
                new SqlParameter("@Email", cliente.Email),
                new SqlParameter("@CUIT", cliente.CUIT),
                new SqlParameter("@Estado", cliente.Activo)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        public void Update(Cliente cliente)
        {
            string query = @"
                UPDATE Cliente 
                SET Nombre = @Nombre, Direccion = @Direccion, Email = @Email, Telefono = @Telefono 
                WHERE IdCliente = @IdCliente";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdCliente", cliente.IdCliente),
                new SqlParameter("@Nombre", cliente.Nombre),
                new SqlParameter("@Direccion", cliente.Direccion),
                new SqlParameter("@Telefono", cliente.Telefono),
                new SqlParameter("@Email", cliente.Email),
                new SqlParameter("@CUIT", cliente.CUIT),
                new SqlParameter("@Estado", cliente.Activo)
            };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        public void Remove(Guid id)
        {
            string query = "DELETE FROM Cliente WHERE IdCliente = @IdCliente";

            SqlParameter[] parameters = { new SqlParameter("@IdCliente", id) };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        public Cliente GetById(Guid id)
        {
            string query = "SELECT * FROM Cliente WHERE IdCliente = @IdCliente";

            SqlParameter[] parameters = { new SqlParameter("@IdCliente", id) };

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text, parameters))
            {
                if (reader.Read())
                {
                    return new Cliente()
                    {
                        IdCliente = (Guid)reader["IdCliente"],
                        Nombre = reader["Nombre"].ToString(),
                        Direccion = reader["Direccion"].ToString(),
                        Email = reader["Telefono"].ToString(),
                        Telefono = reader["Email"].ToString(),
                        CUIT = reader["CUIT"].ToString(),
                        Activo = (bool)reader["Estado"]
                    };
                }
            }
            return null;
        }

        public List<Cliente> GetAll()
        {
            List<Cliente> clientes = new List<Cliente>();

            string query = "SELECT * FROM Cliente";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text))
            {
                while (reader.Read())
                {
                    clientes.Add(new Cliente()
                    {
                        IdCliente = (Guid)reader["IdCliente"],
                        Nombre = reader["Nombre"].ToString(),
                        Direccion = reader["Direccion"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Email = reader["Email"].ToString(),
                        CUIT = reader["CUIT"].ToString(),
                        Activo = (bool)reader["Estado"]
                    });
                }
            }
            return clientes;
        }

        public List<Cliente> GetClientesActivos()
        {
            List<Cliente> clientes = new List<Cliente>();

            string query = "SELECT * FROM Cliente WHERE Activo = 1";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text))
            {
                while (reader.Read())
                {
                    clientes.Add(new Cliente()
                    {
                        IdCliente = (Guid)reader["IdCliente"],
                        Nombre = reader["Nombre"].ToString(),
                        Direccion = reader["Direccion"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Email = reader["Email"].ToString(),
                        CUIT = reader["CUIT"].ToString(),
                        Activo = (bool)reader["Estado"]
                    });
                }
            }
            return clientes;
        }
    }
}
