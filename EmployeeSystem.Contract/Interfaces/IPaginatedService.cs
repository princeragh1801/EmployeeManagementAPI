using EmployeeSystem.Contract.Dtos;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IPaginatedService
    {
        public Task<PaginatedItemsDto<List<EmployeeDto>>> GetEmployees(PaginatedDto paginatedDto);
        public Task<PaginatedItemsDto<List<DepartmentDto>>> GetDepartments(PaginatedDto paginatedDto);
        public Task<PaginatedItemsDto<List<ProjectDto>>> GetProjects(PaginatedDto paginatedDto);
        public Task<PaginatedItemsDto<List<TasksDto>>> GetTasks(PaginatedDto paginatedDto);
    }
}
