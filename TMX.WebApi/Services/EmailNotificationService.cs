using System.Net;
using System.Net.Mail;

namespace TMX.TaskService.WebApi.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly SmtpClient _smtpClient;
        private readonly IConfiguration _config;

        public EmailNotificationService(IConfiguration configuration)
        {
            _smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(configuration["SmtpSettings:Username"],
                configuration["SmtpSettings:Password"]),//Exmaple
            };
            _config = configuration;
        }
        public async Task SentEmailNotificationAsync(string email, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["SmtpSettings:Username"]),  
                Subject = subject,
                Body = body
            };
            mailMessage.To.Add(email);

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
