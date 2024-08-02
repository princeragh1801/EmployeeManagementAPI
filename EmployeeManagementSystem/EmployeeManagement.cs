using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagementSystem;

namespace EmployeeManagementSystem
{
     public class EmployeeManagementSystemClass
    {
        private List<Employee> employees = new List<Employee>();

        public void DisplaySystemEmployees()
        {
            foreach (var employee in employees)
            {
                DisplayEmployeeDetails(employee);
            }
        }

        public void DisplayAllEmployees(List<Employee> employeeList)
        {
            foreach (var employee in employeeList)
            {
                DisplayEmployeeDetails(employee);
            }
        }

        public void DisplayEmployeeDetails(Employee employee)
        {
            Console.WriteLine("Name --> " + employee.Name);
            Console.WriteLine("Id --> " + employee.Id);
            Console.WriteLine("Salary --> " + employee.Salary);
            Console.WriteLine("ManagerId --> " + employee.ManagerId);
            Console.WriteLine("DepartmentId --> "+ employee.DepartmentId);
        }
        
        public void DisplayEmployeesDepartmentWise(List<Department> departments)
        {
            foreach(var department in departments)
            {
                var empWithDepartment = EmployeesOfDepartment(department.Id);
                if(empWithDepartment.Count() > 0)
                {
                    Console.WriteLine("Department Name : "+ department.Name);
                    Console.WriteLine("Employees :");
                    foreach(var employee in empWithDepartment)
                    {
                        Console.WriteLine(employee.Name);
                    }
                    Console.WriteLine();
                }
            }
        }
        public bool AddEmployee(string name, double salary, int managerId = 0, int departmentId = 0)
        {
            // manageId = 0 defines that it has no manager at all

            int id = employees.Count() + 1;

            if (id > 1)
            {
                var last = employees.Last();
                id = last.Id + 1;
            }

            Employee employee = new Employee();

            if (managerId > 0)
            {
                var empInd = employees.FindIndex(emp => emp.Id == managerId);
                if (empInd >= 0)
                {
                    employee.ManagerId = managerId;
                }
                else
                {
                    Console.WriteLine("Manager doesn't exist");
                    return false;
                }
            }


            // assigning the values to employee
            employee.Id = id;
            employee.Name = name;
            employee.Salary = salary;
            employee.DepartmentId = departmentId;

            // Added the new employee to the list of employees
            employees.Add(employee);
            return true;
        }

        public Employee? GetEmployeeDetails(int id)
        {
            var emp = employees.Where(x => x.Id == id).FirstOrDefault();

            return emp;
        }

        public bool UpdateEmployeeDetails(int id, string name = "", double salary = 0.0D, int managerId = 0, int departmentId = 0) 
        {
            

            var employee = employees.Find(emp => emp.Id == id);

            if (employee == null)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(name))
            {
                employee.Name = name;
            }
            if (salary > 0.0D)
            {
                employee.Salary = salary;
            }
            if (managerId != 0)
            {
                employee.ManagerId = managerId;
            }if (departmentId != 0)
            {
                employee.DepartmentId= departmentId;
            }
            return true;
        }

        public bool DeleteEmployee(int id)
        {
            var employee = GetEmployeeDetails(id);
            if (employee == null)
            {
                return false;
            }
            if (employee.Id == 0)
            {
                return false;
            }

            employees.Remove(employee);

            return true;
        }

        public Employee? ManagerOfEmployeeWithId(int id)
        {
            var employee = GetEmployeeDetails(id);
            if(employee == null)
            {
                return null;
            }
            int manageId = employee.ManagerId;
            var manager = GetEmployeeDetails(manageId);
            return manager;
        }

        public List<Employee> GetEmployeesWithManagerId(int managerId)
        {

            var obj = employees.FindAll(employee => employee.ManagerId == managerId);

            return obj;
        }
        public List<Employee> GetAllManagers()
        {
            var managers = employees.Where(emp => emp.ManagerId != 0).ToList();
            return managers;
        }
        public List<Employee> SearchEmployeesByName(string name)
        {
            var obj = employees.FindAll(emp => emp.Name.Contains(name));

            return obj;
        }
        public List<Employee>? EmployeesOfDepartment(int departmentId)
        {
            var employeesWithDepartment = employees.Where(emp=> emp.DepartmentId == departmentId).ToList();

            return employeesWithDepartment;
        }
    }
}
