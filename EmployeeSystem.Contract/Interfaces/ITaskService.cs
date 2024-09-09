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
        /// <summary>
        /// Retrieves the count of tasks associated with a specific project, considering the provided user claims for access control.
        /// </summary>
        /// <param name="claims">A collection of <see cref="Claim"/> objects representing the user's claims used for access control and filtering.</param>
        /// <param name="projectId">The unique identifier of the project for which to retrieve the task count.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a <see cref="TaskCount"/> object with the following details:
        /// <list type="bullet">
        ///     <item><description><see cref="TaskCount.Total"/>: The total count of tasks.</description></item>
        ///     <item><description><see cref="TaskCount.TypeCount"/>: The count of tasks grouped by type.</description></item>
        ///     <item><description><see cref="TaskCount.StatusCount"/>: The count of tasks grouped by status.</description></item>
        ///     <item><description><see cref="TaskCount.AssignCount"/>: The count of tasks grouped by assignment.</description></item>
        /// </list>
        /// </returns>
        public Task<TaskCount> GetCount(IQueryable<Tasks> query);

        public Task<PaginatedItemsDto<TaskPaginationInfo>> Get(int userId, int projectId, ProjectTasksDto paginatedDto);
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

        /// <summary>
        /// Retrieves a list of tasks associated with a specific sprint based on the provided sprint ID.
        /// </summary>
        /// <param name="sprintId">The unique identifier of the sprint for which to retrieve the tasks.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a list of <see cref="TasksDto"/> objects representing the tasks associated with the given sprint.
        /// </returns>
        public Task<List<TasksDto>> GetSprintTask(int sprintId);

        /// <summary>
        /// Updates the status of a specific task based on the provided task ID and new status.
        /// </summary>
        /// <param name="id">The unique identifier of the task whose status is to be updated.</param>
        /// <param name="status">The new <see cref="TasksStatus"/> value to set for the task.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a boolean value indicating whether the status update was successful.
        /// </returns>
        public Task<bool> UpdateTaskStatus(int id, TasksStatus status);

        /// <summary>
        /// Updates the parent task of a specific task based on the provided task ID and new parent task ID.
        /// </summary>
        /// <param name="id">The unique identifier of the task whose parent is to be updated.</param>
        /// <param name="parentId">The unique identifier of the new parent task to be assigned to the specified task.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a boolean value indicating whether the parent update was successful.
        /// </returns>
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

        /// <summary>
        /// Retrieves a list of tasks associated with a specific project and filtered by the provided task type.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project for which to retrieve the tasks.</param>
        /// <param name="type">The <see cref="TaskType"/> that specifies the type of tasks to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a list of <see cref="TaskIdAndName"/> objects representing the tasks of the specified type within the given project.
        /// </returns>
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

        /// <summary>
        /// Adds multiple tasks based on the provided list of task details and user claims for access control.
        /// </summary>
        /// <param name="claims">A collection of <see cref="Claim"/> objects representing the user's claims used for access control and task creation permissions.</param>
        /// <param name="taskList">A list of <see cref="AddTaskDto"/> objects, each containing the details of a task to be added.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a boolean value indicating whether the addition of tasks was successful.
        /// </returns>
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

        /// <summary>
        /// Retrieves a list of child tasks associated with a specific parent task ID.
        /// </summary>
        /// <param name="id">The ID of the parent task for which to retrieve child tasks.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of <see cref="TasksDto"/> objects.</returns>
        public Task<List<TasksDto>> GetChildren(int id);

        /// <summary>
        /// Updates the sprint assignment of a specific task based on the provided task ID and new sprint ID.
        /// </summary>
        /// <param name="sprintId">The unique identifier of the sprint to which the task should be assigned.</param>
        /// <param name="taskId">The unique identifier of the task whose sprint assignment is to be updated.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a boolean value indicating whether the sprint update was successful.
        /// </returns>
        public Task<bool> UpdateTaskSprint(int sprintId, int taskId);

        /// <summary>
        /// Retrieves the details of a specific task based on the provided task ID.
        /// </summary>
        /// <param name="id">The unique identifier of the task to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a <see cref="Tasks"/> object with the details of the task, or <c>null</c> if no task is found with the given ID.
        /// </returns>
        public Task<Tasks?> GetById(int id);

        /// <summary>
        /// Updates the details of an existing task based on the provided <see cref="Tasks"/> object.
        /// </summary>
        /// <param name="task">A <see cref="Tasks"/> object containing the updated details of the task.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. This method does not return a value.
        /// </returns>
        public Task UpdateTask(Tasks task);

        /// <summary>
        /// Checks if a given parent task ID is valid for a specific type of child task.
        /// </summary>
        /// <param name="parentId">The unique identifier of the parent task to be validated.</param>
        /// <param name="child">The <see cref="TaskType"/> of the child task for which the parent task's validity is being checked.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a boolean value indicating whether the parent task is valid for the specified child task type.
        /// </returns>
        public Task<bool> CheckValidParent(int parentId, TaskType child);
    }
}
