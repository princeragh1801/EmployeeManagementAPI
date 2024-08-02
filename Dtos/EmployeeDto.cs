namespace EmployeeSystemWebApi.Contract.Dtos
{
    public class EmployeeDto
    {
        public string Name { get; set; }
        public string ?DepartmentName { get; set; }
        public string ?ManagerName { get; set; }
        public string ?Role {  get; set; }
        public decimal Salary { get; set; }
        public int ?DepartmentId { get; set; }
        public int ?ManagerId { get; set; }

    }
}
