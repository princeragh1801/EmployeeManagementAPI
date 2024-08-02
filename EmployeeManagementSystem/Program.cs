using EmployeeManagementSystem;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Employee Management System");
        int wantToContinue = 0, choice = 0;
        EmployeeManagementSystemClass system = new EmployeeManagementSystemClass();
        /*var employeesToAdd = 
        {
            { Name = "John Doe", Salary = 50000, ManagerId = 0, DepartmentId = 1 },
            { Name = "Jane Smith", Salary = 60000, ManagerId = 0, DepartmentId = 2 },
            { Name = "Peter Johnson", Salary = 70000, ManagerId = 1, DepartmentId = 1 },
            { Name = "Emily Davis", Salary = 80000, ManagerId = 2, DepartmentId = 2 }
        };*/
        system.AddEmployee("John Doe", 5000.09, 0, 1);
        system.AddEmployee("Jane Smith", 5000.09, 0, 2);
        system.AddEmployee("Peter Johnson", 5000.09, 0, 1);
        system.AddEmployee("Emily Davis", 5000.09, 0, 1);
        DepartmentManagement departmentSystem = new DepartmentManagement();
        departmentSystem.AssignMockDepartment();
        /*foreach (var employee in employeesToAdd)
        {
            system.AddEmployee(employee.Name, employee.Salary, employee.ManagerId, employee.DepartmentId);
        }*/
        do
        {

            Console.WriteLine("Enter 1 for adding the employee to the system :");
            Console.WriteLine("Enter 2 for getting the details of the employee :");
            Console.WriteLine("Enter 3 for deleting the employee from the system :");
            Console.WriteLine("Enter 4 for updating the employee in the system :");
            Console.WriteLine("Enter 5 for extracting the details of the manager of an employee :");
            Console.WriteLine("Enter 6 for extracting the details of the employees with the given managerId :");
            Console.WriteLine("Enter 7 for Adding new Department :");
            Console.WriteLine("Enter 8 for getting the details of employees with the given department :");
            Console.WriteLine("Enter 9 for getting the details of employees in the system :");
            Console.WriteLine("Enter 10 for Showing All Departments :");
            Console.WriteLine("Enter 11 for Employees Department wise :");
           // Console.WriteLine("Enter 12 for Getting all the managers :");


            choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    {
                        int managerId = 0;
                        Console.WriteLine("For adding the employee to the system");
                        Console.WriteLine("Enter name :");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter salary :");
                        double salary = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("Employee has manager ? 1/0");
                        int hasManager = Convert.ToInt32(Console.ReadLine());
  
                        if (hasManager == 1)
                        {
                            Console.WriteLine("Enter the managerId");
                            managerId = Convert.ToInt32(Console.ReadLine());
                        }

                        Console.WriteLine("For selecting the department Please enter id or 0:");
                        departmentSystem.PrintDepartments();
                        Console.WriteLine("0 for Adding new Department");
                        int departmentId = Convert.ToInt32(Console.ReadLine());
                        if (departmentId == 0)
                        {
                            Console.WriteLine("Enter department name :");
                            string departmentName = Console.ReadLine();
                            departmentId = departmentSystem.AddNewDepartment(departmentName);
                        }

                        bool operationSuccessful = system.AddEmployee(name ?? "", salary, managerId, departmentId);

                        if (operationSuccessful) Console.WriteLine("Employee has been added to the system");
                    }
                    break;

                case 2:
                    {
                        Console.WriteLine("For extracting the details of the employee");
                        Console.WriteLine("Press 1 for getting details with id, Press 2 for getting details with name");
                        int option = Convert.ToInt32(Console.ReadLine());

                        if(option == 1)
                        {
                            Console.WriteLine("Enter the id : ");
                            int id = Convert.ToInt32(Console.ReadLine());
                            Employee employee = system.GetEmployeeDetails(id);
                            if (employee == null)
                            {
                                Console.WriteLine("Employee doesn't exist");
                            }
                            else
                            {
                                system.DisplayEmployeeDetails(employee);
                            }
                        }else if(option == 2)
                        {
                            Console.WriteLine("Enter name for extracting details");
                            string name = Console.ReadLine();
                            var employees = system.SearchEmployeesByName(name);
                            system.DisplayAllEmployees(employees);
                        }
                        else
                        {
                            Console.WriteLine("No valid option"); 
                        }
                        
                        

                    }
                    break;

                case 3:
                    {
                        Console.WriteLine("For deleting the employee from the system");
                        Console.WriteLine("Enter the id : ");
                        int id = Convert.ToInt32(Console.ReadLine());
                        bool operationSuccessful = system.DeleteEmployee(id);
                        if (operationSuccessful)
                        {
                            Console.WriteLine("Employee has been deleted from the system");
                        }
                        else
                        {
                            Console.WriteLine("Employee is not present in the system");
                        }
                    }
                    break;

                case 4:
                    {
                        string name = "";
                        double salary = 0.0D;
                        int managerId = 0;
                        int departmentId = 0;

                        Console.WriteLine("For updating the details of the employee");
                        Console.WriteLine("Enter id : ");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Do you want to update the name ? 1/0");
                        int updateName = Convert.ToInt32(Console.ReadLine());
                        if (updateName == 1)
                        {
                            Console.WriteLine("Enter the name : ");
                            name = Console.ReadLine();
                        }
                        Console.WriteLine("Do you want to update the salary ? 1/0");
                        int updateSalary = Convert.ToInt32(Console.ReadLine());
                        if (updateSalary == 1)
                        {
                            Console.WriteLine("Enter the salary : ");
                            salary = Convert.ToDouble(Console.ReadLine());
                        }

                        Console.WriteLine("Do you want to update the manager ? 1/0");
                        int updateManager = Convert.ToInt32(Console.ReadLine());
                        if (updateManager == 1)
                        {
                            Console.WriteLine("Enter the managerId : ");
                            managerId = Convert.ToInt32(Console.ReadLine());
                        }

                        Console.WriteLine("Do you want to update the department ? 1/0");
                        int updateDepartment = Convert.ToInt32(Console.ReadLine());
                        if (updateDepartment == 1)
                        {
                            Console.WriteLine("For selecting the department Please enter id or 0:");
                            departmentSystem.PrintDepartments();
                            Console.WriteLine("0 for Adding new Department");
                            departmentId = Convert.ToInt32(Console.ReadLine());
                            if (departmentId == 0)
                            {
                                string departmentName = Console.ReadLine();
                                departmentId = departmentSystem.AddNewDepartment(departmentName);
                            }

                        }

                        bool operationSuccessful = system.UpdateEmployeeDetails(id, name, salary, managerId, departmentId);

                        if (operationSuccessful)
                        {
                            Console.WriteLine("Employee details has been updated");
                        }
                        else
                        {
                            Console.WriteLine("Can't update. Employee is not present in the system");
                        }
                    }
                    break;

                case 5:
                    {
                        Console.WriteLine("For extracting the details of the manager");
                        Console.WriteLine("Enter employeeId : ");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Employee employee = system.ManagerOfEmployeeWithId(id);
                        if (employee != null)
                        {
                            system.DisplayEmployeeDetails(employee);
                        }
                        else
                        {
                            Console.WriteLine("Didn't get the details of the manager");
                        }

                    }
                    break;

                case 6:
                    {
                        Console.WriteLine("For extracting the employees with the given manager id ");
                        Console.WriteLine("Enter manager id :");
                        int managerId = Convert.ToInt32(Console.ReadLine());

                        var employees = system.GetEmployeesWithManagerId(managerId);
                        if (employees.Count() > 0)
                        {
                            Console.WriteLine("Total employees works under the manager is " + employees.Count());
                            system.DisplayAllEmployees(employees);
                        }
                        else
                        {
                            Console.WriteLine("Employees not exist");
                        }
                    }

                    break;

                case 7:
                    {
                        Console.WriteLine("Enter department name :");
                        string departmentName = Console.ReadLine();
                        departmentSystem.AddNewDepartment(departmentName);
                        Console.WriteLine("Department Added successfully");
                    }
                    break;
                case 8:
                    {
                        Console.WriteLine("Enter the id of the department :");
                        departmentSystem.PrintDepartments();
                        int departmentId = Convert.ToInt32(Console.ReadLine());
                        List<Employee> employees = system.EmployeesOfDepartment(departmentId);
                        system.DisplayAllEmployees(employees);

                    }
                    break;
                case 9:
                    system.DisplaySystemEmployees();
                    break;
                case 10:
                    departmentSystem.PrintDepartments();
                    break;
                case 11:
                    var departments = departmentSystem.departments;
                    system.DisplayEmployeesDepartmentWise(departments);
                    break;
                case 12:

                    break;
                default:
                    Console.WriteLine("No valid choice");
                    break;
            }

            Console.WriteLine("Do you want to continue ? 1/0");
            wantToContinue = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
        } while (wantToContinue == 1);
    }
}

// TODO :
// Managers show karane hai
// har operation ke baad console clear karna hai