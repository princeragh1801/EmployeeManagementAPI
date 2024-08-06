using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Models;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IRequestService
    {
        /// <summary>
        /// It is the function that is used by the super-admin to respond the given requests
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns>Boolean</returns>
        public Task<bool> RespondRequest(int userId, int id, Request request);

        //public Task<RequestDto> RequestById(int id);

        /// <summary>
        /// This is the utility function which is used to get info of all the pending request by the super-admin
        /// </summary>
        /// <returns></returns>
        public Task<List<RequestDto>> GetAllPendingRequest();
    }
}
