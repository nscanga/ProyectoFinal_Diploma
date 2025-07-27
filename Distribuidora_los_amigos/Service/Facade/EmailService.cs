using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace Service.Facade
{
    public static class EmailService
    {
        private static string smtpServer = "smtp.gmail.com"; // Servidor SMTP de Gmail
        private static string smtpUser = "soporteclinicac@gmail.com"; // Cambia por tu email de Gmail
        private static string smtpPass = "pwknemritdinbhhy"; // Cambia por tu contraseña de Gmail //clinica7676
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
        private static string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }
    }
}
