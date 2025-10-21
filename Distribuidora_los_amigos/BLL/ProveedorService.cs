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
            ValidarProveedor(proveedor);
            _proveedorRepository.Add(proveedor);
        }

        /// <summary>
        /// Comprueba que los campos obligatorios del proveedor tengan valores válidos.
        /// </summary>
        /// <param name="proveedor">Proveedor que se va a evaluar.</param>
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
            ValidarProveedor(proveedor);
            _proveedorRepository.Update(proveedor);
        }

        /// <summary>
        /// Elimina a un proveedor del sistema.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor a quitar.</param>
        public void EliminarProveedor(Guid idProveedor)
        {
            _proveedorRepository.Remove(idProveedor);
        }

        /// <summary>
        /// Recupera todos los proveedores almacenados.
        /// </summary>
        /// <returns>Listado completo de proveedores.</returns>
        public List<Proveedor> ObtenerTodosProveedores()
        {
            return _proveedorRepository.GetAll();
        }

        /// <summary>
        /// Obtiene un proveedor utilizando su identificador único.
        /// </summary>
        /// <param name="idProveedor">Identificador del proveedor buscado.</param>
        /// <returns>Proveedor encontrado o null si no existe.</returns>
        public Proveedor ObtenerProveedorPorId(Guid idProveedor)
        {
            return _proveedorRepository.GetById(idProveedor);
        }
    }
}
