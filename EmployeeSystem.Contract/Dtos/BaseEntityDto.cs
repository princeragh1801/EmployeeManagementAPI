namespace EmployeeSystem.Contract.Dtos
{
    public class BaseEntityDto
    {
        public int ?CreatedBy { get; set; }
        public int ?UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ?UpdatedOn { get; set; }
    }
}
