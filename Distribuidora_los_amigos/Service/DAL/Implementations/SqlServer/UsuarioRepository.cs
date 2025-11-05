using Service.DAL.Contracts;
using Service.DOMAIN;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Service.DOMAIN.DTO;

namespace Service.DAL.Implementations.SqlServer.Helpers
{
    /// <summary>
    /// Repositorio responsable de gestionar la persistencia de usuarios y sus accesos en SQL Server.
    /// </summary>
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly FamiliaRepository _familiaRepository;

        /// <summary>
        /// Inicializa el repositorio de usuarios con el repositorio de familias asociado.
        /// </summary>
        public UsuarioRepository()
        {
            _familiaRepository = new FamiliaRepository();
        }
        /// <summary>
        /// Inserta un nuevo usuario con estado habilitado y datos principales.
        /// </summary>
        /// <param name="usuario">Entidad con la información a persistir.</param>
        public void CreateUsuario(Usuario usuario)
        {
            SqlHelper.ExecuteNonQuery(
                "INSERT INTO Usuario (IdUsuario, UserName, Password, Estado, Email) VALUES (@IdUsuario, @UserName, @Password, @Estado, @Email)",
                CommandType.Text,
                new SqlParameter("@IdUsuario", usuario.IdUsuario),
                new SqlParameter("@UserName", usuario.UserName),
                new SqlParameter("@Password", usuario.Password),
                new SqlParameter("@Estado", (int)EstadoUsuario.Habilitado),  // Asignar estado por defecto como Habilitado
                new SqlParameter("@Email", usuario.Email)
            );
        }

        /// <summary>
        /// Deshabilita un usuario marcando su estado en la base de datos.
        /// </summary>
        /// <param name="idUsuario">Identificador del usuario a desactivar.</param>
        public void DisableUsuario(Guid idUsuario)
        {
            SqlHelper.ExecuteNonQuery(
                "UPDATE Usuario SET Estado = 0 WHERE IdUsuario = @IdUsuario",
                CommandType.Text,
                new SqlParameter("@IdUsuario", idUsuario)
            );
        }

        /// <summary>
        /// Habilita nuevamente a un usuario cambiando su estado en la base de datos.
        /// </summary>
        /// <param name="idUsuario">Identificador del usuario a habilitar.</param>
        public void EnabledUsuario(Guid idUsuario)
        {
            SqlHelper.ExecuteNonQuery(
                "UPDATE Usuario SET Estado = 1 WHERE IdUsuario = @IdUsuario",
                CommandType.Text,
                new SqlParameter("@IdUsuario", idUsuario)
            );
        }

