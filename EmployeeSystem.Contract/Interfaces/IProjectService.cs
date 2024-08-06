using EmployeeSystem.Contract.Dtos;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IProjectService
    {
        /// <summary>
        /// This is the get request it shows the list of projects present in the system according to the accessability
        /// </summary>
        /// <returns>List</returns>
        public Task<List<ProjectDto>> GetAll();

        /// <summary>
        /// This is the get request it returns the project with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Project object</returns>
        public Task<ProjectDetailsDto?> GetById(int id);

        /// <summary>
        /// This is the post request it adds the project in the system and returns the id of the created project.
        /// This can only be done by the super-admin
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="adminId"></param>
        /// <param name="project"></param>
        /// <returns>Integer</returns>
        public Task<int> Add(int userId, int adminId, AddProjectDto project);

        /// <summary>
        /// This is the delete request it deletes the project in the system and returns the boolean value
        /// This can only be done by the super-admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean</returns>
        public Task<bool> DeleteById(int id);
    }
}
