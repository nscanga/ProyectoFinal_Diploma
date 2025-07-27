using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contratcs;
using DAL.Factory;
using DOMAIN;

namespace BLL
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService()
        {
            _clienteRepository = FactoryDAL.SqlClienteRepository;
        }

        public void CrearCliente(Cliente cliente)
        {
            ValidarCliente(cliente);
            _clienteRepository.Add(cliente);
        }

        private void ValidarCliente(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nombre) ||
                string.IsNullOrWhiteSpace(cliente.Direccion) ||
                string.IsNullOrWhiteSpace(cliente.Telefono) ||
                string.IsNullOrWhiteSpace(cliente.Email) ||
                string.IsNullOrWhiteSpace(cliente.CUIT))
            {
                throw new ArgumentException("Todos los campos del cliente son obligatorios.");
            }

            if (!cliente.Email.Contains("@"))
            {
                throw new ArgumentException("El correo electrónico no es válido.");
            }
        }

        public void ModificarCliente(Cliente cliente)
        {
            ValidarCliente(cliente);
            _clienteRepository.Update(cliente);
        }

        public void EliminarCliente(Guid idCliente)
        {
            _clienteRepository.Remove(idCliente);
        }

        public List<Cliente> ObtenerTodosClientes()
        {
            return _clienteRepository.GetAll();
        }

        public Cliente ObtenerClientePorId(Guid idCliente)
        {
            return _clienteRepository.GetById(idCliente);
        }
    }
}
