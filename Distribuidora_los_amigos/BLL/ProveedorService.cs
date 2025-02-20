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

        public ProveedorService()
        {
            _proveedorRepository = FactoryDAL.SqlProveedorRepository;
        }

        public void CrearProveedor(Proveedor proveedor)
        {
            ValidarProveedor(proveedor);
            _proveedorRepository.Add(proveedor);
        }

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

        public void ModificarProveedor(Proveedor proveedor)
        {
            ValidarProveedor(proveedor);
            _proveedorRepository.Update(proveedor);
        }

        public void EliminarProveedor(Guid idProveedor)
        {
            _proveedorRepository.Remove(idProveedor);
        }

        public List<Proveedor> ObtenerTodosProveedores()
        {
            return _proveedorRepository.GetAll();
        }

        public Proveedor ObtenerProveedorPorId(Guid idProveedor)
        {
            return _proveedorRepository.GetById(idProveedor);
        }
    }
}
