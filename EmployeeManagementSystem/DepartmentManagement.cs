using System;
using System.Collections.Generic;
using EmployeeManagementSystem;

namespace EmployeeManagementSystem
{
    public class DepartmentManagement
    {
        public List<Department> departments = new List<Department>();
        public void AssignMockDepartment()
        {
            AddNewDepartment("Sales");
            AddNewDepartment("Marketing");
            AddNewDepartment("Development");
            AddNewDepartment("HR");

        }
        public void PrintDepartments()
        {
            Console.WriteLine("Department list --> ");

            foreach(var department in departments)
            {
                Console.WriteLine("Department Id : "+department.Id+" Department Name : "+ department.Name);
            }
        }
        
        public int AddNewDepartment(string name)
        {
            int id = departments.Count() + 1;
            Department department = new Department(id, name);
            departments.Add(department);
            return id;
        }
            
    }
}