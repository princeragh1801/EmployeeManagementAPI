namespace EmployeeSystem.Contract.Dtos.Info
{
    public class TaskLogInfo
    {
        public int Remaining { get; set; }
        public List<LogDto> Logs { get; set; } = new List<LogDto>();    
    }
}
