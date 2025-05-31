using CafeVirtual.Pruebas.Utilidades.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;
using CafeVirtual.Pruebas.Utilidades.Model;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CafeVirtual.Pruebas.Utilidades.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _emailSettings;

        public EmailService(IOptions<SmtpSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmailAsync(string destinatario, string asunto, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_emailSettings.FromEmail));
            message.To.Add(MailboxAddress.Parse(destinatario));
            message.Subject = asunto;

            message.Body = new TextPart("html")
            {
                Text = htmlBody
            };

            try
            {
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.SslOnConnect);
                await smtp.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.AppPassword);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendEmailAdjuntosAsync(string destinatario, string asunto, AlternateView htmlView, byte[] adjuntoContent, string nombreAdjunto)
        {
            try
            {
                using var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(_emailSettings.FromEmail, _emailSettings.AppPassword),
                    EnableSsl = true
                };

                var mail = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromEmail),
                    Subject = asunto,
                    IsBodyHtml = true
                };

                mail.To.Add(destinatario);
                mail.AlternateViews.Add(htmlView);

                // Agregar el archivo adjunto
                var attachment = new Attachment(new MemoryStream(adjuntoContent), nombreAdjunto);
                mail.Attachments.Add(attachment);

                await client.SendMailAsync(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
