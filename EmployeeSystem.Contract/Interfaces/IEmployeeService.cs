using EmployeeSystem.Contract.Dtos;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IEmployeeService
    {
        
        public Task<List<EmployeeDto>?> GetAll();
        public Task<EmployeeDto?> GetById(int id);
        public Task<int> Add(int userId, AddEmployeeDto employee);
        public Task<int> AddList(int userId, List<AddEmployeeDto> employees);
        public Task<bool> DeleteById(int id);
        public Task<EmployeeDto?> Update(int userId, int id, AddEmployeeDto employee);
        public Task<List<EmployeeDto>> GetManagers();
        public Task<bool> EmployeeExist(int id);
        public Task<List<EmployeeDto>?> GetEmloyeesWithDepartmentName(int id);
    }
}
