using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOMAIN;

namespace DAL.Contracts
{
    public interface IUsuarioRepository
    {
        void AgregarUsuario(Usuario usuario);
        Usuario ObtenerUsuarioPorId(Guid id);
        Usuario ObtenerUsuarioPorNombre(string userName);
        void ActualizarUsuario(Usuario usuario);
        void EliminarUsuario(Guid id);
        List<Usuario> ObtenerTodosUsuarios();
    }
}
