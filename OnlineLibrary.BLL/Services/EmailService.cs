using OnlineLibrary.BLL.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OnlineLibrary.BLL.Services
{
    public class EmailService : IEmailService
    {
        const string LOGIN = "onlinelibrary17@mail.ru";
        const string PASSWORD = "password123";
        const string SMTP_HOST = "smtp.mail.ru";
        const int SMTP_PORT = 587;
        public async Task<bool> Send(string email, string subject, string body)
        {
            try
            {
                var client = new SmtpClient
                {
                    Host = SMTP_HOST,
                    Port = SMTP_PORT,
                    Credentials = new NetworkCredential(LOGIN, PASSWORD),
                    EnableSsl = true
                };

                var message = new MailMessage
                {
                    From = new MailAddress(LOGIN),
                    To = { new MailAddress(email) },
                    Subject = subject,
                    Body = body
                };
                message.IsBodyHtml = true;
                client.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
