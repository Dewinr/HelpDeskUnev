using System.Net;
using System.Net.Mail;

namespace HelpDeskUnev.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarCorreoEstadoTicketAsync(string destinatario, string nombreUsuario, int ticketId, string nuevoEstado)
        {
            if (string.IsNullOrEmpty(destinatario)) return;

            var emailServer = _configuration["EmailSettings:Server"];
            var port = int.Parse(_configuration["EmailSettings:Port"] ?? "587");
            var senderName = _configuration["EmailSettings:SenderName"];
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var password = _configuration["EmailSettings:Password"];

            var asunto = $"Actualización en tu Ticket #{ticketId} - Help Desk UNEV";
            var mensajeHtml = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; color: #333;'>
                    <h2 style='color: #54071E;'>Help Desk UNEV</h2>
                    <p>Hola <strong>{nombreUsuario}</strong>,</p>
                    <p>Te informamos que tu solicitud con el <strong>Ticket #{ticketId}</strong> ha cambiado de estado.</p>
                    <p>Nuevo estado: <span style='background-color: #ffc107; padding: 5px 10px; font-weight: bold; border-radius: 4px;'>{nuevoEstado}</span></p>
                    <hr style='border: none; border-top: 1px solid #eee;' />
                    <p style='font-size: 12px; color: #777;'>Este es un mensaje automático del sistema Help Desk UNEV. Por favor no respondas a este correo.</p>
                </div>";

            using var client = new SmtpClient(emailServer, port)
            {
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail!, senderName),
                Subject = asunto,
                Body = mensajeHtml,
                IsBodyHtml = true
            };

            mailMessage.To.Add(destinatario);

            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Manejo de errores de envío (puedes registrar el log si lo deseas)
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
            }
        }
    }
}