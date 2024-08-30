using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.Count;
using EmployeeSystem.Contract.Dtos.IdAndName;
using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Dtos.Info.PaginationInfo;
using EmployeeSystem.Contract.Models;
using System.Security.Claims;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface ITaskService
    {
        public Task<TaskCount> GetCount(IEnumerable<Claim> claims);

        public Task<PaginatedItemsDto<List<TasksDto>>> Get(int userId, int projectId, ProjectTasksDto paginatedDto);
        /// <summary>
        /// Retrieves a list of tasks based on the accessibility role of the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose role determines accessibility.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of TasksDto representing the accessible tasks.
        /// </returns>
        public Task<List<TasksDto>> GetAllTasks(int userId);

        /// <summary>
        /// Retrieves a list of tasks assigned to the specified user.
        /// </summary>
        /// <param name="employeeId">The unique identifier of the user whose role determines accessibility.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of TasksDto representing the accessible tasks.
        /// </returns>
        public Task<List<TasksDto>> GetEmployeeTask(int employeeId);

        public Task<List<TasksDto>> GetSprintTask(int sprintId);

        public Task<bool> UpdateTaskStatus(int id, TasksStatus status);

        public Task<bool> UpdateTaskParent(int id, int parentId);

        /// <summary>
        /// Retrieves the task object with the specified ID for the given user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user requesting the task.</param>
        /// <param name="id">The unique identifier of the task to be retrieved.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a TasksDto with the task information, or null if the task is not found.
        /// </returns>
        public Task<TaskInfo?> GetById(int userId, int id);

        public Task<List<TaskIdAndName>> GetByType(int projectId, TaskType type);

        /// <summary>
        /// Updates the status of the task with the specified ID and returns the updated task object.
        /// </summary>
        /// <param name="userId">The unique identifier of the user performing the update.</param>
        /// <param name="id">The unique identifier of the task whose status is being updated.</param>
        /// <param name="taskStatus">The new status to be assigned to the task.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a TasksDto with the updated task information, or null if the task is not found.
        /// </returns>
        public Task<bool?> Update(int userId, int id, UpdateTaskDto taskDto);


        /// <summary>
        /// Adds a new task to the system and returns the ID of the created task.
        /// </summary>
        /// <param name="userId">The unique identifier of the user adding the task.</param>
        /// <param name="adminId">The unique identifier of the admin authorizing the task addition.</param>
        /// <param name="task">An object containing the details of the task to be added.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the ID of the newly created task.
        /// </returns>
        public Task<int> Add(IEnumerable<Claim> claims, AddTaskDto task);

        public Task<bool> AddMany(IEnumerable<Claim> claims, List<AddTaskDto> taskList);
        /// <summary>
        /// Deletes the task with the specified ID and returns a boolean indicating the success of the operation.
        /// </summary>
        /// <param name="userId">The unique identifier of the user requesting the deletion.</param>
        /// <param name="id">The unique identifier of the task to be deleted.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a nullable boolean value indicating whether the deletion was successful.
        /// </returns>
        public Task<bool?> Delete(int userId, int id);


        /// <summary>
        /// Checks whether a task with the specified ID exists in the system.
        /// </summary>
        /// <param name="id">The unique identifier of the task to be checked.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a boolean value indicating whether the task exists.
        /// </returns>
        public Task<bool> TaskExist(int id);

        public Task<List<TasksDto>> GetChildren(int id);

        public Task<bool> UpdateTaskSprint(int sprintId, int taskId);

        public Task<Tasks?> GetById(int id);

        public Task UpdateTask(Tasks task);

        public Task<bool> CheckValidParent(int parentId, TaskType child);
    }
}
