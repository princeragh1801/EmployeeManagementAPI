using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.Count;
using EmployeeSystem.Contract.Dtos.IdAndName;
using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Dtos.Info.PaginationInfo;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Provider.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IUtilityService _utilityService;
        public EmployeeService(ApplicationDbContext applicationDbContext, IEmailService emailService, IUtilityService utilityService, IMapper mapper)
        {
            _context = applicationDbContext;
            _emailService = emailService;
            _utilityService = utilityService;
            _mapper = mapper;
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
            var user = await _context.Employees.FirstOrDefaultAsync(e => e.Username == username);
            return user != null;
        }

        public async Task<PaginatedItemsDto<List<EmployeePaginationInfo>>> Get(IEnumerable<Claim> claims, PaginatedDto<Role?> paginatedDto)
        {
            try
            {
                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;
                var role = paginatedDto.Status;
                var userId = Convert.ToInt32(claims.First(e => e.Type == "UserId")?.Value);
                var userRole = claims.First(e => e.Type == "Role")?.Value;
                
                // including the details of the employee in query
                var query = _context.Employees
                    .Include(e => e.Manager)
                    .Include(m => m.Department)
                    .Where(e => e.IsActive);

                if (userRole != "SuperAdmin")
                {
                    query = query.Where(e => e.ManagerID != null && e.ManagerID == userId);
                }

                var range = paginatedDto.DateRange;

                // range filter
                if (range != null)
                {
                    var startDate = range.StartDate;
                    var endDate = range.EndDate;

                    query = query.Where(t => t.CreatedOn >= startDate && t.CreatedOn <= endDate);
                }

                if (role != null)
                {
                    query = query.Where(e => e.Role == role);
                }
                // applying search filter on that
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(e => e.Name.Contains(search) || e.Department.Name.Contains(search) || e.Manager.Name.Contains(search) || e.Phone.Contains(search));
                }

                query = orderBy == SortedOrder.NoOrder ? query : _utilityService.GetOrdered<Employee>(query, orderKey, orderBy == SortedOrder.Ascending ? true : false);

                // calculating the total count and pages
                var totalCount = query.Count();
                var totalPages = totalCount / paginatedDto.PagedItemsCount;
                if (totalCount % paginatedDto.PagedItemsCount != 0)
                {
                    totalPages++;
                }

                // now extrating employees of the page-[x]
                var employees = await query.
                    Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                    .Take(paginatedDto.PagedItemsCount)
                    .ProjectTo<EmployeePaginationInfo>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                // creating new dto to send the info
                PaginatedItemsDto<List<EmployeePaginationInfo>> res = new PaginatedItemsDto<List<EmployeePaginationInfo>>();

                res.Data = employees;
                res.TotalPages = totalPages;
                res.TotalItems = totalCount;
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<EmployeeCount> GetCounts()
        {
            try
            {
                var query = _context.Employees.Where(t => t.IsActive);
                var totalActive = await query.CountAsync();
                var superAdmin = await query.Where(t => t.Role == Role.SuperAdmin).CountAsync();
                var admin = await query.Where(t => t.Role == Role.Admin).CountAsync();
                var employee = await query.Where(t => t.Role == Role.Employee).CountAsync();
                var count = new EmployeeCount
                {
                    Employee = employee,
                    Admin = admin,
                    SuperAdmin = superAdmin,
                    Total = totalActive,
                };
                return count;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // get all employees
        public async Task<List<EmployeeDto>?> GetAll(IEnumerable<Claim> claims)
        {
            try
            {
                var userId = Convert.ToInt32(claims.First(e => e.Type == "UserId")?.Value);
                var userRole = claims.First(e => e.Type == "Role")?.Value;
                // fetching the user details
                var emp = await _context.Employees.FirstOrDefaultAsync(e => e.Id == userId);

                // if user is not a super-admin then sending the employees where the manager id == emp.id
                if(userRole != "SuperAdmin")
                {
                    return await _context.Employees
                        .Where(e => e.ManagerID == emp.Id)
                        .Include(e => e.Manager)
                    .ThenInclude(e => e.Department)
                    .Where(e => e.IsActive)
                    .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                }


                // fetching employees and converting them to list of dto
                var employees = await _context.Employees
                    .Include(e => e.Manager)
                    .ThenInclude(e => e.Department)
                    .Where(e => e.IsActive)
                    .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

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
                    .Include(e => e.Creator)
                    .ProjectTo<EmployeeInfo>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == id);

                return employee;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // add new employee
        public async Task<int> Add(IEnumerable<Claim> claims, AddEmployeeDto employeeDto)
        {
            try
            {
                var userId = Convert.ToInt32(claims.First(e => e.Type == "UserId")?.Value);
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
                
                var firstLetter = employeeDto.Name.ElementAt(0);
                var lastLetter = employeeDto.Name.ElementAt(1);
                var imageUrl = $"https://ui-avatars.com/api/?name={firstLetter}+{lastLetter}";
                Console.WriteLine("First " + firstLetter);
                Console.WriteLine("Last " + lastLetter);
                Console.WriteLine("Url "+ imageUrl);
                var employee = _mapper.Map<Employee>(employeeDto);
                employee.CreatedBy = userId;
                employee.ImageUrl = imageUrl;
                // adding and saving to the database
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                // Send the email with the username and password
                /*string subject = "Welcome to the Employee Management system";
                string body = $"Dear {employee.Name},\n\nYour account has been created.\n\nUsername: {user.Username}\nPassword: {user.Password}\n";

                await _emailService.SendEmail(employee.Email, subject, body);*/

                return employee.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // add list of employees
        public async Task<int> AddList(IEnumerable<Claim> claims, List<AddEmployeeDto> employees)
        {
            // started the transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var userId = Convert.ToInt32(claims.First(e => e.Type == "UserId")?.Value);
                // created a save point for roll back
                await transaction.CreateSavepointAsync("Adding list");
                foreach (var employeeDto in employees)
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

                    var firstLetter = employeeDto.Name.ElementAt(0);
                    var lastLetter = employeeDto.Name.ElementAt(1);
                    var imageUrl = $"https://ui-avatars.com/api/?name={firstLetter}+{lastLetter}";

                    var employee = _mapper.Map<Employee>(employeeDto);
                    employee.CreatedBy = userId;
                    employee.ImageUrl = imageUrl;

                    // adding and saving to the database
                    _context.Employees.Add(employee);

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
        public async Task<bool?> Update(IEnumerable<Claim> claims, int id, UpdateEmployeeDto employeeDto)
        {
            try
            {
                var userId = Convert.ToInt32(claims.First(e => e.Type == "UserId")?.Value);
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
                    return false;
                    
                }
                var firstLetter = employeeDto.Name.ElementAt(0);
                var lastLetter = employeeDto.Name.ElementAt(1);
                
                var imageUrl = $"https://ui-avatars.com/api/?name={firstLetter}+{lastLetter}";

                _mapper.Map(employeeToUpdate, employeeDto);

                employeeToUpdate.ImageUrl = imageUrl;
                employeeToUpdate.UpdatedBy = userId;

                await _context.SaveChangesAsync();

                var employee = await GetById(id);
                return true;


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
                    .ProjectTo<EmployeeIdAndName>(_mapper.ConfigurationProvider)
                    .ToListAsync();

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
                    .ProjectTo<EmployeeIdAndName>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return employeeInfo;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

}
