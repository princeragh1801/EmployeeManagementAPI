using EmployeeSystem.Contract.Dtos;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface ITaskService
    {
        /// <summary>
        /// This is the get request it returns the list of tasks on the basis of accessibility role
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List</returns>
        public Task<List<TasksDto>> GetAllTasks(int userId);

        /// <summary>
        /// This is the get request it returns the task object with the given id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns>Task object</returns>
        public Task<TasksDto?> GetById(int userId, int id);

        /// <summary>
        /// This is the put request, it updates the task status and returns the task object
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <param name="taskStatus"></param>
        /// <returns>Task object</returns>
        public Task<TasksDto?> UpdateStatus(int userId, int id, TasksStatus taskStatus);

        /// <summary>
        /// It is the post request, it adds the the new task and returns the task id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="adminId"></param>
        /// <param name="task"></param>
        /// <returns>Integer</returns>
        public Task<int> Add(int userId, int adminId, AddTaskDto task);

        /// <summary>
        /// It is the delete request it deletes the the task with the given id and returns the boolean value
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns>Boolean</returns>
        public Task<bool?> Delete(int userId, int id);

        /// <summary>
        /// It is the utility method which checks whether the task exist or not
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean value</returns>
        public Task<bool> TaskExist(int id);
    }
}
