using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Models
{
    public class Request : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public RequestType RequestType { get; set; }
        public RequestStatus RequestStatus { get; set; } = RequestStatus.Requested;
    }
}
