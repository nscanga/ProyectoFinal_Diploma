using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAL.Contratcs;
using DAL.Factory;
using DOMAIN;
using BLL.Exceptions;
using BLL.Helpers;

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
            try
            {
                // 🆕 Validación completa de negocio
                ValidarClienteCompleto(cliente, esCreacion: true);
                
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    _clienteRepository.Add(cliente);
                }, "Error al crear cliente");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"❌ No se puede crear cliente sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Valida completamente un cliente antes de crearlo o modificarlo.
        /// </summary>
        private void ValidarClienteCompleto(Cliente cliente, bool esCreacion = false)
        {
            // Validar que el cliente no sea nulo
            if (cliente == null)
                throw ClienteException.ClienteNulo();

            // Validar nombre
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                throw ClienteException.NombreRequerido();

            // Validar dirección
            if (string.IsNullOrWhiteSpace(cliente.Direccion))
                throw ClienteException.DireccionRequerida();

            // Validar teléfono
            if (string.IsNullOrWhiteSpace(cliente.Telefono))
                throw ClienteException.TelefonoRequerido();

            // Validar que teléfono no sea email
            if (cliente.Telefono.Contains("@"))
                throw ClienteException.TelefonoInvalido(cliente.Telefono);

            // Validar teléfono mínimo 10 dígitos
            if (!EsTelefonoValido(cliente.Telefono))
                throw ClienteException.TelefonoInvalido(cliente.Telefono);

            // Validar email
            if (string.IsNullOrWhiteSpace(cliente.Email))
                throw ClienteException.EmailRequerido();

            // Validar que email no sea teléfono
            if (Regex.IsMatch(cliente.Email, @"^\d{8,15}$"))
                throw ClienteException.EmailInvalido(cliente.Email);

            // Validar formato de email
            if (!EsEmailValido(cliente.Email))
                throw ClienteException.EmailInvalido(cliente.Email);

            // Validar CUIT
            if (string.IsNullOrWhiteSpace(cliente.CUIT))
                throw ClienteException.CUITRequerido();

            if (!EsCUITValido(cliente.CUIT))
                throw ClienteException.CUITInvalido(cliente.CUIT);
        }

        /// <summary>
        /// Valida el formato de un email usando expresión regular.
        /// </summary>
        private bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Valida que el teléfono tenga al menos 10 dígitos.
        /// </summary>
        private bool EsTelefonoValido(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return false;

            // Eliminar caracteres no numéricos
            string telefonoLimpio = new string(telefono.Where(char.IsDigit).ToArray());
            return telefonoLimpio.Length >= 10;
        }

        /// <summary>
        /// Valida que el CUIT tenga 11 dígitos.
        /// </summary>
        private bool EsCUITValido(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                return false;

            // Eliminar guiones y espacios
            string cuitLimpio = cuit.Replace("-", "").Replace(" ", "");
            return cuitLimpio.Length == 11 && cuitLimpio.All(char.IsDigit);
        }

        /// <summary>
        /// Revisa que la información del cliente cumpla con los campos obligatorios y formatos esperados.
        /// NOTA: Método legacy mantenido por compatibilidad. Usar ValidarClienteCompleto() en su lugar.
        /// </summary>
        /// <param name="cliente">Entidad a validar.</param>
        [Obsolete("Usar ValidarClienteCompleto() que incluye validaciones más robustas")]
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

            // Validar que email no sea teléfono
            if (Regex.IsMatch(cliente.Email, @"^\d{8,15}$"))
            {
                throw new ArgumentException($"ERROR: El campo Email contiene un teléfono ({cliente.Email}). Por favor ingrese un email válido.");
            }

            // Validar que teléfono no sea email
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
            try
            {
                // 🆕 Usar validación completa
                ValidarClienteCompleto(cliente, esCreacion: false);
                
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    _clienteRepository.Update(cliente);
                }, "Error al modificar cliente");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"❌ No se puede modificar cliente sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Elimina definitivamente un cliente del repositorio.
        /// </summary>
        /// <param name="idCliente">Identificador del cliente a remover.</param>
        public void EliminarCliente(Guid idCliente)
        {
            try
            {
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    _clienteRepository.Remove(idCliente);
                }, "Error al eliminar cliente");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"❌ No se puede eliminar cliente sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Recupera el listado completo de clientes registrados.
        /// Si hay error de conexión, propaga la excepción para que la UI la maneje.
        /// </summary>
        /// <returns>Lista con todos los clientes disponibles.</returns>
        public List<Cliente> ObtenerTodosClientes()
        {
            try
            {
                return ExceptionMapper.ExecuteWithMapping(() =>
                {
                    return _clienteRepository.GetAll();
                }, "Error al obtener clientes");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    Console.WriteLine($"⚠️ Error de conexión al obtener clientes.");
                }
                throw;
            }
        }

        /// <summary>
        /// Busca un cliente por su identificador único.
        /// </summary>
        /// <param name="idCliente">Id del cliente a consultar.</param>
        /// <returns>Cliente correspondiente o null si no existe.</returns>
        public Cliente ObtenerClientePorId(Guid idCliente)
        {
            try
            {
                return ExceptionMapper.ExecuteWithMapping(() =>
                {
                    return _clienteRepository.GetById(idCliente);
                }, $"Error al obtener cliente {idCliente}");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    Console.WriteLine($"⚠️ Error de conexión al obtener cliente.");
                }
                throw;
            }
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
