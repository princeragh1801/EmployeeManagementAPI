using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.Info;
using System.Security.Claims;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface ISprintService
    {
        /// <summary>
        /// Inserts or updates a sprint based on the provided sprint ID and details.
        /// </summary>
        /// <param name="id">The unique identifier of the sprint to be inserted or updated. If the ID is zero or not provided, a new sprint will be created.</param>
        /// <param name="addSprintDto">The details of the sprint to be inserted or updated, encapsulated in an <see cref="AddSprintDto"/> object.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing the unique identifier of the inserted or updated sprint.
        /// </returns>
        public Task<int> Upsert(int id, IEnumerable<Claim> claims, AddSprintDto addSprintDto);

        /// <summary>
        /// Retrieves a list of all sprints.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a list of <see cref="SprintInfo"/> objects representing all sprints.
        /// </returns>
        public Task<List<SprintInfo>> GetAll();

        /// <summary>
        /// Retrieves the details of a specific sprint based on the provided sprint ID.
        /// </summary>
        /// <param name="id">The unique identifier of the sprint to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a <see cref="SprintInfo"/> object with the details of the sprint, or <c>null</c> if no sprint is found with the given ID.
        /// </returns>
        public Task<SprintInfo?> GetById(int id);

        /// <summary>
        /// Retrieves a list of sprints associated with a specific project based on the provided project ID.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project for which to retrieve the sprints.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a list of <see cref="SprintInfo"/> objects representing the sprints associated with the given project.
        /// </returns>
        public Task<List<SprintInfo>> GetByProjectId(int projectId);

        /// <summary>
        /// Deletes a sprint based on the provided sprint ID.
        /// </summary>
        /// <param name="id">The unique identifier of the sprint to be deleted.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a boolean value indicating whether the deletion was successful.
        /// </returns>
        public Task<bool> DeleteById(int id, IEnumerable<Claim> claims);
    }
}
