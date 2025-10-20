using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using DOMAIN;

namespace Service.Facade
{
    public static class EmailService
    {
        private static string smtpServer = "smtp.gmail.com"; // Servidor SMTP de Gmail
        private static string smtpUser = "nico.sc294@gmail.com"; // Cambia por tu email de Gmail
        private static string smtpPass = "jrneoedsbnyrdrej"; // Cambia por tu contraseña de Gmail //clinica7676
        private static int smtpPort = 587; // Puerto para TLS/STARTTLS

        public static void SendRecoveryEmail(string toEmail, string recoveryToken)
        {
            string messageKey = "Recuperación de Contraseña";
            string translatedMessage = TranslateMessageKey(messageKey);
            string subject = translatedMessage;
            string messageKey1 = "Este token es válido por 10 minutos. Tu token de recuperación es: ";
            string translatedMessage1 = TranslateMessageKey(messageKey1);
            string body = translatedMessage1 + $" {recoveryToken} ";

            using (var message = new MailMessage(smtpUser, toEmail))
            {
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = false;

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    client.EnableSsl = true; // Habilitar SSL
                    client.Send(message);
                }
            }
        }

        // 🆕 NUEVA FUNCIONALIDAD: Enviar notificación de pedido en camino
        public static void EnviarNotificacionPedidoEnCamino(Pedido pedido, Cliente cliente)
        {
            try
            {
                // 🔍 DEBUGGING: Agregar logs detallados
                Console.WriteLine($"🔍 DEBUG - Iniciando envío de email:");
                Console.WriteLine($"   Cliente ID: {cliente?.IdCliente}");
                Console.WriteLine($"   Cliente Nombre: {cliente?.Nombre}");
                Console.WriteLine($"   Cliente Email CRUDO: '{cliente?.Email}'");
                Console.WriteLine($"   Email es null?: {cliente?.Email == null}");
                Console.WriteLine($"   Email está vacío?: {string.IsNullOrEmpty(cliente?.Email)}");

                if (cliente == null)
                {
                    throw new Exception("El cliente es nulo.");
                }

                if (string.IsNullOrEmpty(cliente.Email))
                {
                    throw new Exception("El cliente no tiene un email registrado.");
                }

                // 🔧 LIMPIAR Y VALIDAR EMAIL
                string emailLimpio = LimpiarEmail(cliente.Email);
                Console.WriteLine($"   Email LIMPIO: '{emailLimpio}'");

                if (!EsEmailValido(emailLimpio))
                {
                    throw new Exception($"El email del cliente no tiene un formato válido: '{emailLimpio}'");
                }

                string asunto = "🚚 Su pedido está en camino - Distribuidora Los Amigos";
                string cuerpoEmail = GenerarCuerpoEmailPedidoEnCamino(pedido, cliente);

                Console.WriteLine($"📧 Intentando enviar email a: {emailLimpio}");

                using (var message = new MailMessage())
                {
                    // 🔧 CONFIGURAR MANUALMENTE PARA EVITAR ERRORES
                    message.From = new MailAddress(smtpUser, "Distribuidora Los Amigos");
                    message.To.Add(new MailAddress(emailLimpio)); // 🎯 Usar email limpio
                    message.Subject = asunto;
                    message.Body = cuerpoEmail;
                    message.IsBodyHtml = true;
                    message.Priority = MailPriority.Normal;

                    using (var client = new SmtpClient(smtpServer, smtpPort))
                    {
                        client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                        client.EnableSsl = true;
                        client.Timeout = 30000; // 30 segundos de timeout
                        
                        Console.WriteLine($"📤 Enviando email...");
                        client.Send(message);
                        Console.WriteLine($"✅ Email enviado exitosamente a: {emailLimpio}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error detallado en envío de email: {ex.Message}");
                Console.WriteLine($"❌ Stack trace: {ex.StackTrace}");
                throw new Exception($"Error al enviar email de notificación: {ex.Message}", ex);
            }
        }

        // 🆕 Método para limpiar el email
        private static string LimpiarEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return string.Empty;

            // Eliminar espacios en blanco al inicio y final
            string emailLimpio = email.Trim();
            
            // Eliminar caracteres de control y espacios en el medio
            emailLimpio = Regex.Replace(emailLimpio, @"\s+", "");
            
            // Eliminar caracteres especiales que no deberían estar en un email
            emailLimpio = Regex.Replace(emailLimpio, @"[^\w@.-]", "");

            return emailLimpio;
        }

        // 🆕 Método para validar formato de email
        private static bool EsEmailValido(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                // Usar el constructor de MailAddress para validar
                var mailAddress = new MailAddress(email);
                
                // Validación adicional con regex
                string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                return Regex.IsMatch(email, patron);
            }
            catch
            {
                return false;
            }
        }

