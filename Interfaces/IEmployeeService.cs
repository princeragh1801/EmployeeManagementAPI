using EmployeeSystemWebApi.Contract.Models;

namespace EmployeeSystemWebApi.Provider.Interfaces
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<Employee>> GetAll();
        public Task<Employee?> GetById(int id);
        public Task AddNew(Employee employee);
        public Task DeleteById(int id);
        public Task Update(Employee employee);
    }
}
