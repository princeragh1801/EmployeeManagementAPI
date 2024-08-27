namespace EmployeeSystem.Contract.Dtos
{
    public class DepartmentDto : BaseEntityDto
    {
        public int ?Id { get; set; }
        public required string Name { get; set; }

    }
}
