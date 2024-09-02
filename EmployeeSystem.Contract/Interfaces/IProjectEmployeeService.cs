using EmployeeSystem.Contract.Dtos.IdAndName;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IProjectEmployeeService
    {
        /// <summary>
        /// Retrieves the details of all employees associated with a specific project.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project for which to retrieve employee details.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a list of <see cref="EmployeeIdAndName"/> objects representing the members of the project.
        /// </returns>
        public Task<List<EmployeeIdAndName>> GetAll(int projectId);

        /// <summary>
        /// Adds a list of employees to a specific project based on the provided project ID.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project to which employees will be added.</param>
        /// <param name="employeesToAdd">A list of employee IDs representing the employees to be added to the project.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a boolean value indicating whether the addition of employees was successful.
        /// </returns>
        public Task<bool> AddMembers(int projectId, List<int> employeesToAdd);

        /// <summary>
        /// Removes a list of employees from a specific project based on the provided project ID.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project from which employees will be removed.</param>
        /// <param name="employeesToAdd">A list of employee IDs representing the employees to be removed from the project.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a boolean value indicating whether the removal of employees was successful.
        /// </returns>
        public Task<bool> DeleteMembers(int projectId, List<int> employeesToAdd);
    }
}
