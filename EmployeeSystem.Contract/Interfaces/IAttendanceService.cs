using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Response;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IAttendanceService
    {
        public Task<ApiResponse<List<AttendanceDto>>> GetByEmployeeId(int employeeId);
        public Task<ApiResponse<int>> Add(int employeeId);
    }
}