        // 🆕 Generar el contenido HTML del email para pedido en camino
        private static string GenerarCuerpoEmailPedidoEnCamino(Pedido pedido, Cliente cliente)
        {
            string idPedidoCorto = pedido.IdPedido.ToString().Substring(0, 8).ToUpper();
            
            return $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; margin: 0; padding: 20px; background-color: #f4f4f4; }}
                        .container {{ max-width: 600px; margin: 0 auto; background-color: white; padding: 30px; border-radius: 10px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }}
                        .header {{ text-align: center; color: #2c3e50; margin-bottom: 30px; }}
                        .content {{ color: #333; line-height: 1.6; }}
                        .pedido-info {{ background-color: #e8f4fd; padding: 15px; border-radius: 5px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 30px; color: #7f8c8d; font-size: 12px; }}
                        .logo {{ font-size: 24px; font-weight: bold; color: #3498db; }}
                        .estado {{ color: #27ae60; font-weight: bold; font-size: 18px; }}
                        .highlight {{ background-color: #fff3cd; padding: 10px; border-radius: 5px; border-left: 4px solid #ffc107; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <div class='logo'>📦 Distribuidora Los Amigos</div>
                            <h2>¡Su pedido está en camino!</h2>
                        </div>
                        
                        <div class='content'>
                            <p>Estimado/a <strong>{cliente.Nombre}</strong>,</p>
                            
                            <p>Nos complace informarle que su pedido ha sido despachado y se encuentra <span class='estado'>EN CAMINO</span> hacia su destino.</p>
                            
                            <div class='pedido-info'>
                                <h3>📋 Información del Pedido:</h3>
                                <p><strong>🆔 ID Pedido:</strong> {idPedidoCorto}</p>
                                <p><strong>📅 Fecha del Pedido:</strong> {pedido.FechaPedido:dd/MM/yyyy}</p>
                                <p><strong>💰 Total:</strong> {pedido.Total:C2}</p>
                                <p><strong>📍 Dirección de Entrega:</strong> {cliente.Direccion}</p>
                                <p><strong>📞 Teléfono de Contacto:</strong> {cliente.Telefono}</p>
                            </div>
                            
                            <div class='highlight'>
                                <h3>🚚 ¿Qué sigue ahora?</h3>
                                <ul>
                                    <li><strong>Tiempo estimado:</strong> Su pedido llegará en los próximos 2-3 días hábiles</li>
                                    <li><strong>Comunicación previa:</strong> Nuestro equipo de entrega se comunicará 30 minutos antes</li>
                                    <li><strong>Disponibilidad:</strong> Asegúrese de estar disponible en la dirección registrada</li>
                                    <li><strong>Documento:</strong> Tenga a mano su documento de identidad para la entrega</li>
                                </ul>
                            </div>
                            
                            <p><strong>📞 ¿Necesita cambiar algo?</strong><br>
                            Si necesita modificar la dirección de entrega o tiene alguna consulta, contáctenos inmediatamente.</p>
                            
                            <p>¡Gracias por confiar en Distribuidora Los Amigos!</p>
                            
                            <p style='color: #666; font-style: italic;'>
                                Este email fue enviado automáticamente el {DateTime.Now:dd/MM/yyyy} a las {DateTime.Now:HH:mm} hs.
                            </p>
                        </div>
                        
                        <div class='footer'>
                            <p><strong>📞 Atención al Cliente:</strong> (011) 1234-5678</p>
                            <p><strong>📧 Email:</strong> info@distribuidoralosamigos.com</p>
                            <p><strong>🕒 Horarios:</strong> Lunes a Viernes 8:00 - 18:00 hs</p>
                            <hr style='border: none; border-top: 1px solid #ddd; margin: 15px 0;'>
                            <p>Este es un email automático, por favor no responda a este mensaje.</p>
                            <p>&copy; 2024 Distribuidora Los Amigos. Todos los derechos reservados.</p>
                        </div>
                    </div>
                </body>
                </html>";
        }

        /// <summary>
        /// Envía notificación por email cuando el stock de un producto es bajo
        /// </summary>
        public static void EnviarNotificacionStockBajo(Producto producto, Stock stock, string emailAdministrador)
        {
            try
            {
                Console.WriteLine($"🔍 DEBUG - Enviando notificación de stock bajo:");
                Console.WriteLine($"   Producto: {producto?.Nombre}");
                Console.WriteLine($"   Stock actual: {stock?.Cantidad}");
                Console.WriteLine($"   Email admin: '{emailAdministrador}'");

                if (producto == null)
                {
                    throw new Exception("El producto es nulo.");
                }

                if (stock == null)
                {
                    throw new Exception("El stock es nulo.");
                }

                if (string.IsNullOrEmpty(emailAdministrador))
                {
                    throw new Exception("No hay email de administrador configurado.");
                }

                // Limpiar y validar email
                string emailLimpio = LimpiarEmail(emailAdministrador);
                
                if (!EsEmailValido(emailLimpio))
                {
                    throw new Exception($"El email del administrador no tiene un formato válido: '{emailLimpio}'");
                }

                string asunto = "⚠️ ALERTA: Stock Bajo - Distribuidora Los Amigos";
                string cuerpoEmail = GenerarCuerpoEmailStockBajo(producto, stock);

                Console.WriteLine($"📧 Enviando notificación de stock bajo a: {emailLimpio}");

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(smtpUser, "Sistema de Stock - Distribuidora Los Amigos");
                    message.To.Add(new MailAddress(emailLimpio));
                    message.Subject = asunto;
                    message.Body = cuerpoEmail;
                    message.IsBodyHtml = true;
                    message.Priority = MailPriority.High; // Prioridad alta para alertas

                    using (var client = new SmtpClient(smtpServer, smtpPort))
                    {
                        client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                        client.EnableSsl = true;
                        client.Timeout = 30000;
                        
                        client.Send(message);
                        Console.WriteLine($"✅ Notificación de stock bajo enviada exitosamente");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al enviar notificación de stock bajo: {ex.Message}");
                throw new Exception($"Error al enviar notificación de stock bajo: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Genera el cuerpo HTML del email para notificación de stock bajo
        /// </summary>
        private static string GenerarCuerpoEmailStockBajo(Producto producto, Stock stock)
        {
            string idProductoCorto = producto.IdProducto.ToString().Substring(0, 8).ToUpper();
            string nivelAlerta = stock.Cantidad <= 5 ? "CRÍTICO" : "BAJO";
            string colorAlerta = stock.Cantidad <= 5 ? "#dc3545" : "#ffc107";
            
            return $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; margin: 0; padding: 20px; background-color: #f4f4f4; }}
                        .container {{ max-width: 600px; margin: 0 auto; background-color: white; padding: 30px; border-radius: 10px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }}
                        .header {{ text-align: center; color: #2c3e50; margin-bottom: 30px; border-bottom: 3px solid {colorAlerta}; padding-bottom: 15px; }}
                        .content {{ color: #333; line-height: 1.6; }}
                        .alert-box {{ background-color: #fff3cd; padding: 20px; border-radius: 5px; margin: 20px 0; border-left: 5px solid {colorAlerta}; }}
                        .producto-info {{ background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 30px; color: #7f8c8d; font-size: 12px; border-top: 1px solid #ddd; padding-top: 15px; }}
                        .logo {{ font-size: 24px; font-weight: bold; color: {colorAlerta}; }}
                        .nivel-alerta {{ color: {colorAlerta}; font-weight: bold; font-size: 20px; }}
                        .cantidad {{ font-size: 32px; color: {colorAlerta}; font-weight: bold; }}
                        .accion {{ background-color: #d1ecf1; padding: 15px; border-radius: 5px; border-left: 4px solid #0c5460; margin: 15px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <div class='logo'>⚠️ Sistema de Alertas de Stock</div>
                            <h2>Notificación de Stock {nivelAlerta}</h2>
                        </div>
                        
                        <div class='content'>
                            <div class='alert-box'>
                                <h2 style='margin-top: 0; color: {colorAlerta};'>⚠️ ATENCIÓN: STOCK {nivelAlerta}</h2>
                                <p style='font-size: 18px;'>El siguiente producto tiene un nivel de stock {nivelAlerta.ToLower()} y requiere atención inmediata.</p>
                                <p style='text-align: center; margin: 20px 0;'>
                                    <span style='font-size: 16px;'>Stock Actual:</span><br>
                                    <span class='cantidad'>{stock.Cantidad}</span> <span style='font-size: 18px;'>{stock.Tipo}</span>
                                </p>
                            </div>
                            
                            <div class='producto-info'>
                                <h3>📦 Información del Producto:</h3>
                                <p><strong>🆔 ID Producto:</strong> {idProductoCorto}</p>
                                <p><strong>📝 Nombre:</strong> {producto.Nombre}</p>
                                <p><strong>📂 Categoría:</strong> {producto.Categoria}</p>
                                <p><strong>💰 Precio:</strong> {producto.Precio:C2}</p>
                                <p><strong>📊 Tipo de Stock:</strong> {stock.Tipo}</p>
                                <p><strong>📅 Fecha de Ingreso:</strong> {producto.FechaIngreso:dd/MM/yyyy}</p>
                                {(producto.Vencimiento.HasValue ? $"<p><strong>⏰ Vencimiento:</strong> {producto.Vencimiento.Value:dd/MM/yyyy}</p>" : "")}
                            </div>
                            
                            <div class='accion'>
                                <h3>✅ Acciones Recomendadas:</h3>
                                <ul>
                                    <li><strong>Verificar demanda:</strong> Revisar el historial de ventas del producto</li>
                                    <li><strong>Contactar proveedor:</strong> Realizar pedido de reposición urgente</li>
                                    <li><strong>Evaluar alternativas:</strong> Considerar productos sustitutos temporales</li>
                                    <li><strong>Actualizar sistema:</strong> Registrar el pedido de reposición</li>
                                </ul>
                            </div>
                            
                            <p style='color: #666; font-style: italic; text-align: center; margin-top: 30px;'>
                                Esta alerta fue generada automáticamente el {DateTime.Now:dd/MM/yyyy} a las {DateTime.Now:HH:mm} hs.
                            </p>
                        </div>
                        
                        <div class='footer'>
                            <p><strong>📞 Soporte Técnico:</strong> (011) 1234-5678</p>
                            <p><strong>📧 Email:</strong> info@distribuidoralosamigos.com</p>
                            <p>&copy; 2024 Distribuidora Los Amigos - Sistema de Gestión de Stock</p>
                        </div>
                    </div>
                </body>
                </html>";
        }

        private static string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }
    }
}
