using Service.DOMAIN;
using Service.DOMAIN.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL.Contracts
{
    public interface IUsuarioRepository : IGenericServiceDAL<Usuario>
    {
        Usuario GetUsuarioByUsername(string username);
        void CreateUsuario(Usuario usuario);
        void DisableUsuario(Guid idUsuario);
        void UpdateAccesos(Guid idUsuario, List<Acceso> accesos);
      
        void EnabledUsuario(Guid idUsuario);
        void CreatePatente(Patente patente);

        Usuario GetUsuarioById(Guid idUsuario);

        Usuario ObetenerUsuarioById(Guid idUsuario);
        bool HasAccess(Guid idUsuario, TipoAcceso tipoAcceso);

        List<UsuarioRolDto> GetUsuariosConFamilasYPatentes();
        bool ExistePatente(string nombrePatente);

        Usuario GetUsuarioCompletos(string username);

        void UpdateUsuarioToken(Usuario usuario);
        void UpdatePassword(Usuario usuario);

        void UpdateLanguage(Guid idUsuario, string language);

        string GetUserLanguage(Guid idUsuario);
    }
}
