using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface ITaskService
    {
        public Task<List<TasksDto>> GetAllTasks();
        public Task<TasksDto?> GetById(int id);
        public Task<TasksDto?> UpdateStatus(int userId, int id, TasksStatus taskStatus);
        public Task<int> Add(int userId, AddTaskDto task);
        public Task<bool> Delete(int id);
        public Task<bool> TaskExist(int id);
    }
}
