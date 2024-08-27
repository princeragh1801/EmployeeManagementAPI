namespace EmployeeSystem.Contract.Dtos.Add
{
    public class AddTaskLogDto
    {
        public int TaskId { get; set; }
        public required string Message { get; set; }
    }
}
