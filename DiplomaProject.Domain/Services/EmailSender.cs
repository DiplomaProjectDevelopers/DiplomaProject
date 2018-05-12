using DiplomaProject.Domain.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = true,
                EnableSsl = true,
                Credentials = new NetworkCredential("issd.armenianteam@gmail.com", "13&3d3upp0rt!")
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("issd.armenianteam@gmail.com", "ISSD Support Team"),
                Subject = subject,
                Body = message
            };
            mailMessage.To.Add(email);
            client.Send(mailMessage);
            return Task.CompletedTask;          
        }
    }
}
