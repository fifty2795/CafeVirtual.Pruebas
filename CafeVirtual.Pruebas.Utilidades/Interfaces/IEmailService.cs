using CafeVirtual.Pruebas.Utilidades.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Utilidades.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string destinatario, string asunto, string htmlBody);

        Task<bool> SendEmailAdjuntosAsync(string destinatario, string asunto, AlternateView htmlView, byte[] adjuntoContent, string nombreAdjunto);
    }
}
