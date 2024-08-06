using EmployeeSystem.Contract.Dtos;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IPaginatedService
    {
        /// <summary>
        /// This function retuns list of employee on a particular page with the querable fields
        /// </summary>
        /// <param name="paginatedDto"></param>
        /// <returns>List</returns>
        public Task<PaginatedItemsDto<List<EmployeeDto>>> GetEmployees(PaginatedDto paginatedDto);

        /// <summary>
        /// This function retuns list of departments on a particular page with the querable fields
        /// </summary>
        /// <param name="paginatedDto"></param>
        /// <returns>List</returns>
        public Task<PaginatedItemsDto<List<DepartmentDto>>> GetDepartments(PaginatedDto paginatedDto);

        /// <summary>
        /// This function retuns list of projects on a particular page with the querable fields
        /// </summary>
        /// <param name="paginatedDto"></param>
        /// <returns>List</returns>
        public Task<PaginatedItemsDto<List<ProjectDto>>> GetProjects(PaginatedDto paginatedDto);

        /// <summary>
        /// This function retuns list of tasks on a particular page with the querable fields
        /// </summary>
        /// <param name="paginatedDto"></param>
        /// <returns>List</returns>
        public Task<PaginatedItemsDto<List<TasksDto>>> GetTasks(PaginatedDto paginatedDto);
    }
}
