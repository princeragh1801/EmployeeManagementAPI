namespace EmployeeSystem.Contract.Interfaces;
public interface ITaskLogService
{
    /// <summary>
    /// Retrieves a list of logs associated with a specific task based on the provided task ID.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task for which to retrieve the logs.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a list of <see cref="LogDto"/> objects representing the logs associated with the given task.
    /// </returns>
    public Task<TaskLogInfo> GetLogs(int taskId, int skip);

    /// <summary>
    /// Adds a new task log entry based on the provided log details.
    /// </summary>
    /// <param name="log">An <see cref="AddTaskLogDto"/> object containing the details of the task log to be added.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. This method does not return a value.
    /// </returns>
    public Task Add(AddTaskLogDto log);

    /// <summary>
    /// Adds multiple task log entries based on the provided list of log details.
    /// </summary>
    /// <param name="logs">A list of <see cref="AddTaskLogDto"/> objects, each containing the details of a task log to be added.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. This method does not return a value.
    /// </returns>
    public Task AddMany(List<AddTaskLogDto> logs);
}
