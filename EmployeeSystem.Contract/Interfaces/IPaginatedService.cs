using EmployeeSystem.Contract.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IPaginatedService
    {
        public Task<List<EmployeeDto>> GetEmployees(PaginatedDto paginatedDto);
        public Task<List<DepartmentDto>> GetDepartments(PaginatedDto paginatedDto);
        public Task<List<ProjectDto>> GetProjects(PaginatedDto paginatedDto);
        public Task<List<TasksDto>> GetTasks(PaginatedDto paginatedDto);
    }
}
