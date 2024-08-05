using EmployeeSystem.Contract.Dtos;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface ITaskService
    {
        public Task<List<TasksDto>> GetAllTasks(int userId);
        public Task<TasksDto?> GetById(int userId, int id);
        public Task<TasksDto?> UpdateStatus(int userId, int id, TasksStatus taskStatus);
        public Task<int> Add(int userId, int adminId, AddTaskDto task);
        public Task<bool?> Delete(int userId, int id);
        public Task<bool> TaskExist(int id);
    }
}
