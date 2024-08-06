using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Response;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IAttendanceService
    {
        /// <summary>
        /// Retrieves the list of attendance dates for the specified employee when they were present.
        /// </summary>
        /// <param name="employeeId">The unique identifier of the employee.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing an ApiResponse with a list of AttendanceDto.
        /// </returns>
        public Task<ApiResponse<List<AttendanceDto>>> GetByEmployeeId(int employeeId);


        /// <summary>
        /// Adds the attendance record for the specified employee.
        /// </summary>
        /// <param name="employeeId">The unique identifier of the employee.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing an ApiResponse with the ID of the newly created attendance record.
        /// </returns>
        public Task<ApiResponse<int>> Add(int employeeId);

    }
}
