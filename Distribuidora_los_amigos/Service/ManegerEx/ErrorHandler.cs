using Service.Facade;
using Services.Facade;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service.ManegerEx
{
    /// <summary>
    /// Centraliza el manejo de excepciones mostrando mensajes localizados al usuario.
    /// </summary>
    public static class ErrorHandler
    {
        /// <summary>
        /// Gestiona errores SQL mostrando un mensaje amigable según el código recibido.
        /// También registra el error en la bitácora.
        /// </summary>
        public static void HandleSqlException(SqlException ex, string username)
        {
            string messageKey = "";
            switch (ex.Number)
            {
                case 2627: // Violación de Clave Única
                    messageKey = "Error: No se puede crear el usuario porque ya existe un usuario con el mismo nombre.";
                    break;

                case 547: // Violación de Clave Foránea
                    messageKey = "Error: La operación viola una restricción de clave foránea.";
                    break;

                case 515: // Violación de Restricción de Null
                    messageKey = "Error: Se ha intentado insertar un valor NULL en una columna que no permite valores NULL.";
                    break;

                case 1205: // Error de Transacción Fallida
                    messageKey = "Error: La transacción ha fallado debido a un bloqueo.";
                    break;

                case 229: // Error de Permisos Insuficientes
                    messageKey = "Error: El usuario no tiene permisos suficientes para realizar esta operación.";
                    break;

                case 4060: // No se puede abrir la base de datos
                    messageKey = "Error: No se puede abrir la base de datos. Verifique que la base de datos exista y esté accesible.";
                    break;

                case 18456: // Error de autenticación
                    messageKey = "Error: Fallo en la autenticación con el servidor de base de datos. Verifique las credenciales de conexión.";
                    break;

                case -1: // Tiempo de espera agotado
                    messageKey = "Error: Se agotó el tiempo de espera al conectar con la base de datos. Verifique la conexión de red.";
                    break;

                case 2: // Error de red
                    messageKey = "Error: No se puede establecer conexión con el servidor de base de datos. Verifique que el servidor esté disponible.";
                    break;

                default:
                    messageKey = "Error al realizar la operación.";
                    break;
            }

            string translatedMessage = TranslateMessageKey(messageKey);
            string titleKey = "Error SQL";
            string translatedTitle = TranslateMessageKey(titleKey);
            
            // Registrar en bitácora
            LogError(ex, username, translatedMessage);
            
            MessageBox.Show(translatedMessage + "\n\nDetalles técnicos: " + ex.Message, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Maneja errores de conexión SQL de forma genérica.
        /// Útil para manejar excepciones desde capas que no tienen acceso a DAL.
        /// </summary>
        /// <param name="ex">Excepción SQL o DatabaseException.</param>
        /// <param name="username">Usuario actual.</param>
        /// <param name="showMessageBox">Indica si se debe mostrar mensaje al usuario.</param>
        public static void HandleDatabaseException(Exception ex, string username, bool showMessageBox = true)
        {
            string messageKey = "Error: No se puede establecer conexión con el servidor de base de datos. Verifique que el servidor esté disponible.";
            string translatedMessage = TranslateMessageKey(messageKey);

            // Registrar en bitácora (intentar con fallback a archivo)
            try
            {
                LogError(ex, username, translatedMessage);
            }
            catch
            {
                // Si falla el registro en BD, intentar escribir en archivo
                try
                {
                    LoggerService.WriteException(ex);
                }
                catch
                {
                    // Último recurso: ignorar si no se puede registrar
                }
            }

            // Determinar si es un error recuperable
            bool isRecoverable = IsRecoverableError(ex);

            // Solo mostrar mensaje si se indica explícitamente
            if (showMessageBox)
            {
                if (isRecoverable)
                {
                    // Mensaje más suave para errores recuperables
                    string titleKey = "Advertencia";
                    string translatedTitle = TranslateMessageKey(titleKey);
                    MessageBox.Show(translatedMessage + "\n\nLa aplicación continuará funcionando con funcionalidad limitada.", 
                        translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // Mensaje de error para errores críticos
                    string titleKey = "Error de Base de Datos";
                    string translatedTitle = TranslateMessageKey(titleKey);
                    MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Determina si una excepción representa un error recuperable.
        /// </summary>
        private static bool IsRecoverableError(Exception ex)
        {
            // Verificar si es una DatabaseException de BLL
            if (ex.GetType().Name == "DatabaseException")
            {
                // Usar reflexión para obtener el ErrorType
                var errorTypeProp = ex.GetType().GetProperty("ErrorType");
                if (errorTypeProp != null)
                {
                    var errorType = errorTypeProp.GetValue(ex);
                    string errorTypeName = errorType?.ToString() ?? "";
                    
                    // Errores recuperables: ConnectionFailed, Timeout, NetworkError
                    return errorTypeName == "ConnectionFailed" || 
                           errorTypeName == "Timeout" || 
                           errorTypeName == "NetworkError";
                }
            }
            
            // Si es SqlException directa
            if (ex is SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case -1: // Timeout
                    case -2:
                    case 2: // Error de red
                    case 10053:
                    case 10054:
                    case 10060:
                    case 10061:
                    case 1205: // Deadlock
                        return true;
                    default:
                        return false;
                }
            }
            
            // Verificar InnerException
            if (ex.InnerException != null)
            {
                return IsRecoverableError(ex.InnerException);
            }
            
            return false;
        }

        /// <summary>
        /// Devuelve una descripción localizada para un error SQL concreto.
        /// </summary>
        public static string FormatSqlException(SqlException ex, string username)
        {
            string messageKey = "";
            switch (ex.Number)
            {
                case 2627: // Violación de Clave Única
                    messageKey = "Error: No se puede crear el usuario porque ya existe un usuario con el mismo nombre.";
                    break;
                case 547: // Violación de Clave Foránea
                    messageKey = "Error: La operación viola una restricción de clave foránea.";
                    break;
                case 515: // Violación de Restricción de Null
                    messageKey = "Error: Se ha intentado insertar un valor NULL en una columna que no permite valores NULL.";
                    break;
                case 1205: // Error de Transacción Fallida
                    messageKey = "Error: La transacción ha fallado debido a un bloqueo.";
                    break;
                case 229: // Error de Permisos Insuficientes
                    messageKey = "Error: El usuario no tiene permisos suficientes para realizar esta operación.";
                    break;
                case 4060: // No se puede abrir la base de datos
                    messageKey = "Error: No se puede abrir la base de datos. Verifique que la base de datos exista y esté accesible.";
                    break;
                case 18456: // Error de autenticación
                    messageKey = "Error: Fallo en la autenticación con el servidor de base de datos. Verifique las credenciales de conexión.";
                    break;
                case -1: // Tiempo de espera agotado
                    messageKey = "Error: Se agotó el tiempo de espera al conectar con la base de datos. Verifique la conexión de red.";
                    break;
                case 2: // Error de red
                    messageKey = "Error: No se puede establecer conexión con el servidor de base de datos. Verifique que el servidor esté disponible.";
                    break;
                default:
                    messageKey = "Error al realizar la operación.";
                    break;
            }

            return TranslateMessageKey(messageKey);
        }

        /// <summary>
        /// Muestra un mensaje general para excepciones no controladas.
        /// </summary>
        public static void HandleGeneralException(Exception ex)
        {
            string messageKey = "Error inesperado:";
            string translatedMessage = TranslateMessageKey(messageKey);
            string titleKey = "Error";
            string translatedTitle = TranslateMessageKey(titleKey);
            
            // Registrar en bitácora
            try
            {
                LoggerService.WriteException(ex);
            }
            catch
            {
                // Ignorar si falla el registro
            }
            
            MessageBox.Show(translatedMessage + " " + ex.Message, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Maneja el caso específico cuando no hay usuarios en el sistema.
        /// </summary>
        public static void HandleNoUsersFound()
        {
            string messageKey = "No se encontraron usuarios en el sistema. Se debe crear un usuario administrador antes de continuar.";
            string translatedMessage = TranslateMessageKey(messageKey);
            string titleKey = "Sistema sin usuarios";
            string translatedTitle = TranslateMessageKey(titleKey);
            MessageBox.Show(translatedMessage, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Registra un error en la bitácora.
        /// </summary>
        /// <param name="ex">Excepción a registrar.</param>
        /// <param name="username">Usuario actual.</param>
        /// <param name="customMessage">Mensaje personalizado opcional.</param>
        private static void LogError(Exception ex, string username, string customMessage = null)
        {
            try
            {
                string message = customMessage ?? ex.Message;
                string details = $"Usuario: {username ?? "Desconocido"}\nTipo: {ex.GetType().Name}\nMensaje: {ex.Message}";
                
                if (ex.InnerException != null)
                {
                    details += $"\nInnerException: {ex.InnerException.Message}";
                }

                LoggerService.WriteLog(message, System.Diagnostics.TraceLevel.Error);
                LoggerService.WriteException(ex);
            }
            catch
            {
                // Si falla el registro, no propagamos el error para no afectar la funcionalidad principal
            }
        }

        /// <summary>
        /// Traduce una clave de mensaje utilizando el servicio de idiomas.
        /// </summary>
        private static string TranslateMessageKey(string messageKey)
        {
            try
            {
                return IdiomaService.Translate(messageKey);
            }
            catch
            {
                // Si falla la traducción, devolver el mensaje original
                return messageKey;
            }
        }
    }
}
