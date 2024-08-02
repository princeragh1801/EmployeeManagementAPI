using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Response;

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
