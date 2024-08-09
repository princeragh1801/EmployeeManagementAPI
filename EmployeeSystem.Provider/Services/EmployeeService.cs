using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.IdAndName;
using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Provider.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        public EmployeeService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<bool> CheckManagerAndEmployeeDepartment(int ?ManagerId, int? DepartmentId)
        {
            try
            {
                int managerId = ManagerId ?? 0;
                int departmentId = DepartmentId ?? 0;
                if (managerId != 0)
                {
                    var manager = await _context.Employees.FirstAsync(e => e.Id == managerId);
                    if (manager == null)
                    {
                        return false;
                    }
                    else if (departmentId != 0 && manager.DepartmentID != null && manager.DepartmentID != DepartmentId)
                    {
                        return false;
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool RoleExist(Role employeeRole)
        {
            int role = Convert.ToInt32(employeeRole);
            if (role > 2 || role < 0)
            {
                return false;
            }return true;
        }

        public async Task<bool> EmployeeExist(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id & e.IsActive);
            return employee != null;
        }

        public async Task<bool> UserExist(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Username == username);
            return user != null;
        }


        // get all employees
        public async Task<List<EmployeeDto>?> GetAll(int userId)
        {
            try
            {
                // fetching the user details
                var emp = await _context.Employees.FirstOrDefaultAsync(e => e.Id == userId);

                // if user is not a super-admin then sending the employees where the manager id == emp.id
                if(emp != null && emp.Role != Role.SuperAdmin)
                {
                    return await _context.Employees
                        .Where(e => e.ManagerID == emp.Id)
                        .Include(e => e.Manager)
                    .ThenInclude(e => e.Department)
                    .Where(e => e.IsActive)
                    .Select(e => new EmployeeDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Salary = e.Salary,
                        Role = e.Role,
                        ManagerName = e.Manager.Name,
                        DepartmentName = e.Department.Name,
                        DepartmentId = e.DepartmentID,
                        ManagerId = e.ManagerID,
                        CreatedBy = e.CreatedBy,
                        UpdatedBy = e.UpdatedBy,
                        CreatedOn = e.CreatedOn,
                        UpdatedOn = e.UpdatedOn,

                    }).ToListAsync();
                }


                // fetching employees and converting them to list of dto
                var employees = await _context.Employees
                    .Include(e => e.Manager)
                    .ThenInclude(e => e.Department)
                    .Where(e => e.IsActive)
                    .Select(e=> new EmployeeDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Salary = e.Salary,
                        Role = e.Role,
                        ManagerName = e.Manager.Name,
                        DepartmentName = e.Department.Name,
                        DepartmentId = e.DepartmentID,
                        ManagerId = e.ManagerID,
                        CreatedBy = e.CreatedBy,
                        UpdatedBy = e.UpdatedBy,
                        CreatedOn = e.CreatedOn,
                        UpdatedOn = e.UpdatedOn,

                    }).ToListAsync();

                return employees;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        // get employee by id
        public async Task<EmployeeInfo?> GetById(int id)
        {
            try
            {
                // check employee exist or not
                var check = await EmployeeExist(id);
                if (!check)
                {
                    return null;
                }
                
                // fetching the employee and converting it into the employee dto

                var employee = await _context.Employees
                    .Include(e => e.Manager)
                    .Include(e => e.Department)
                    .Select(e => new EmployeeInfo
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Salary = e.Salary,
                        Role = e.Role,
                        ManagerName = e.Manager.Name,
                        DepartmentName = e.Department.Name,
                        Phone = e.Phone,
                        Email = e.Email,
                        Address = e.Address,
                        ImageUrl = e.ImageUrl,
                        CreatedOn = e.CreatedOn,
                        CreatedBy = e.CreatedByName
                    }).FirstOrDefaultAsync(e => e.Id == id);



                return employee;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // add new employee
        public async Task<int> Add(int userID, AddEmployeeDto employeeDto)
        {
            try
            {
                // checking the role exist or not
                bool checkRole = RoleExist(employeeDto.Role);
                if (!checkRole)
                {
                    return -1;
                }

                // checking the user already exist or not
                bool checkUser = await UserExist(employeeDto.Username);

                if (checkUser)
                {
                    return -2;
                }

                int? managerId = employeeDto.ManagerID;
                int? departmentId = employeeDto.DepartmentID;

                // checking the manager belongs to the same department
                bool check = await CheckManagerAndEmployeeDepartment(managerId, departmentId);
                
                
                if (!check)
                {
                    return 0;
                }
                var obj = await _context.Employees.FirstAsync(e => e.Id == userID);
                var createdByName = obj.Name;
                // creating user 
                var user = new User
                {
                    Username = employeeDto.Username,
                    Password = employeeDto.Password,
                    CreatedBy = userID,
                    CreatedOn = DateTime.Now
                };

                // adding user to the db
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var firstLetter = employeeDto.Name.ElementAt(0);
                var lastLetter = employeeDto.Name.ElementAt(1);
                var imageUrl = $"https://ui-avatars.com/api/?name={firstLetter}+{lastLetter}";
                Console.WriteLine("First " + firstLetter);
                Console.WriteLine("Last " + lastLetter);
                Console.WriteLine("Url "+ imageUrl);
                var employee = new Employee
                {
                    Name = employeeDto.Name,
                    Email = employeeDto.Email,
                    Address = employeeDto.Address,
                    Salary = employeeDto.Salary,
                    Phone = "+91-"+employeeDto.Phone,
                    Role = employeeDto.Role,
                    ImageUrl = imageUrl,
                    DepartmentID = departmentId == 0 ? null : departmentId,
                    ManagerID = managerId == 0 ? null : managerId,
                    UserId = user.Id,
                    IsActive = true,
                    CreatedOn = DateTime.Now,
                    CreatedBy = userID,
                    CreatedByName = createdByName
                };

                // adding and saving to the database
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return employee.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // add list of employees
        public async Task<int> AddList(int userID, List<AddEmployeeDto> employees)
        {
            // started the transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // created a save point for roll back
                await transaction.CreateSavepointAsync("Adding list");
                var obj = await _context.Employees.FirstAsync(e => e.Id == userID);
                var createdByName = obj.Name;
                foreach (var employeeDto in employees)
                {
                    int? managerId = employeeDto.ManagerID;
                    int? departmentId = employeeDto.DepartmentID;

                    // checking the role
                    bool checkRole = RoleExist(employeeDto.Role);
                    if (!checkRole)
                    {
                        //await transaction.RollbackToSavepointAsync("Adding list");
                        return -1;
                    }

                    // checking whether the user with given username is already exist
                    bool checkUser = await UserExist(employeeDto.Username);

                    if (checkUser)
                    {
                        return -3;
                    }

                    // checking the employee and manager relation
                    bool check = await CheckManagerAndEmployeeDepartment(managerId, departmentId);

                    if (!check)
                    {
                       // await transaction.RollbackToSavepointAsync("Adding list");
                        return -2;
                    }

                    // creating the user
                    var user = new User
                    {
                        Username = employeeDto.Username,
                        Password = employeeDto.Password,
                        CreatedBy = userID,
                        CreatedOn = DateTime.Now,
                    };

                    // adding user to the db
                    _context.Users.Add(user);


                    var firstLetter = employeeDto.Name.ElementAt(0);
                    var lastLetter = employeeDto.Name.ElementAt(1);
                    var imageUrl = $"https://ui-avatars.com/api/?name={firstLetter}+{lastLetter}";

                    var employee = new Employee
                    {
                        Name = employeeDto.Name,
                        Email = employeeDto.Email,
                        Address = employeeDto.Address,
                        Phone = "+91-" + employeeDto.Phone,
                        Salary = employeeDto.Salary,
                        Role = employeeDto.Role,
                        DepartmentID = departmentId == 0 ? null : departmentId,
                        ManagerID = managerId == 0 ? null : managerId,
                        UserId = user.Id,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = userID,
                        CreatedByName = createdByName,
                    };

                    _context.Add(employee);
                    
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return 1;
            }
            catch (Exception ex)
            {
                // rollback if transaction got failed
                await transaction.RollbackToSavepointAsync("Adding list");
                throw new Exception(ex.Message);
            }
        }
        // delete employee
        public async Task<bool> DeleteById(int id)
        {
            try
            {
                // fetching the employee info
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id & e.IsActive);
                if (employee == null || employee.Role == Role.SuperAdmin)
                {
                    return false;

                }

                // optional
                /*var employees = await _context.Employees.Where(e => e.ManagerID == id).ToListAsync();
                foreach (var emp in employees)
                {
                    emp.ManagerID = null;
                }*/

                // soft delete
                employee.IsActive = false;

                // hard delete
                //_context.Employees.Remove(employee);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // update employee
        public async Task<EmployeeInfo?> Update(int userID, int id, UpdateEmployeeDto employeeDto)
        {
            try
            {
                // checking the role
                bool checkRole = RoleExist(employeeDto.Role);
                if (!checkRole)
                {
                    return null;
                }

                // check employee exist
                var checkEmp = await EmployeeExist(id);
                if (!checkEmp)
                {
                    return null;
                }

                int? managerId = employeeDto.ManagerID;
                int? departmentId = employeeDto.DepartmentID;

                // checking for employee manager relation
                bool check = await CheckManagerAndEmployeeDepartment(managerId, departmentId);

                if (!check)
                {
                    return null;
                }

                // fetching the employee and converting it into the employee dto

                var employeeToUpdate = await _context.Employees
                    .FirstOrDefaultAsync(e=> e.Id == id);

               
                // updating the feilds if employee exist
                if (employeeToUpdate == null)
                {
                    return null;
                    
                }
                var firstLetter = employeeDto.Name.ElementAt(0);
                var lastLetter = employeeDto.Name.ElementAt(1);
                
                var imageUrl = $"https://ui-avatars.com/api/?name={firstLetter}+{lastLetter}";
                var obj = await _context.Employees.FirstAsync(e => e.Id == userID);
                var updatedByName = obj.Name;

                employeeToUpdate.Name = employeeDto.Name;
                employeeToUpdate.ImageUrl = imageUrl;
                employeeToUpdate.Email = employeeDto.Email;
                employeeToUpdate.Address = employeeDto.Address;
                employeeToUpdate.Phone = "+91-" + employeeDto.Phone;
                employeeToUpdate.Salary = employeeDto.Salary;
                employeeToUpdate.Role = employeeDto.Role;
                employeeToUpdate.DepartmentID = employeeDto.DepartmentID;
                employeeToUpdate.ManagerID = employeeDto.ManagerID;
                employeeToUpdate.UpdatedOn = DateTime.Now;
                employeeToUpdate.UpdatedBy = userID;
                employeeToUpdate.UpdatedByName = updatedByName;

                await _context.SaveChangesAsync();

                var employee = await GetById(id);
                return employee;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        // get manager details
        public async Task<List<EmployeeDto>> GetManagers()
        {
            try
            {
                // fetching managers and converting them to list of dto
                var managers = await _context.Employees
                .Where(e => e.IsActive & _context.Employees.Any(emp => emp.ManagerID == e.Id))
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Salary = e.Salary,
                    Role = e.Role,
                    ManagerName = e.Manager.Name,
                    DepartmentName = e.Department.Name,
                    DepartmentId = e.DepartmentID,
                    ManagerId = e.ManagerID
                })
                .ToListAsync();

                /*var managers = await _context.Employees
                    .Select(emp => emp.Manager)
                    .Include(d => d.Department)
                    .Include(m => m.Manager)
                    .Distinct()
                    .Where(emp => emp.IsActive & emp.ManagerID != null)
                    .Where(manager => manager != null & manager.IsActive)
                    .Select(manager => new EmployeeDto
                    {
                        Id = manager.Id, 
                        Name = manager.Name, 
                        Salary = manager.Salary,
                        Role = manager.Role,
                        DepartmentName = manager.Department.Name,
                        ManagerName = manager.Manager.Name,
                        DepartmentId = manager.Department.Id,
                        ManagerId = manager.Manager.Id,
                        CreatedBy = manager.CreatedBy,
                        UpdatedBy = manager.UpdatedBy,
                        CreatedOn = manager.CreatedOn,
                        UpdatedOn = manager.UpdatedOn,

                    }).ToListAsync();*/

                return managers;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    

        public async Task<List<EmployeeIdAndName>?> GetEmployeesWithDepartmentName(int id)
        {
            try
            {
                // check whether the department with given id is exist or not
                var res = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id & d.IsActive);
                if(res == null)
                {
                    return null;
                }
                /*var employees = await _context.Departments.Include(d => d.Employees).ThenInclude(e => e.Manager).Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    DepartmentName = departmentName,
                    
                })*/

                // fetching the employees of the department with id-[x]
                var employees = await _context.Employees
                    .Where(e => e.DepartmentID == res.Id & e.IsActive)
                    .Select(e => new EmployeeIdAndName
                    {
                        Id = e.Id,
                        Name = e.Name,
                    }).ToListAsync();

                return employees;
                
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<List<EmployeeIdAndName>> GetEmployeeIdAndName()
        {
            try
            {
                var employeeInfo = await _context.Employees
                    .Include(e => e.Department)
                    .Select(e => new EmployeeIdAndName
                    {
                        Id = e.Id,
                        Name = e.Name,
                        DepartmentName = e.Department.Name
                    }).ToListAsync();
                return employeeInfo;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
