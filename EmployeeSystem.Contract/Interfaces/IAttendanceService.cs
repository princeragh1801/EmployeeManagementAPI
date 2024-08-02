using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IAttendanceService
    {
        public Task<ApiResponse<List<AttendanceDto>>> GetByEmployeeId(int employeeId);
        public Task<ApiResponse<int>> Add(int employeeId);
    }
}
