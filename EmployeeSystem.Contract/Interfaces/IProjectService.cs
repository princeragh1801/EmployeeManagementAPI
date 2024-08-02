using EmployeeSystem.Contract.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IProjectService
    {
        public Task<List<ProjectDto>> GetAll();
        public Task<ProjectDetailsDto?> GetById(int id);
        public Task<int> Add(int userId, AddProjectDto project);
        public Task<bool> DeleteById(int id);
    }
}
