using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BLL.Exceptions;
using BLL.Helpers;
using DAL.Contratcs;
using DAL.Factory;
using DOMAIN;

namespace BLL
{
    public class ProveedorService
    {
        private readonly IProveedorRepository _proveedorRepository;

        /// <summary>
        /// Inicializa el servicio de proveedores con el repositorio concreto.
        /// </summary>
        public ProveedorService()
        {
            _proveedorRepository = FactoryDAL.SqlProveedorRepository;
        }

        /// <summary>
        /// Agrega un proveedor nuevo luego de validar su información.
        /// </summary>
        /// <param name="proveedor">Proveedor a registrar.</param>
        public void CrearProveedor(Proveedor proveedor)
        {
            try
            {
                // 🆕 Validación completa de negocio
                ValidarProveedorCompleto(proveedor);
                
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    _proveedorRepository.Add(proveedor);
                }, "Error al crear proveedor");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"❌ No se puede crear proveedor sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Valida completamente un proveedor antes de crearlo o modificarlo.
        /// </summary>
        private void ValidarProveedorCompleto(Proveedor proveedor)
        {
            // Validar que el proveedor no sea nulo
            if (proveedor == null)
                throw ProveedorException.ProveedorNulo();

            // Validar nombre
            if (string.IsNullOrWhiteSpace(proveedor.Nombre))
                throw ProveedorException.NombreRequerido();

            // Validar dirección
            if (string.IsNullOrWhiteSpace(proveedor.Direccion))
                throw ProveedorException.DireccionRequerida();

            // Validar email
            if (string.IsNullOrWhiteSpace(proveedor.Email))
                throw ProveedorException.EmailRequerido();

            // Validar formato de email
            if (!EsEmailValido(proveedor.Email))
                throw ProveedorException.EmailInvalido(proveedor.Email);

            // Validar teléfono
            if (string.IsNullOrWhiteSpace(proveedor.Telefono))
                throw ProveedorException.TelefonoRequerido();

            // Validar teléfono mínimo 10 dígitos
            if (!EsTelefonoValido(proveedor.Telefono))
                throw ProveedorException.TelefonoInvalido(proveedor.Telefono);

            // Validar categoría
            if (string.IsNullOrWhiteSpace(proveedor.Categoria))
                throw ProveedorException.CategoriaRequerida();
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
        /// Comprueba que los campos obligatorios del proveedor tengan valores válidos.
        /// NOTA: Método legacy mantenido por compatibilidad. Usar ValidarProveedorCompleto() en su lugar.
        /// </summary>
        /// <param name="proveedor">Proveedor que se va a evaluar.</param>
        [Obsolete("Usar ValidarProveedorCompleto() que incluye validaciones más robustas")]
        private void ValidarProveedor(Proveedor proveedor)
        {
            if (string.IsNullOrWhiteSpace(proveedor.Nombre) ||
                string.IsNullOrWhiteSpace(proveedor.Direccion) ||
                string.IsNullOrWhiteSpace(proveedor.Email) ||
                string.IsNullOrWhiteSpace(proveedor.Telefono) ||
                string.IsNullOrWhiteSpace(proveedor.Categoria))
            {
                throw new ArgumentException("Todos los campos son obligatorios.");
            }

            if (!proveedor.Email.Contains("@"))
            {
                throw new ArgumentException("El correo electrónico no es válido.");
            }
        }

        /// <summary>
        /// Actualiza la información de un proveedor existente.
        /// </summary>
        /// <param name="proveedor">Proveedor con los datos modificados.</param>
        public void ModificarProveedor(Proveedor proveedor)
        {
            try
            {
                // 🆕 Usar validación completa
                ValidarProveedorCompleto(proveedor);
                
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    _proveedorRepository.Update(proveedor);
                }, "Error al modificar proveedor");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"❌ No se puede modificar proveedor sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Elimina a un proveedor del sistema.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor a quitar.</param>
        public void EliminarProveedor(Guid idProveedor)
        {
            try
            {
                ExceptionMapper.ExecuteWithMapping(() =>
                {
                    _proveedorRepository.Remove(idProveedor);
                }, "Error al eliminar proveedor");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed)
                {
                    Console.WriteLine($"❌ No se puede eliminar proveedor sin conexión.");
                }
                throw;
            }
        }

        /// <summary>
        /// Recupera todos los proveedores almacenados.
        /// </summary>
        /// <returns>Listado completo de proveedores.</returns>
        public List<Proveedor> ObtenerTodosProveedores()
        {
            try
            {
                return ExceptionMapper.ExecuteWithMapping(() =>
                {
                    return _proveedorRepository.GetAll();
                }, "Error al obtener proveedores");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    Console.WriteLine($"⚠️ Error de conexión al obtener proveedores.");
                }
                throw;
            }
        }

        /// <summary>
        /// Obtiene un proveedor utilizando su identificador único.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor buscado.</param>
        /// <returns>Proveedor encontrado o null si no existe.</returns>
        public Proveedor ObtenerProveedorPorId(Guid idProveedor)
        {
            try
            {
                return ExceptionMapper.ExecuteWithMapping(() =>
                {
                    return _proveedorRepository.GetById(idProveedor);
                }, $"Error al obtener proveedor {idProveedor}");
            }
            catch (DatabaseException dbEx)
            {
                if (dbEx.ErrorType == DatabaseErrorType.ConnectionFailed || 
                    dbEx.ErrorType == DatabaseErrorType.Timeout)
                {
                    Console.WriteLine($"⚠️ Error de conexión al obtener proveedor.");
                }
                throw;
            }
        }
    }
}
