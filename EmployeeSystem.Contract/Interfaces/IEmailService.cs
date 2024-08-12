namespace EmployeeSystem.Contract.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmail(string to, string subject, string body);
        public void SendWhatsAppMessage(string to, string message);
    }
}
