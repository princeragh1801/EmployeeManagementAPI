using EmployeeSystem.Contract.Dtos;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface ISalaryService
    {
        public Task<List<SalaryDto>> GetEmployeeSalaryDetails(int id);

        public Task<bool> Pay(int employeeId);
    }
}
