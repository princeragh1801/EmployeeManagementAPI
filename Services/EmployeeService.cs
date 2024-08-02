using EmployeeSystemWebApi.Contract;
using EmployeeSystemWebApi.Contract.Dtos;
using EmployeeSystemWebApi.Contract.Models;
using EmployeeSystemWebApi.Provider.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSystemWebApi.Provider.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        public EmployeeService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<bool> EmployeeExist(int id)
        {
            var employee = await _context.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();
            return employee != null;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            try
            {
                var employees = await _context.Employees.ToListAsync();
                return employees;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        // get employee by id
        public async Task<Employee?> GetById(int id)
        {
            try
            {
                var employee = await _context.Employees.Where(e=> e.Id == id).FirstOrDefaultAsync();
                 
                if(employee == null)
                {
                    throw new Exception("Employee doesn't exist");
                }
                return employee;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }

        // add new employee
        public async Task AddNew(Employee employee)
        {
            try
            {
                if(employee.ManagerID != 0)
                {
                    var managerExist = await _context.Employees.Where(e => e.Id == employee.ManagerID).FirstOrDefaultAsync();
                    if (managerExist == null)
                    {
                        throw new Exception("Manager doesn't exist");
                    }
                }
                
                if(employee.DepartmentID != 0)
                {
                    var departmentExist = await _context.Departments.Where(d => d.Id == employee.DepartmentID).FirstOrDefaultAsync();

                    if (departmentExist == null)
                    {
                        throw new Exception("Department doesn't exist");
                    }
                }
                

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // delete employee
        public async Task DeleteById(int id)
        {
            try
            {
                var employee = await _context.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();
                if(employee == null)
                {
                    throw new Exception("Employee doesn't exist");
                    
                }

                var employees = await _context.Employees.Where(e=> e.ManagerID == id).ToListAsync();
                foreach(var emp in employees)
                {
                    emp.ManagerID = null;
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // update employee
        public async Task Update(Employee employee)
        {
            try
            {
                
                var employeeToUpdate = await _context.Employees.Where(e => e.Id == employee.Id).FirstOrDefaultAsync();

               // error 
                /*if (employee.ManagerID != null)
                {
                    int managerId = employee.ManagerID;
                    var checkManager = await EmployeeExist(employee.ManagerID);
                }*/

                if (employeeToUpdate != null)
                {
                    
                    employeeToUpdate.Name = employee.Name;
                    employeeToUpdate.Salary = employee.Salary;
                    employeeToUpdate.Role = employee.Role;
                    employeeToUpdate.DepartmentID = employee.DepartmentID;
                    employeeToUpdate.ManagerID = employee.ManagerID;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
