using EmployeeSystemWebApi.Contract.Models;

namespace EmployeeSystemWebApi.Provider.Interfaces
{
    public interface IDepartmentService
    {
        public Task<IEnumerable<Department>> GetAll();
        public Task<Department?> GetById(int id);
        public Task DeleteById(int id);
        public Task AddNew(Department department);
    }
}
