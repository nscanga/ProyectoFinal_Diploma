using Service.DAL.Implementations.SqlServer.Helpers;
using Service.DOMAIN;
using Service.DOMAIN.DTO;
using Service.Logic;
using Services.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Service.Facade
{
    /// <summary>
    /// Fachada que expone operaciones de usuario combinando validaciones y lógica de negocio.
    /// </summary>
    public static class UserService
    {
        private static UserLogic _userLogic = new UserLogic();
        private static readonly UsuarioRepository _UsuarioDAL = new UsuarioRepository();
        private static readonly FamiliaRepository _familiaDAL = new FamiliaRepository();

        /// <summary>
        /// Valida credenciales de usuario delegando en la lógica correspondiente.
        /// </summary>
        public static bool Login(string username, string password)
        {
            return _userLogic.ValidateUser(username, password);
        }

        /// <summary>
        /// Registra un nuevo usuario verificando previamente el formato del correo.
        /// </summary>
        public static void Register(string username, string password, string email)
        {
            if (!IsValidEmail(email))
            {
                string messageKey = "El formato del correo electrónico no es válido.";
                string translatedMessage = TranslateMessageKey(messageKey);
                throw new ArgumentException(translatedMessage);
            }
            var usuario = new Usuario
            {
                UserName = username
            };
            _userLogic.CreateUser(usuario, password, email);
        }

        /// <summary>
        /// Inicializa el sistema creando el usuario administrador por defecto con su rol.
        /// Este método se ejecuta automáticamente cuando no existen usuarios en el sistema.
        /// </summary>
        /// <returns>True si la inicialización fue exitosa, False si ya existían usuarios.</returns>
        public static bool InicializarSistemaConAdminDefault()
        {
            try
            {
                // Verificar si ya existen usuarios
                List<Usuario> usuarios = GetAllUsuarios();
                if (usuarios.Count > 0)
                {
                    return false; // El sistema ya está inicializado
                }

                // Crear el usuario administrador
                Register("admin", "Admin123!", "admin@sistema.com");
                LoggerService.WriteLog("Usuario administrador por defecto creado exitosamente.", System.Diagnostics.TraceLevel.Info);

                // Obtener el usuario recién creado
                Usuario adminUser = _userLogic.GetUsuarioByUsername("admin");
                if (adminUser == null)
                {
                    throw new Exception("Error al recuperar el usuario administrador recién creado.");
                }

                // Buscar o crear la familia Administrador
                Familia familiaAdmin = ObtenerOCrearFamiliaAdministrador();

                // Asignar la familia al usuario admin (sin validación de familia existente)
                _familiaDAL.SaveUsuarioFamilia(adminUser.IdUsuario, familiaAdmin.Id);
                
                LoggerService.WriteLog($"Rol 'Administrador' asignado al usuario admin exitosamente.", System.Diagnostics.TraceLevel.Info);

                return true;
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog($"Error al inicializar sistema con usuario admin: {ex.Message}", System.Diagnostics.TraceLevel.Error);
                LoggerService.WriteException(ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene la familia Administrador o la crea si no existe con todas las patentes disponibles.
        /// </summary>
        private static Familia ObtenerOCrearFamiliaAdministrador()
        {
            // Buscar si ya existe la familia Administrador
            List<Familia> todasLasFamilias = _familiaDAL.GetAll();
            Familia familiaAdmin = todasLasFamilias.FirstOrDefault(f => f.Nombre.Equals("Administrador", StringComparison.OrdinalIgnoreCase));

            if (familiaAdmin != null)
            {
                // La familia ya existe, cargar sus patentes
                familiaAdmin.Accesos.AddRange(_familiaDAL.GetPatentesByFamiliaId(familiaAdmin.Id));
                LoggerService.WriteLog("Familia 'Administrador' encontrada en el sistema.", System.Diagnostics.TraceLevel.Info);
                return familiaAdmin;
            }

            // La familia no existe, crearla con todas las patentes
            familiaAdmin = new Familia
            {
                Id = Guid.NewGuid(),
                Nombre = "Administrador"
            };

            // Obtener todas las patentes disponibles
            List<Patente> todasLasPatentes = _familiaDAL.GetAllPatentes();
            
            if (todasLasPatentes.Count == 0)
            {
                // Si no hay patentes, crear las básicas para que el admin pueda gestionar el sistema
                LoggerService.WriteLog("ADVERTENCIA: No existen patentes en el sistema. El administrador tendrá acceso limitado hasta que se configuren las patentes.", System.Diagnostics.TraceLevel.Warning);
            }
            else
            {
                // Agregar todas las patentes a la familia Administrador
                foreach (var patente in todasLasPatentes)
                {
                    familiaAdmin.Add(patente);
                }
                LoggerService.WriteLog($"Familia 'Administrador' creada con {todasLasPatentes.Count} patentes.", System.Diagnostics.TraceLevel.Info);
            }

            // Crear la familia en la base de datos
            _familiaDAL.CreateFamilia(familiaAdmin);

            return familiaAdmin;
        }

        /// <summary>
        /// Crea una patente siempre que no exista otra con el mismo nombre.
        /// </summary>
        public static void CreatePatente(Patente patente)
        {
            if (_UsuarioDAL.ExistePatente(patente.Nombre))
            {
                //LoggerService.WriteLog($"Intento de crear una familia con un nombre ya existente: {patente.Nombre}.", System.Diagnostics.TraceLevel.Warning);

                string messageKey = "Ya existe una familia con el mismo nombre.";
                string translatedMessage = TranslateMessageKey(messageKey);

                throw new Exception(translatedMessage);
            }

            _userLogic.CreatePatente(patente);
        }

        /// <summary>
        /// Deshabilita al usuario indicado.
        /// </summary>
        public static void DisableUser(Guid idUsuario)
        {
            _userLogic.DisableUser(idUsuario);
        }

        //public static void EnabledUsuario(Guid idUsuario)
        //{
        //    _userLogic.EnabledUsuario(idUsuario);
        //}

        /// <summary>
        /// Actualiza los accesos asignados a un usuario.
        /// </summary>
        public static void UpdateUserAccesos(Guid idUsuario, List<Acceso> accesos)
        {
            _userLogic.UpdateUserAccesos(idUsuario, accesos);
        }

        /// <summary>
        /// Devuelve todos los usuarios registrados.
        /// </summary>
        public static List<Usuario> GetAllUsuarios()
        {
            return _userLogic.GetAllUsuarios();
        }


        /// <summary>
        /// Obtiene un usuario por nombre.
        /// </summary>
        public static Usuario GetUsuarioByUsername(string username)
        {

            return _userLogic.GetUsuarioByUsername(username);
        }
        /// <summary>
        /// Lista los usuarios con el detalle de familias y patentes.
        /// </summary>
        public static List<UsuarioRolDto> GetUsuariosConFamilasYPatentes()
        {
            return _userLogic.GetUsuariosConFamilasYPatentes();
        }
        /// <summary>
        /// Traduce una clave de mensaje.
        /// </summary>
        private static string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }
        /// <summary>
        /// Valida el formato del correo electrónico.
        /// </summary>
        private static bool IsValidEmail(string email)
        {
            // Expresión regular para validar el formato del correo electrónico
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
        /// <summary>
        /// Guarda el idioma preferido del usuario.
        /// </summary>
        public static void SaveLenguaje(Guid idUsuario, string lenguaje)
        {
            _userLogic.SaveLenguaje(idUsuario, lenguaje);
        }


        /// <summary>
        /// Obtiene el idioma configurado para el usuario.
        /// </summary>
        public static string GetUserLenguaje(Guid idUsuario)
        {
            return _userLogic.GetUserLenguaje(idUsuario);
        }
    }
}

