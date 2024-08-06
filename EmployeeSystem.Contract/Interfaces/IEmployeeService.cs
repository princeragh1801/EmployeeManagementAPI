using EmployeeSystem.Contract.Dtos;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IEmployeeService
    {
        /// <summary>
        /// This is the get request it shows the list of employees present in the system according to the accessability
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<EmployeeDto>?> GetAll(int userId);

        /// <summary>
        /// This function retreives the employee info on the basis of the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<EmployeeDto?> GetById(int id);

        /// <summary>
        /// This function add the employee in the db.
        /// This route can only be accessible by the the Employees whose role is super-admin
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        public Task<int> Add(int userId, AddEmployeeDto employee);

        /// <summary>
        /// This function add the list of employees in the db.
        /// This route can only be accessible by the the Employees whose role is super-admin
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="employees"></param>
        /// <returns></returns>
        public Task<int> AddList(int userId, List<AddEmployeeDto> employees);

        /// <summary>
        /// This function delete the employee with the given id in the db.
        /// This route can only be accessible by the the Employees whose role is super-admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean value</returns>
        public Task<bool> DeleteById(int id);

        /// <summary>
        /// This function updates the employee info in the db.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns>Employee Information</returns>
        public Task<EmployeeDto?> Update(int userId, int id, UpdateEmployeeDto employee);

        /// <summary>
        /// This is the get request it shows the list of employees who is the manager present in the system according to the accessability
        /// </summary>
        /// <returns></returns>
        public Task<List<EmployeeDto>> GetManagers();

        /// <summary>
        /// This function checks whethe the employee with the given id exist or not
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> EmployeeExist(int id);

        /// <summary>
        /// This function returns the list of employees of a particular department
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<List<EmployeeDto>?> GetEmloyeesWithDepartmentName(int id);
    }
}
