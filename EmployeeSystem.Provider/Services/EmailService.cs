
using System.Net;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace EmployeeSystem.Provider.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _accountSid = string.Empty;
        private readonly string _smtpServer = string.Empty;
        private readonly string _from = string.Empty;
        private readonly string _port = string.Empty;
        private readonly string _username = string.Empty;
        private readonly string _password = string.Empty;
        private readonly string _authToken = string.Empty;
        private readonly string _whatsAppFrom = string.Empty;
        public EmailService()
        {
            _smtpServer = Environment.GetEnvironmentVariable("EMAIL_SMTP_SERVER");
            _from = Environment.GetEnvironmentVariable("EMAIL_FROM");
            _port = Environment.GetEnvironmentVariable("EMAIL_PORT");
            _username = Environment.GetEnvironmentVariable("EMAIL_USERNAME");
            _password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
        }

        public async Task SendEmail(string to, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_from),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            using var client = new SmtpClient(_smtpServer, Convert.ToInt32(_port))
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = true
            };

            await client.SendMailAsync(mailMessage);
        }

        public void SendWhatsAppMessage(string to, string message)
        {
            TwilioClient.Init(_accountSid, _authToken);

            var messageOptions = new CreateMessageOptions(
              new PhoneNumber($"whatsapp:{to}"));
            messageOptions.From = new PhoneNumber(_whatsAppFrom);
            messageOptions.Body = message;
            

            var msg = MessageResource.Create(messageOptions);

        }

        // TODO::   
        public async void Send()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://graph.facebook.com/v20.0/332056280002043/messages");
                //client.DefaultRequestHeaders.Add("");
            }catch (Exception ex)
            {

            }
        }
    }
}
