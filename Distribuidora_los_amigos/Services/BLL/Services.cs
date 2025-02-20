using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contracts;
using DOMAIN;

namespace Services.BLL
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public void CrearUsuario(Usuario usuario)
        {
            usuario.Password = SeguridadLogic.EncriptarPassword(usuario.Password);
            _usuarioRepository.AgregarUsuario(usuario);
        }

        public Usuario ObtenerUsuarioPorNombre(string userName)
        {
            return _usuarioRepository.ObtenerUsuarioPorNombre(userName);
        }

        public void AsignarAcceso(Usuario usuario, Acceso acceso)
        {
            usuario.Accesos.Add(acceso);
            _usuarioRepository.ActualizarUsuario(usuario);
        }

        public List<Patente> ObtenerPatentes(Usuario usuario)
        {
            return usuario.GetPatentes();
        }

        public List<Familia> ObtenerFamilias(Usuario usuario)
        {
            return usuario.GetFamilias();
        }
    }
}
