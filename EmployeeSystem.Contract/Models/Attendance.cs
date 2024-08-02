using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeSystem.Contract.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        public DateOnly DateOnly { get; set; } = new DateOnly();

        
        public int EmployeeId { get; set; }
        //public AttendanceStatus Status { get; set; } = AttendanceStatus.Absent;

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }
    }
}
