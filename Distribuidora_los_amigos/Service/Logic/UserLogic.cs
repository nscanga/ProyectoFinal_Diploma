using Service.DAL.Contracts;
using Service.DAL.FactoryServices;
using Service.DAL.Implementations.SqlServer;
using Service.DAL.Implementations.SqlServer.Helpers;
using Service.DOMAIN;
using Service.DOMAIN.DTO;
using Service.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Logic
{


    public class UserLogic
    {
        private static DVHLogic _dvhLogic = new DVHLogic();

        private readonly IUsuarioRepository _usuarioRepository;


        // Constructor que inicializa el repositorio de usuarios mediante la fábrica
        public UserLogic()
        {
            _usuarioRepository = FactoryDAL.UsuarioRepository;
        }

        // Método para validar un usuario basándose en su nombre de usuario y contraseña
        public bool ValidateUser(string username, string password)
        {
            // Obtener el usuario por su nombre de usuario
            var usuario = _usuarioRepository.GetUsuarioByUsername(username);
            if (usuario != null)
            {
                // Verificar si el usuario está deshabilitado
                if (usuario.Estado == "0")
                {

                    return false;
                }

                // Verificar la contraseña comparando la contraseña proporcionada (hasheada) con la almacenada
                string hashedPassword = CryptographyService.HashMd5(password);
                return usuario.Password == hashedPassword;
            }
            return false; // Usuario no encontrado o contraseña incorrecta
        }

        // Método para crear un nuevo usuario con una contraseña en texto plano
        public void CreateUser(Usuario usuario, string plainPassword, string emial)
        {
            // Hashear la contraseña en texto plano
            usuario.Password = CryptographyService.HashMd5(plainPassword);
            usuario.Email = emial;
            
            // COMENTADO: Sistema de DVH deshabilitado temporalmente
            // _dvhLogic.GenerarYGuardarCodigoVerificador(usuario);
            
            _usuarioRepository.CreateUsuario(usuario);
        }

        // Método para deshabilitar un usuario por su ID
        public void DisableUser(Guid idUsuario)
        {
            _usuarioRepository.DisableUsuario(idUsuario);
            
            // COMENTADO: Sistema de DVH deshabilitado temporalmente
            // var usuario = _usuarioRepository.ObetenerUsuarioById(idUsuario);
            // if (usuario != null)
            // {
            //     _dvhLogic.ActualizarCodigoVerificador(usuario);
            // }
        }

        // método para habilitar un usuario por su id
        //public void enabledusuario(guid idusuario)
        //{
        //    _usuariorepository.enabledusuario(idusuario);
        //    var usuario = _usuariorepository.obetenerusiariobyid(idusuario);
        //    if (usuario != null)
        //    {
        //        _dvhlogic.actualizarcodigoverificador(usuario);
        //    }
        //}

        // Método para actualizar los accesos (permisos) de un usuario
        public void UpdateUserAccesos(Guid idUsuario, List<Acceso> accesos)
        {
            // Tu lógica de actualización de accesos aquí...
            
            // COMENTADO: Sistema de DVH deshabilitado temporalmente
            // var usuario = _usuarioRepository.ObetenerUsuarioById(idUsuario);
            // if (usuario != null)
            // {
            //     _dvhLogic.ActualizarCodigoVerificador(usuario);
            // }
        }

        // Método para obtener todos los usuarios
        public List<Usuario> GetAllUsuarios()
        {
            return _usuarioRepository.GetAll();
        }

        // Método para obtener un usuario específico por su nombre de usuario
        public Usuario GetUsuarioByUsername(string username)
        {
            return _usuarioRepository.GetUsuarioByUsername(username);
        }
        public Usuario GetUsuarioDatos(string username)
        {
            return _usuarioRepository.GetUsuarioCompletos(username);
        }

        public List<UsuarioRolDto> GetUsuariosConFamilasYPatentes()
        {
            return _usuarioRepository.GetUsuariosConFamilasYPatentes();
        }

        // Método para crear una nueva patente (permiso o acceso)
        public void CreatePatente(Patente patente)
        {
            _usuarioRepository.CreatePatente(patente);
        }

        public void SaveLenguaje(Guid idUsuario, string lenguaje)
        {
            _usuarioRepository.UpdateLenguaje(idUsuario, lenguaje);

            var usuario = _usuarioRepository.ObetenerUsuarioById(idUsuario);
            if (usuario != null)
            {
                _dvhLogic.ActualizarCodigoVerificador(usuario);
            }
        }

        public string GetUserLenguaje(Guid idUsuario)
        {
            try
            {
                string lenguaje = _usuarioRepository.GetUserLenguaje(idUsuario);
                return string.IsNullOrEmpty(lenguaje) ? "es-ES" : lenguaje;
            }
            catch (Exception ex)
            {
                // Aquí podrías loguear el error
                return "es-ES"; // Valor predeterminado en caso de error
            }
        }
    }

}


