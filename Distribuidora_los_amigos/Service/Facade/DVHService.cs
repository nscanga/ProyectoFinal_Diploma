using DOMAIN;
using Service.DAL.Implementations.SqlServer.Helpers;
using Service.DOMAIN;
using Service.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service.Facade
{
    public class DVHService
    {
        private static readonly UsuarioRepository _UsuarioDAL = new UsuarioRepository();
        private static DVHLogic _dvhLogic = new DVHLogic();

        // Método que invoca la lógica de DVH para todos los usuarios
        //public void VerificarDVHDeUsuarios()
        //{
        //    HashSet<string> erroresRegistrados = new HashSet<string>(); // Usamos un HashSet para evitar duplicados

        //    // Obtener todos los usuarios
        //    List<Usuario> usuarios = _UsuarioDAL.GetAllUsuarios();

        //    foreach (var usuario in usuarios)
        //    {
        //        if (usuario == null)
        //        {
        //            // Si el usuario es nulo, continuamos con el siguiente
        //            continue;
        //        }

        //        // Verificar el DVH para cada usuario
        //        bool esValido = _dvhLogic.VerificarDVH(usuario);

        //        if (!esValido)
        //        {
        //            // Crear el mensaje de error
        //            string mensajeError = $"Hay inconsistencias en la base de datos Login, En la Tabla Usuario, En el usuario con nombre: {usuario.UserName}";

        //            // Verificar si el error ya ha sido registrado
        //            if (!erroresRegistrados.Contains(mensajeError))
        //            {
        //                // Si no se ha registrado, agregarlo al HashSet
        //                erroresRegistrados.Add(mensajeError);

        //                // Llamar al método para escribir en la bitácora
        //                //LoggerService.WriteLog(mensajeError, System.Diagnostics.TraceLevel.Error);
        //            }
        //        }
        //    }
        //}

        //}


    }
}


