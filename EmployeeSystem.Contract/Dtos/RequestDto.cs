using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos
{
    public class RequestDto : BaseEntityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RequestedBy { get; set; }
        public RequestStatus Status { get; set; }
        public int TotalPendingRequests { get; set; }
    }
}
