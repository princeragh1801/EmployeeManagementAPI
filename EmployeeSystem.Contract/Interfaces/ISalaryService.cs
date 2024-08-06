using EmployeeSystem.Contract.Dtos;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface ISalaryService
    {
        /// <summary>
        /// This function is the get request it basically retuns the salary details of the employee with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List</returns>
        public Task<List<SalaryDto>> GetEmployeeSalaryDetails(int id);

        /// <summary>
        /// This is the post method it is used to pay the salary of the employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>Boolean</returns>
        public Task<bool> Pay(int employeeId);

        /// <summary>
        /// This is the utility method through which super-admin can pay the salary of all the employees
        /// </summary>
        /// <returns>Boolean</returns>
        public Task<bool> PayAll();
    }
}
