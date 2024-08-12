using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IProjectService
    {
        /// <summary>
        /// Retrieves a list of projects present in the system based on accessibility.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of ProjectDto representing the projects.
        /// </returns>
        public Task<List<ProjectDto>> GetAll(int id);

        /// <summary>
        /// Retrieves a list of projects assigned to the specified user.
        /// </summary>
        /// <param name="employeeId">The unique identifier of the project.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of ProjectDto representing the projects.
        /// </returns>
        public Task<List<ProjectDto>> GetProjectsByEmployee(int employeeId);

        /// <summary>
        /// Retrieves a list of projects according to the status.
        /// </summary>
        /// <param name="status">The unique identifier of the project.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of ProjectDto representing the projects.
        /// </returns>
        public Task<List<ProjectDto>> GetProjectsStatusWise(ProjectStatus status);

        /// <summary>
        /// Retrieves the project with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the project.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a ProjectDetailsDto with the project information, or null if the project is not found.
        /// </returns>
        public Task<ProjectDetailsDto?> GetById(int id);


        /// <summary>
        /// Adds a new project to the system and returns the ID of the created project.
        /// This operation can only be performed by users with the super-admin role.
        /// </summary>
        /// <param name="adminId">The unique identifier of the super-admin authorizing the project addition.</param>
        /// <param name="project">An object containing the details of the project to be added.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the ID of the newly created project.
        /// </returns>
        public Task<int> Add(int adminId, AddProjectDto project);

        /// <summary>
        /// Update project to the system and returns the ID of the updated project.
        /// This operation can only be performed by users with the super-admin role.
        /// </summary>
        /// <param name="adminId">The unique identifier of the super-admin authorizing the project addition.</param>
        /// <param name="project">An object containing the details of the project to be added.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the ID of the newly created project.
        /// </returns>
        public Task<int> Update(int id, int adminId, AddProjectDto project);

        /// <summary>
        /// Deletes the project with the specified ID from the system and returns a boolean indicating the success of the operation.
        /// This operation can only be performed by users with the super-admin role.
        /// </summary>
        /// <param name="id">The unique identifier of the project to be deleted.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a boolean value indicating whether the deletion was successful.
        /// </returns>
        public Task<bool> DeleteById(int id);

    }
}
