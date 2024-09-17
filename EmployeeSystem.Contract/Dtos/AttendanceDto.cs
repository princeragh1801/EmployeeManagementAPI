namespace EmployeeSystem.Contract.Dtos;
public class AttendanceDto
    {
        public int Id { get; set; }

        public DateOnly DateOnly { get; set; } = new DateOnly();
    }
