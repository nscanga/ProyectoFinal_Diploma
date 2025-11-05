using Service.Facade;
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
            MessageBox.Show(translatedMessage + "\n\nDetalles técnicos: " + ex.Message, translatedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Traduce una clave de mensaje utilizando el servicio de idiomas.
        /// </summary>
        private  static string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }
    }
}
