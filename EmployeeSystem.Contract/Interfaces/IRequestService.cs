using EmployeeSystem.Contract.Models;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IRequestService
    {
        public Task<bool> RespondRequest(int userId, int id, Request request);
    }
}
