using FinalProject.Areas.Admin.Models;
using System.Net.Mail;
using System.Net;

namespace FinalProject.Areas.Admin.GmailService
{
    public class EmailService
    {
        private readonly string smtpServer;
        private readonly int port;
        private readonly string username;
        private readonly string password;

        public EmailService()
        {
            // SMTP server configuration
            smtpServer = "smtp-mail.outlook.com";
            port = 587;  // For SSL use 465, for TLS use 587
            username = "ibadovmuslum402@gmail.com";
            password = "muslum2005";
        }

        public void SendEmail(EmailModel emailModel)
        {
            try
            {
                // Create a new MailMessage object
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(emailModel.SenderEmail),
                    Subject = emailModel.Subject,
                    Body = emailModel.Body,
                    IsBodyHtml = true // Set to true if your email body is HTML
                };
                mail.To.Add(emailModel.ReceiverEmail);

                // Set up the SMTP client
                SmtpClient smtpClient = new SmtpClient(smtpServer, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true
                };

                // Send the email
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error sending email: {ex.Message}", ex);
            }
        }
    }
}

