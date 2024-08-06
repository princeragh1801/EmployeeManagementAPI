using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Models;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IRequestService
    {
        public Task<bool> RespondRequest(int userId, int id, Request request);

        //public Task<RequestDto> RequestById(int id);

        public Task<List<RequestDto>> GetAllPendingRequest();
    }
}
