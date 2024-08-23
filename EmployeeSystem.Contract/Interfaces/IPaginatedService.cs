using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Info.PaginationInfo;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IPaginatedService
    {
        /// <summary>
        /// Retrieves a paginated list of employees based on the specified pagination parameters.
        /// </summary>
        /// <param name="paginatedDto">An object containing pagination and query parameters.</param>
        /// <param name="userId">User id.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a PaginatedItemsDto with a list of EmployeeDto.
        /// </returns>
        public Task<PaginatedItemsDto<List<EmployeePaginationInfo>>> GetEmployees(int userId, PaginatedDto paginatedDto);

        /// <summary>
        /// Retrieves a paginated list of departments based on the specified pagination parameters.
        /// </summary>
        /// <param name="paginatedDto">An object containing pagination and query parameters.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a PaginatedItemsDto with a list of DepartmentDto.
        /// </returns>
        public Task<PaginatedItemsDto<List<DepartmentPaginationInfo>>> GetDepartments(PaginatedDto paginatedDto);


        /// <summary>
        /// Retrieves a paginated list of projects based on the specified pagination parameters.
        /// </summary>
        /// <param name="paginatedDto">An object containing pagination and query parameters.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a PaginatedItemsDto with a list of ProjectDto.
        /// </returns>
        public Task<PaginatedItemsDto<List<ProjectDto>>> GetProjects( int userId, PaginatedDto paginatedDto);


        /// <summary>
        /// Retrieves a paginated list of tasks based on the specified pagination parameters.
        /// </summary>
        /// <param name="paginatedDto">An object containing pagination and query parameters.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a PaginatedItemsDto with a list of TasksDto.
        /// </returns>
        public Task<PaginatedItemsDto<List<TasksDto>>> GetTasks(int userId, PaginatedDto paginatedDto);

        public Task<PaginatedItemsDto<List<TasksDto>>> GetProjectTasks(int userId, int projectId, ProjectTasksDto paginatedDto);

    }
}
