using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Models;
using EmployeeSystem.Contract.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IDepartmentService
    {
        public Task<ApiResponse<List<DepartmentDto>>> GetAll();
        public Task<ApiResponse<DepartmentDto>> GetById(int id);
        public Task<ApiResponse<bool>> DeleteById(int id);
        public Task<ApiResponse<int>> Add(int userId, AddDepartmentDto department);
    }
}
