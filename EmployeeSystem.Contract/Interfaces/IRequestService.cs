namespace EmployeeSystem.Contract.Interfaces;
public interface IRequestService
{
    /// <summary>
    /// Allows the super-admin to respond to a specified request.
    /// </summary>
    /// <param name="userId">The unique identifier of the super-admin responding to the request.</param>
    /// <param name="id">The unique identifier of the request being responded to.</param>
    /// <param name="request">An object containing the response details for the request.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a boolean value indicating whether the response was successful.
    /// </returns>
    public Task<bool> RespondRequest(int userId, int id, Request request);


    //public Task<RequestDto> RequestById(int id);

    /// <summary>
    /// Retrieves information about all pending requests that require a response from the super-admin.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, containing a list of RequestDto for the pending requests.
    /// </returns>
    public Task<List<RequestDto>> GetAllPendingRequest();

}
