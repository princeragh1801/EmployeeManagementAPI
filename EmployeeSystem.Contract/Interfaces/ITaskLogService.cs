using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Models;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface ITaskLogService
    {
        public Task<List<LogDto>> GetLogs(int taskId);
        public Task Add(TaskLog log);
    }
}
