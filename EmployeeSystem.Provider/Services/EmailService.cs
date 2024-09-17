using EmployeeSystem.Contract.Utils;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace EmployeeSystem.Provider.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly string _accountSid = string.Empty;
        private readonly string _authToken = string.Empty;
        private readonly string _whatsAppFrom = string.Empty;
        public EmailService(EmailConfiguration emailConfig, IConfiguration configuration)
        {
            _emailConfig = emailConfig;
            /*_accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _whatsAppFrom = configuration["Twilio:WhatsAppFrom"];*/
        }

        public async Task SendEmail(string to, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailConfig.From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            using var client = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port)
            {
                Credentials = new NetworkCredential(_emailConfig.Username, _emailConfig.Password),
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
