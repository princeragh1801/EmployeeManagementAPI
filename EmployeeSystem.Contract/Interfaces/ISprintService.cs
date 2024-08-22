using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.Info;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface ISprintService
    {
        public Task<int> Add(AddSprintDto addSprintDto);
        public Task<List<SprintInfo>> GetAll();
        public Task<SprintInfo?> GetById(int id);
        public Task<List<SprintInfo>> GetByProjectId(int projectId);
        public Task<bool> DeleteById(int id);
    }
}
