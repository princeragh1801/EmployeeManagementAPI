namespace EmployeeSystem.Contract.Utils
{
    public class EmailConfiguration
    { 
        public required string SmtpServer { get; set; }
        public int Port { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string From { get; set; }
    }
}
