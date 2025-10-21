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

        /// <summary>
        /// Inicializa el servicio de clientes resolviendo el repositorio concreto a utilizar.
        /// </summary>
        public ClienteService()
        {
            _clienteRepository = FactoryDAL.SqlClienteRepository;
        }

        /// <summary>
        /// Registra un nuevo cliente luego de verificar que sus datos sean válidos.
        /// </summary>
        /// <param name="cliente">Cliente que se agregará al sistema.</param>
        public void CrearCliente(Cliente cliente)
        {
            ValidarCliente(cliente);
            _clienteRepository.Add(cliente);
        }

        /// <summary>
        /// Revisa que la información del cliente cumpla con los campos obligatorios y formatos esperados.
        /// </summary>
        /// <param name="cliente">Entidad a validar.</param>
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

            // 🔧 VALIDAR QUE EMAIL NO SEA TELÉFONO
            if (System.Text.RegularExpressions.Regex.IsMatch(cliente.Email, @"^\d{8,15}$"))
            {
                throw new ArgumentException($"ERROR: El campo Email contiene un teléfono ({cliente.Email}). Por favor ingrese un email válido.");
            }

            // 🔧 VALIDAR QUE TELÉFONO NO SEA EMAIL
            if (cliente.Telefono.Contains("@"))
            {
                throw new ArgumentException($"ERROR: El campo Teléfono contiene un email ({cliente.Telefono}). Por favor ingrese solo números.");
            }

            if (!cliente.Email.Contains("@"))
            {
                throw new ArgumentException("El correo electrónico no es válido.");
            }
        }

        /// <summary>
        /// Actualiza los datos almacenados de un cliente existente.
        /// </summary>
        /// <param name="cliente">Cliente con la información actualizada.</param>
        public void ModificarCliente(Cliente cliente)
        {
            ValidarCliente(cliente);
            _clienteRepository.Update(cliente);
        }

        /// <summary>
        /// Elimina definitivamente un cliente del repositorio.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente a remover.</param>
        public void EliminarCliente(Guid idCliente)
        {
            _clienteRepository.Remove(idCliente);
        }

        /// <summary>
        /// Recupera el listado completo de clientes registrados.
        /// </summary>
        /// <returns>Lista con todos los clientes disponibles.</returns>
        public List<Cliente> ObtenerTodosClientes()
        {
            return _clienteRepository.GetAll();
        }

        /// <summary>
        /// Busca un cliente por su identificador único.
        /// </summary>
        /// <param name="idCliente">Id del cliente a consultar.</param>
        /// <returns>Cliente correspondiente o null si no existe.</returns>
        public Cliente ObtenerClientePorId(Guid idCliente)
        {
            return _clienteRepository.GetById(idCliente);
        }

        /// <summary>
        /// Imprime por consola información detallada de los emails de los clientes para depuración.
        /// </summary>
        public void VerificarEmailsClientes()
        {
            var clientes = _clienteRepository.GetAll();

            Console.WriteLine($"🔍 VERIFICANDO EMAILS DE {clientes.Count} CLIENTES:");
            Console.WriteLine(new string('=', 60));
            
            foreach (var cliente in clientes)
            {
                Console.WriteLine($"Cliente: {cliente.Nombre}");
                Console.WriteLine($"  ID: {cliente.IdCliente}");
                Console.WriteLine($"  Email crudo: '{cliente.Email}'");
                Console.WriteLine($"  Email es null: {cliente.Email == null}");
                Console.WriteLine($"  Email está vacío: {string.IsNullOrEmpty(cliente.Email)}");
                Console.WriteLine($"  Longitud: {cliente.Email?.Length ?? 0}");
                
                if (!string.IsNullOrEmpty(cliente.Email))
                {
                    // Mostrar cada carácter para detectar caracteres ocultos
                    Console.WriteLine($"  Caracteres: {string.Join(",", cliente.Email.ToCharArray().Select(c => $"'{c}'({(int)c})"))}");
                }
                
                Console.WriteLine("  ---");
            }
        }
    }
}
