using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.Info.PaginationInfo;
using EmployeeSystem.Contract.Response;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IDepartmentService
    {
        public Task<PaginatedItemsDto<List<DepartmentPaginationInfo>>> Get(PaginatedDto<Role?> paginatedDto);
        /// <summary>
        /// Retrieves the list of all departments in the system.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing an ApiResponse with a list of DepartmentDto.
        /// </returns>
        public Task<ApiResponse<List<DepartmentDto>>> GetAll();


        /// <summary>
        /// Retrieves the information of the department with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the department.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing an ApiResponse with the DepartmentDto for the specified ID.
        /// </returns>
        public Task<ApiResponse<DepartmentDto>> GetById(int id);


        /// <summary>
        /// Deletes the department with the specified ID from the system.
        /// </summary>
        /// <param name="id">The unique identifier of the department to be deleted.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing an ApiResponse with a boolean indicating whether the deletion was successful.
        /// </returns>
        public Task<ApiResponse<bool>> DeleteById(int id);


        /// <summary>
        /// Adds a new department to the database and returns the ID of the added department.
        /// </summary>
        /// <param name="userId">The unique identifier of the user adding the department.</param>
        /// <param name="department">An object containing the details of the department to be added.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing an ApiResponse with the ID of the newly added department.
        /// </returns>
        public Task<ApiResponse<int>> Add(int userId, AddDepartmentDto department);

    }
}