        /// <summary>
        /// Reemplaza las patentes asignadas a un usuario por un nuevo conjunto de accesos.
        /// </summary>
        /// <param name="idUsuario">Usuario al que se aplicarán los cambios.</param>
        /// <param name="accesos">Colección de accesos a vincular.</param>
        public void UpdateAccesos(Guid idUsuario, List<Acceso> accesos)
        {
            // Primero eliminamos los accesos existentes
            SqlHelper.ExecuteNonQuery(
                "DELETE FROM Usuario_Patente WHERE IdUsuario = @IdUsuario",
                CommandType.Text,
                new SqlParameter("@IdUsuario", idUsuario)
            );

            // Luego insertamos los nuevos accesos
            foreach (var acceso in accesos)
            {
                SqlHelper.ExecuteNonQuery(
                    "INSERT INTO Usuario_Patente (IdUsuario, IdPatente) VALUES (@IdUsuario, @IdPatente)",
                    CommandType.Text,
                    new SqlParameter("@IdUsuario", idUsuario),
                    new SqlParameter("@IdPatente", acceso.Id)
                );
            }
        }
        /// <summary>
        /// Recupera un usuario por su nombre, incluyendo las familias asociadas.
        /// </summary>
        /// <param name="username">Nombre de usuario sensible a mayúsculas.</param>
        /// <returns>Entidad usuario completa o <c>null</c> si no existe.</returns>
         public Usuario GetUsuarioByUsername(string username)
        {
            Usuario usuario = null;

            // Primero, obtenemos los datos básicos del usuario
            using (SqlDataReader reader = SqlHelper.ExecuteReader(
                "SELECT IdUsuario, UserName, Password, Estado FROM Usuario WHERE UserName COLLATE Latin1_General_CS_AS = @UserName",
                CommandType.Text,
                new SqlParameter("@UserName", username)))
            {
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        IdUsuario = reader.GetGuid(0),
                        UserName = reader.GetString(1),
                        Password = reader.GetString(2),
                        Estado = reader.GetInt32(3).ToString()
                    };
                }
            }

            if (usuario != null)
            {
                // Obtener las familias del usuario
                List<Familia> familias = GetFamiliasByUsuarioId(usuario.IdUsuario);

                // Asignar las familias (con sus patentes) al usuario
                usuario.Accesos.AddRange(familias);
            }

            return usuario;
        }

        /// <summary>
        /// Obtiene un usuario mediante su identificador, incorporando las familias relacionadas.
        /// </summary>
        /// <param name="idUsuario">Identificador del usuario buscado.</param>
        /// <returns>Entidad usuario o <c>null</c> si no se encuentra.</returns>
        public Usuario GetUsuarioById(Guid idUsuario)
        {
            Usuario usuario = null;

            // Primero, obtenemos los datos básicos del usuario
            using (SqlDataReader reader = SqlHelper.ExecuteReader(
                "SELECT IdUsuario, UserName, Password, Estado FROM Usuario WHERE IdUsuario = @IdUsuario",
                CommandType.Text,
                new SqlParameter("@IdUsuario", idUsuario)))
            {
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        IdUsuario = reader.GetGuid(0),
                        UserName = reader.GetString(1),
                        Password = reader.GetString(2),
                        Estado = reader.GetInt32(3).ToString()
                    };
                }
            }

            if (usuario != null)
            {
                // Obtener las familias del usuario
                List<Familia> familias = GetFamiliasByUsuarioId(usuario.IdUsuario);

                // Asignar las familias (con sus patentes) al usuario
                usuario.Accesos.AddRange(familias);
            }

            return usuario;
        }
        /// <summary>
        /// Construye la lista de familias asociadas a un usuario incluyendo sus patentes.
        /// </summary>
        /// <param name="usuarioId">Identificador del usuario.</param>
        /// <returns>Colección de familias con sus accesos.</returns>
        private List<Familia> GetFamiliasByUsuarioId(Guid usuarioId)
        {
            List<Familia> familias = new List<Familia>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(
                "SELECT f.IdFamilia, f.Nombre FROM Familia f " +
                "INNER JOIN Usuario_Familia uf ON f.IdFamilia = uf.IdFamilia " +
                "WHERE uf.IdUsuario = @IdUsuario",
                CommandType.Text,
                new SqlParameter("@IdUsuario", usuarioId)))
            {
                while (reader.Read())
                {
                    var familia = new Familia
                    {
                        Id = reader.GetGuid(0),
                        Nombre = reader.GetString(1)
                    };

                    // Obtener las patentes asociadas a esta familia
                    familia.Accesos.AddRange(_familiaRepository.GetPatentesByFamiliaId(familia.Id));

                    // Añadir la familia a la lista
                    familias.Add(familia);
                }
            }

            return familias;
        }
        /// <summary>
        /// Agrega una nueva patente al catálogo disponible.
        /// </summary>
        /// <param name="patente">Patente a registrar.</param>
        public void CreatePatente(Patente patente)
        {
            SqlHelper.ExecuteNonQuery(
                "INSERT INTO Patente (IdPatente, Nombre, DataKey, TipoAcceso) VALUES (@IdPatente, @Nombre, @DataKey, @TipoAcceso)",
                CommandType.Text,
                new SqlParameter("@IdPatente", patente.Id),
                new SqlParameter("@Nombre", patente.Nombre),
                new SqlParameter("@DataKey", patente.DataKey),
                new SqlParameter("@TipoAcceso", (int)patente.TipoAcceso) // Convertimos el enum a entero
            );
        }

        /// <summary>
        /// Verifica si una patente ya está registrada por nombre.
        /// </summary>
        /// <param name="nombrePatente">Nombre a validar.</param>
        /// <returns><c>true</c> si existe al menos una coincidencia.</returns>
        public bool ExistePatente(string nombrePatente)
        {
            var query = "SELECT COUNT(*) FROM Patente WHERE Nombre = @Nombre";
            var count = (int)SqlHelper.ExecuteScalar(query, CommandType.Text, new SqlParameter("@Nombre", nombrePatente));
            return count > 0;
        }
       


        /// <summary>
        /// Implementación pendiente para agregar un usuario genérico del contrato base.
        /// </summary>
        /// <param name="obj">Entidad de usuario a agregar.</param>
        public void Add(Usuario obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implementación pendiente para actualizar un usuario mediante el contrato genérico.
        /// </summary>
        /// <param name="obj">Entidad con los datos modificados.</param>
        public void Update(Usuario obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implementación pendiente para eliminar un usuario según el contrato genérico.
        /// </summary>
        /// <param name="id">Identificador del usuario a eliminar.</param>
        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implementación pendiente para obtener un usuario genérico por identificador.
        /// </summary>
        /// <param name="id">Identificador buscado.</param>
        /// <returns>Usuario correspondiente cuando se implemente.</returns>
        public Usuario GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene todos los usuarios registrando únicamente sus datos básicos.
        /// </summary>
        /// <returns>Listado de usuarios.</returns>
        public List<Usuario> GetAll()
        {
            var usuarios = new List<Usuario>();

            // Ejecutar la consulta SQL para obtener todos los usuarios
            using (var reader = SqlHelper.ExecuteReader(
                "SELECT IdUsuario, UserName, Password, Estado FROM Usuario",
                CommandType.Text))
            {
                // Leer cada fila del resultado
                while (reader.Read())
                {
                    // Crear un nuevo objeto Usuario y asignar los valores de la base de datos
                    var usuario = new Usuario
                    {
                        IdUsuario = reader.GetGuid(reader.GetOrdinal("IdUsuario")),
                        UserName = reader.GetString(reader.GetOrdinal("UserName")),
                        Password = reader.GetString(reader.GetOrdinal("Password")),
                      
                    };

                    // Agregar el usuario a la lista
                    usuarios.Add(usuario);
                }
            }

            // Devolver la lista de usuarios
            return usuarios;
        }
        /// <summary>
        /// Determina si un usuario posee alguna patente del tipo requerido.
        /// </summary>
        /// <param name="idUsuario">Identificador del usuario.</param>
        /// <param name="tipoAcceso">Tipo de acceso a validar.</param>
        /// <returns><c>true</c> cuando el usuario tiene la patente solicitada.</returns>
        public bool HasAccess(Guid idUsuario, TipoAcceso tipoAcceso)
        {
            // Obtener el usuario con sus accesos (patentes)
            var usuario = GetUsuarioById(idUsuario);

            if (usuario == null)
            {
                return false;
            }

            // Verificar si alguna de las patentes del usuario tiene el TipoAcceso requerido
            return usuario.Accesos
                .OfType<Patente>()
                .Any(patente => patente.TipoAcceso == tipoAcceso);
        }

        /// <summary>
        /// Obtiene la relación de usuarios junto con sus familias asignadas.
        /// </summary>
        /// <returns>Listado de usuarios con sus roles.</returns>
        public List<UsuarioRolDto> GetUsuariosConFamilasYPatentes()
        {
            var usuariosRoles = new List<UsuarioRolDto>();

            // Consulta SQL compatible con SQL Server 2008 y superiores
            // Agrupa las familias por usuario usando STUFF y FOR XML PATH
            string query = @"
            SELECT 
                u.UserName AS Usuario,
                STUFF((
                    SELECT DISTINCT ', ' + f2.Nombre
                    FROM Usuario_Familia uf2
                    INNER JOIN Familia f2 ON uf2.IdFamilia = f2.IdFamilia
                    WHERE uf2.IdUsuario = u.IdUsuario
                    FOR XML PATH(''), TYPE
                ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS Familias,
                u.Estado
            FROM Usuario u
            GROUP BY u.IdUsuario, u.UserName, u.Estado
            ORDER BY u.UserName
            ";

            using (var reader = SqlHelper.ExecuteReader(query, CommandType.Text))
            {
                while (reader.Read())
                {
                    var familias = reader.IsDBNull(reader.GetOrdinal("Familias")) 
                        ? "Sin rol asignado" 
                        : reader.GetString(reader.GetOrdinal("Familias"));

                    var usuarioRolDto = new UsuarioRolDto
                    {
                        Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                        Familia = familias,
                        Estado = reader.GetInt32(reader.GetOrdinal("Estado")) == 1 ? "Habilitado" : "Deshabilitado"
                    };

                    usuariosRoles.Add(usuarioRolDto);
                }
            }

            return usuariosRoles;
        }

        /// <summary>
        /// Recupera un usuario incluyendo datos de contacto y tokens de recuperación.
        /// </summary>
        /// <param name="username">Nombre de usuario a buscar.</param>
        /// <returns>Usuario con información ampliada o <c>null</c>.</returns>
        public Usuario GetUsuarioCompletos(string username)
        {
            Usuario usuario = null;

            // Obtenemos los datos del usuario SIN la columna Language por ahora
            using (SqlDataReader reader = SqlHelper.ExecuteReader(
                "SELECT IdUsuario, UserName, Password, Estado, Email, RecoveryToken, TokenExpiration FROM Usuario WHERE UserName = @UserName",
                CommandType.Text,
                new SqlParameter("@UserName", username)))
            {
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        IdUsuario = reader.GetGuid(0),
                        UserName = reader.GetString(1),
                        Password = reader.GetString(2),
                        Estado = reader.GetInt32(3).ToString(),  // Convertir el estado (1/0) a String
                        Email = reader.IsDBNull(4) ? null : reader.GetString(4),  // Verificar si el email es NULL
                        RecoveryToken = reader.IsDBNull(5) ? null : reader.GetString(5),  // Verificar si el RecoveryToken es NULL
                        TokenExpiration = reader.IsDBNull(6) ? DateTime.MinValue : reader.GetDateTime(6),   // Verificar si TokenExpiration es NULL
                        Lenguaje = "es-ES" // Valor por defecto hasta que agreguemos la columna correctamente
                    };
                }
            }

            return usuario;
        }

        /// <summary>
        /// Actualiza el token y su vencimiento para la recuperación de contraseña de un usuario.
        /// </summary>
        /// <param name="usuario">Usuario con los nuevos valores de token.</param>
        public void UpdateUsuarioToken(Usuario usuario)
        {
            // Comando SQL para actualizar el usuario
            string query = @"
        UPDATE Usuario 
        SET RecoveryToken = @RecoveryToken, 
            TokenExpiration = @TokenExpiration 
        WHERE IdUsuario = @IdUsuario";

            // Ejecutamos el comando utilizando SqlHelper
            SqlHelper.ExecuteNonQuery(
                query,
                CommandType.Text,
                new SqlParameter("@RecoveryToken", usuario.RecoveryToken),
                new SqlParameter("@TokenExpiration", usuario.TokenExpiration),
                new SqlParameter("@IdUsuario", usuario.IdUsuario)
            );
        }

        /// <summary>
        /// Cambia la contraseña almacenada para el usuario indicado.
        /// </summary>
        /// <param name="usuario">Usuario con la nueva contraseña encriptada.</param>
        public void UpdatePassword(Usuario usuario)
        {
            string query = @"
        UPDATE Usuario
        SET Password = @Password 
        WHERE IdUsuario = @IdUsuario";

            SqlHelper.ExecuteNonQuery(
                query,
                CommandType.Text,
                new SqlParameter("@Password", usuario.Password),
                new SqlParameter("@IdUsuario", usuario.IdUsuario)
            );
        }
        /// <summary>
        /// Actualiza el idioma preferido del usuario si la columna existe.
        /// </summary>
        /// <param name="idUsuario">Identificador del usuario.</param>
        /// <param name="lenguaje">Código de idioma a asignar.</param>
        public void UpdateLenguaje(Guid idUsuario, string lenguaje)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(
                    "UPDATE Usuario SET UserLanguage = @UserLanguage WHERE IdUsuario = @IdUsuario",
                    CommandType.Text,
                    new SqlParameter("@IdUsuario", idUsuario),
                    new SqlParameter("@UserLanguage", lenguaje)
                );
            }
            catch (SqlException)
            {
                // Si la columna UserLanguage no existe, simplemente no hacer nada por ahora
                // TODO: Agregar columna UserLanguage a la base de datos
            }
        }
        
        /// <summary>
        /// Obtiene el código de idioma configurado para un usuario o devuelve el valor por defecto.
        /// </summary>
        /// <param name="idUsuario">Identificador del usuario.</param>
        /// <returns>Código de cultura almacenado.</returns>
        public string GetUserLenguaje(Guid idUsuario)
        {
            try
            {
                string lenguaje = string.Empty;
                SqlDataReader reader = SqlHelper.ExecuteReader(
                    "SELECT UserLanguage FROM Usuario WHERE IdUsuario = @IdUsuario",
                    CommandType.Text,
                    new SqlParameter("@IdUsuario", idUsuario)
                );

                if (reader.Read())
                {
                    lenguaje = reader["UserLanguage"].ToString();
                }
                reader.Close();

                return lenguaje;
            }
            catch (SqlException)
            {
                // Si la columna UserLanguage no existe, devolver idioma por defecto
                return "es-ES";
            }
        }

        /// <summary>
        /// Recupera un usuario con sus datos extendidos tolerando la falta de columnas opcionales.
        /// </summary>
        /// <param name="idUsuario">Identificador del usuario buscado.</param>
        /// <returns>Usuario encontrado o <c>null</c> si no existe.</returns>
        public Usuario ObetenerUsuarioById(Guid idUsuario)
        {
            Usuario usuario = null;

            try
            {
                // Intentar primero con todas las columnas incluyendo Language
                using (SqlDataReader reader = SqlHelper.ExecuteReader(
                "SELECT IdUsuario, UserName, Password, Estado, Email, RecoveryToken, TokenExpiration FROM Usuario WHERE IdUsuario = @IdUsuario",
                    CommandType.Text,
                    new SqlParameter("@IdUsuario", idUsuario)))
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            IdUsuario = reader.GetGuid(0),
                            UserName = reader.GetString(1),
                            Password = reader.GetString(2),
                            Estado = reader.GetInt32(3).ToString(),  // Convertir el estado (1/0) a String
                            Email = reader.IsDBNull(4) ? null : reader.GetString(4),  // Verificar si el email es NULL
                            RecoveryToken = reader.IsDBNull(5) ? null : reader.GetString(5),  // Verificar si el RecoveryToken es NULL
                            TokenExpiration = reader.IsDBNull(6) ? DateTime.MinValue : reader.GetDateTime(6),   // Verificar si TokenExpiration es NULL
                        Lenguaje = "es-ES" // Valor por defecto temporal
                        };
                    }
                }
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid column name 'UserLanguage'"))
            {
                // Si falla por la columna UserLanguage, usar consulta sin esa columna
                using (SqlDataReader reader = SqlHelper.ExecuteReader(
                    "SELECT IdUsuario, UserName, Password, Estado, Email, RecoveryToken, TokenExpiration FROM Usuario WHERE IdUsuario = @IdUsuario",
                    CommandType.Text,
                    new SqlParameter("@IdUsuario", idUsuario)))
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            IdUsuario = reader.GetGuid(0),
                            UserName = reader.GetString(1),
                            Password = reader.GetString(2),
                            Estado = reader.GetInt32(3).ToString(),
                            Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                            RecoveryToken = reader.IsDBNull(5) ? null : reader.GetString(5),
                            TokenExpiration = reader.IsDBNull(6) ? DateTime.MinValue : reader.GetDateTime(6),
                            Lenguaje = "es-ES" // Valor por defecto
                        };
                    }
                }
            }

            return usuario;
        }
        /// <summary>
        /// Lista todos los usuarios disponibles incluyendo datos de recuperación cuando están presentes.
        /// </summary>
        /// <returns>Colección de usuarios.</returns>
        public List<Usuario> GetAllUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                // Consulta simplificada para obtener todos los usuarios (sin dependencia de DigitosVerificadores)
                string query = @"
            SELECT u.IdUsuario, u.UserName, u.Password, u.Estado, u.Email, u.RecoveryToken, 
                   u.TokenExpiration
            FROM Usuario u";

                using (SqlDataReader reader = SqlHelper.ExecuteReader(query, CommandType.Text))
                {
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario
                        {
                            IdUsuario = reader.GetGuid(0),
                            UserName = reader.GetString(1),
                            Password = reader.GetString(2),
                            Estado = reader.GetInt32(3).ToString(),
                            Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                            RecoveryToken = reader.IsDBNull(5) ? null : reader.GetString(5),
                            TokenExpiration = reader.IsDBNull(6) ? DateTime.MinValue : reader.GetDateTime(6),
                            Lenguaje = "es-ES" // Valor por defecto
                        });
                    }
                }
            }
            catch (SqlException ex) when (ex.Message.Contains("Invalid column name"))
            {
                // Si falla por alguna columna que no existe, usar consulta con solo campos básicos
                string queryFallback = @"
            SELECT u.IdUsuario, u.UserName, u.Password, u.Estado, u.Email
            FROM Usuario u";

                using (SqlDataReader reader = SqlHelper.ExecuteReader(queryFallback, CommandType.Text))
                {
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario
                        {
                            IdUsuario = reader.GetGuid(0),
                            UserName = reader.GetString(1),
                            Password = reader.GetString(2),
                            Estado = reader.GetInt32(3).ToString(),
                            Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                            RecoveryToken = null,
                            TokenExpiration = DateTime.MinValue,
                            Lenguaje = "es-ES" // Valor por defecto
                        });
                    }
                }
            }

            return usuarios;
        }
    }
    }

