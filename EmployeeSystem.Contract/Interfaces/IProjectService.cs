using EmployeeSystem.Contract.Dtos;

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
