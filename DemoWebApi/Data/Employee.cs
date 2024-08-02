﻿namespace DemoWebApi.Data
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public int? DepartmentId { get; set; }
        public int? ManagerId { get; set; }

        public Department Department { get; set; }
        public Employee Manager { get; set; }

        public ICollection<Employee> Subordinates { get; set; }
    }
}
