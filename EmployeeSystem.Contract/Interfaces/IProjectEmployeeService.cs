using EmployeeSystem.Contract.Dtos.IdAndName;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IProjectEmployeeService
    {
        public Task<List<EmployeeIdAndName>> GetAll(int projectId);
        public Task<bool> AddMembers(int projectId, List<int> employeesToAdd);
        public Task<bool> DeleteMembers(int projectId, List<int> employeesToAdd);
    }
}
