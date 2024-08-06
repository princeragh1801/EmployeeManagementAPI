using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Response;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IAttendanceService
    {
        /// <summary>
        /// This function returns the list of dates of logged in user when the user is present
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public Task<ApiResponse<List<AttendanceDto>>> GetByEmployeeId(int employeeId);

        /// <summary>
        /// This function adds the attendance of the employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public Task<ApiResponse<int>> Add(int employeeId);
    }
}
