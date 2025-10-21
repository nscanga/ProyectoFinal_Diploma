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
    /// Implementación SQL Server del repositorio de clientes.
    /// </summary>
    public class SqlClienteRepository : IClienteRepository
    {
        /// <summary>
        /// Inserta un cliente en la base de datos.
        /// </summary>
        /// <param name="cliente">Entidad con los datos a persistir.</param>
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

        /// <summary>
        /// Actualiza la información de un cliente existente.
        /// </summary>
        /// <param name="cliente">Entidad que contiene los datos modificados.</param>
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

        /// <summary>
        /// Elimina un cliente por su identificador.
        /// </summary>
        /// <param name="id">Identificador del cliente a eliminar.</param>
        public void Remove(Guid id)
        {
            string query = "DELETE FROM Cliente WHERE IdCliente = @IdCliente";

            SqlParameter[] parameters = { new SqlParameter("@IdCliente", id) };

            SqlHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        /// <summary>
        /// Recupera un cliente utilizando su identificador único.
        /// </summary>
        /// <param name="id">Identificador buscado.</param>
        /// <returns>El cliente encontrado o <c>null</c> si no existe.</returns>
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
                        Email = reader["Email"].ToString(),    
                        Telefono = reader["Telefono"].ToString(), 
                        CUIT = reader["CUIT"].ToString(),
                        Activo = (bool)reader["Estado"]
                    };
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene todos los clientes registrados.
        /// </summary>
        /// <returns>Lista con los clientes almacenados.</returns>
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

        /// <summary>
        /// Devuelve únicamente los clientes marcados como activos.
        /// </summary>
        /// <returns>Lista con los clientes activos.</returns>
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
