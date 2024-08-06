using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Response;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IDepartmentService
    {
        /// <summary>
        /// This function returns the list of departments exist in the system
        /// </summary>
        /// <returns></returns>
        public Task<ApiResponse<List<DepartmentDto>>> GetAll();

        /// <summary>
        /// This function returns the info the department with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ApiResponse<DepartmentDto>> GetById(int id);

        /// <summary>
        /// This function deletes the department from the system
        /// It returns the boolean value
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean</returns>
        public Task<ApiResponse<bool>> DeleteById(int id);

        /// <summary>
        /// This function adds the new department in the db
        /// It returns id of the added department
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="department"></param>
        /// <returns>Integer</returns>
        public Task<ApiResponse<int>> Add(int userId, AddDepartmentDto department);
    }
}
