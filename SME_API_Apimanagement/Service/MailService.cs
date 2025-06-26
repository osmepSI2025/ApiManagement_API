using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace SME_API_Apimanagement.Services
{
    public class MailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendMailAsync(string subject, string body, string? recipientEmail = null)
        {
            try
            {
                // Get mail configuration from appsettings.json
                var mailSupport = _configuration["Information:Mail_Support"];
                var mailPassword = _configuration["Information:Mail_SupportPassWord"];
                var mailHost = _configuration["Information:Mail_Host"];
                var mailPort = int.Parse(_configuration["Information:Mail_Port"]);
                var mailSentTo = recipientEmail ?? _configuration["Information:Mail_SentTo"];

                // Create the SMTP client
                using var smtpClient = new SmtpClient(mailHost, mailPort)
                {
                    Credentials = new NetworkCredential(mailSupport, mailPassword),
                    EnableSsl = true
                };

                // Create the mail message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(mailSupport),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                // Add recipient
                mailMessage.To.Add(mailSentTo);

                // Send the email
                await smtpClient.SendMailAsync(mailMessage);

                return true; // Email sent successfully
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false; // Email sending failed
            }
        }
    }
}
