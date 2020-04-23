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
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("diploma.project.polytech@gmail.com", "polytechDP")
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("diploma.project.polytech@gmail.com", "ISSD Support Team"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);
            client.Send(mailMessage);
            return Task.CompletedTask;          
        }
    }
}
