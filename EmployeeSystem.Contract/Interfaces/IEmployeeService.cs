using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.Count;
using EmployeeSystem.Contract.Dtos.IdAndName;
using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Dtos.Info.PaginationInfo;
using EmployeeSystem.Contract.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Retrieves the total count of employees, including counts categorized by roles such as Employee, Admin, and SuperAdmin.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation, containing an <see cref="EmployeeCount"/> object with details of 
        /// the total employee count and counts grouped by each role.
        /// </returns>
        public Task<EmployeeCount> GetCounts(int userId);

        public Task<PaginatedItemsDto<List<EmployeePaginationInfo>>> Get(IEnumerable<Claim> claims, PaginatedDto<Role?> paginatedDto);
        /// <summary>
        /// Retrieves a list of employees present in the system based on the user's accessibility.
        /// </summary>
        /// <param name="userId">The unique identifier of the user requesting the employee list.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of EmployeeDto or null if no employees are accessible.
        /// </returns>
        public Task<List<EmployeeDto>?> GetAll(IEnumerable<Claim> claims);


        /// <summary>
        /// Retrieves the information of the employee with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the employee.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing an EmployeeDto if the employee is found, or null if not.
        /// </returns>
        public Task<EmployeeInfo?> GetById(int id);


        /// <summary>
        /// Adds a new employee to the database. 
        /// This route is accessible only by users with the super-admin role.
        /// </summary>
        /// <param name="userId">The unique identifier of the user adding the employee.</param>
        /// <param name="employee">An object containing the details of the employee to be added.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the ID of the newly added employee.
        /// </returns>
        public Task<int> Add(IEnumerable<Claim> claims, AddEmployeeDto employee);


        /// <summary>
        /// Adds a list of employees to the database.
        /// This route is accessible only by users with the super-admin role.
        /// </summary>
        /// <param name="userId">The unique identifier of the user adding the employees.</param>
        /// <param name="employees">A list of objects containing the details of the employees to be added.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the count of successfully added employees.
        /// </returns>
        public Task<int> AddList(IEnumerable<Claim> claims, List<AddEmployeeDto> employees);


        /// <summary>
        /// Deletes the employee with the specified ID from the database.
        /// This route is accessible only by users with the super-admin role.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to be deleted.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a boolean value indicating whether the deletion was successful.
        /// </returns>
        public Task<bool> DeleteById(int id);


        /// <summary>
        /// Updates the information of the employee with the specified ID in the database.
        /// </summary>
        /// <param name="userId">The unique identifier of the user performing the update.</param>
        /// <param name="id">The unique identifier of the employee to be updated.</param>
        /// <param name="employee">An object containing the updated details of the employee.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing an EmployeeDto with the updated information, or null if the employee is not found.
        /// </returns>
        public Task<bool?> Update(IEnumerable<Claim> claims, int id, UpdateEmployeeDto employee);


        /// <summary>
        /// Retrieves a list of employees who are managers present in the system, based on accessibility.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of EmployeeDto representing the managers.
        /// </returns>
        public Task<List<EmployeeDto>> GetManagers();


        /// <summary>
        /// Checks whether an employee with the specified ID exists in the system.
        /// </summary>
        /// <param name="id">The unique identifier of the employee.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a boolean value indicating whether the employee exists.
        /// </returns>
        public Task<bool> EmployeeExist(int id);


        /// <summary>
        /// Retrieves the list of employees belonging to the specified department.
        /// </summary>
        /// <param name="id">The unique identifier of the department.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of EmployeeDto for the employees in the specified department, or null if no employees are found.
        /// </returns>
        public Task<List<EmployeeIdAndName>?> GetEmployeesWithDepartmentName(int id);

        /// <summary>
        /// Retrieves the list of employees with the name and id.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of EmployeeIdAndName.
        /// </returns>
        public Task<List<EmployeeIdAndName>> GetEmployeeIdAndName();

        /// <summary>
        /// Retrieves the details of an employee for the purpose of updating, based on the specified employee ID.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to retrieve for update.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing an <see cref="UpdateEmployeeDto"/> object with the employee's details.
        /// </returns>
        public Task<UpdateEmployeeDto> GetEmployeeToUpdate(int id);

        public Task<bool> UpdateAvatar(int id, IFormFile file);
    }
}
