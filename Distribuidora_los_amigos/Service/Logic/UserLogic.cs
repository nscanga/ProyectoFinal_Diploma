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


    /// <summary>
    /// Contiene la lógica de negocio asociada a usuarios y sus configuraciones.
    /// </summary>
    public class UserLogic
    {
        private static DVHLogic _dvhLogic = new DVHLogic();

        private readonly IUsuarioRepository _usuarioRepository;


        // Constructor que inicializa el repositorio de usuarios mediante la fábrica
        /// <summary>
        /// Inicializa la lógica de usuarios con el repositorio configurado por la fábrica DAL.
        /// </summary>
        public UserLogic()
        {
            _usuarioRepository = FactoryDAL.UsuarioRepository;
        }

        // Método para validar un usuario basándose en su nombre de usuario y contraseña
        /// <summary>
        /// Valida las credenciales del usuario comprobando estado y contraseña encriptada.
        /// </summary>
        /// <param name="username">Nombre de usuario ingresado.</param>
        /// <param name="password">Contraseña en texto plano a verificar.</param>
        /// <returns><c>true</c> si las credenciales son correctas.</returns>
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
        /// <summary>
        /// Crea un nuevo usuario aplicando hashing a la contraseña recibida.
        /// </summary>
        /// <param name="usuario">Entidad usuario a persistir.</param>
        /// <param name="plainPassword">Contraseña sin encriptar.</param>
        /// <param name="emial">Correo electrónico asociado.</param>
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
        /// <summary>
        /// Deshabilita al usuario especificado.
        /// </summary>
        /// <param name="idUsuario">Identificador del usuario.</param>
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
        /// <summary>
        /// Actualiza las patentes y familias asignadas a un usuario.
        /// </summary>
        /// <param name="idUsuario">Identificador del usuario.</param>
        /// <param name="accesos">Colección de accesos a establecer.</param>
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
        /// <summary>
        /// Devuelve todos los usuarios registrados.
        /// </summary>
        public List<Usuario> GetAllUsuarios()
        {
            return _usuarioRepository.GetAll();
        }

        // Método para obtener un usuario específico por su nombre de usuario
        /// <summary>
        /// Obtiene un usuario a partir de su nombre.
        /// </summary>
        public Usuario GetUsuarioByUsername(string username)
        {
            return _usuarioRepository.GetUsuarioByUsername(username);
        }
        /// <summary>
        /// Recupera un usuario con información ampliada.
        /// </summary>
        public Usuario GetUsuarioDatos(string username)
        {
            return _usuarioRepository.GetUsuarioCompletos(username);
        }

        /// <summary>
        /// Lista usuarios con el detalle de roles y patentes.
        /// </summary>
        public List<UsuarioRolDto> GetUsuariosConFamilasYPatentes()
        {
            return _usuarioRepository.GetUsuariosConFamilasYPatentes();
        }

        // Método para crear una nueva patente (permiso o acceso)
        /// <summary>
        /// Registra una nueva patente en el sistema.
        /// </summary>
        public void CreatePatente(Patente patente)
        {
            _usuarioRepository.CreatePatente(patente);
        }

        /// <summary>
        /// Guarda el idioma seleccionado para el usuario y actualiza su DVH.
        /// </summary>
        /// <param name="idUsuario">Identificador del usuario.</param>
        /// <param name="lenguaje">Código de idioma a asignar.</param>
        public void SaveLenguaje(Guid idUsuario, string lenguaje)
        {
            _usuarioRepository.UpdateLenguaje(idUsuario, lenguaje);

            var usuario = _usuarioRepository.ObetenerUsuarioById(idUsuario);
            if (usuario != null)
            {
                _dvhLogic.ActualizarCodigoVerificador(usuario);
            }
        }

        /// <summary>
        /// Obtiene el idioma configurado para el usuario o retorna el valor por defecto en caso de error.
        /// </summary>
        /// <param name="idUsuario">Identificador del usuario.</param>
        /// <returns>Código de idioma.</returns>
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


