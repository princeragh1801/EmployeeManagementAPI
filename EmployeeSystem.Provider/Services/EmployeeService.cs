using Microsoft.AspNetCore.Http;

namespace EmployeeSystem.Provider.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IUtilityService _utilityService;
    private readonly ICloudinaryService _cloudinaryService;
    public EmployeeService(ApplicationDbContext applicationDbContext, IEmailService emailService, IUtilityService utilityService, IMapper mapper, ICloudinaryService cloudinaryService)
    {
        _context = applicationDbContext;
        _emailService = emailService;
        _utilityService = utilityService;
        _mapper = mapper;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<List<EmployeeDto>?> ConvertEmployeeToEmployeeDto(IQueryable<Employee> query)
    {
        try
        {
            var employees = await query.Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                Role = e.Role,
                Salary = e.Salary,
                CreatedBy = e.CreatedBy,
                CreatedOn = e.CreatedOn,
                ManagerId = e.ManagerID,
                DepartmentId = e.DepartmentID,
                DepartmentName = e.Department != null ? e.Department.Name : null,
                ManagerName = e.Manager != null ? e.Manager.Name : null,

            }).ToListAsync();
            return employees;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EmployeeInfo?> ConvertEmployeeToEmployeeInfo(IQueryable<Employee> query, int id)
    {
        try
        {
            var employee = await query.Select(e => new EmployeeInfo
            {
                Id = e.Id,
                Name = e.Name,
                Role = e.Role,
                Salary = e.Salary,
                CreatedBy = e.Creator != null ? e.Creator.Name : "",
                CreatedOn = e.CreatedOn,
                DepartmentName = e.Department != null ? e.Department.Name : null,
                ManagerName = e.Manager != null ? e.Manager.Name : null,
                Email = e.Email,
                Phone = e.Phone,
                Address = e.Address,
                ImageUrl = e.ImageUrl,

            }).FirstOrDefaultAsync(e => e.Id == id);
            return employee;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    
    public async Task<List<EmployeePaginationInfo>?> ConvertEmployeeToEmployeePaginationInfo(IQueryable<Employee> query)
    {
        try
        {
            var employees = await query.Select(e => new EmployeePaginationInfo
            {
                Id = e.Id,
                Name = e.Name,
                Role = e.Role,
                Salary = e.Salary,
                CreatedOn = e.CreatedOn,
                DepartmentName = e.Department != null ? e.Department.Name : null,
                ManagerName = e.Manager != null ? e.Manager.Name : null,
                Email = e.Email
            }).ToListAsync();
            return employees;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<List<EmployeeIdAndName>?> ConvertEmployeeToEmployeeIdAndName(IQueryable<Employee> query)
    {
        try
        {
            var employees = await query.Select(e => new EmployeeIdAndName
            {
                Id = e.Id,
                Name = e.Name,
                DepartmentName = e.Department != null ? e.Department.Name : ""
            }).ToListAsync();
            return employees;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> CheckManagerAndEmployeeDepartment(int? ManagerId, int? DepartmentId)
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
        }
        return true;
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
                .Where(e => e.IsActive && e.Id != userId && e.Role != Role.SuperAdmin)
                .AsNoTracking();

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
            query = query.
                Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                .Take(paginatedDto.PagedItemsCount);

            var employees = await ConvertEmployeeToEmployeePaginationInfo(query);
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


    public async Task<EmployeeCount> GetCounts(int userId)
    {
        try
        {
            var query = _context.Employees.Where(e => e.IsActive && e.Id != userId && e.Role != Role.SuperAdmin).AsNoTracking();
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
            var employees = new List<EmployeeDto>();
            // if user is not a super-admin then sending the employees where the manager id == emp.id
            if (userRole != "SuperAdmin")
            {
                var query = _context.Employees
                    .Where(e => e.ManagerID == userId)
                    .Include(e => e.Manager)
                .ThenInclude(e => e.Department)
                .Where(e => e.IsActive)
                .AsNoTracking();

                employees = await ConvertEmployeeToEmployeeDto(query);
                return employees;
            }


            // fetching employees and converting them to list of dto
            var queryEmp = _context.Employees
                .Include(e => e.Manager)
                .ThenInclude(e => e.Department)
                .Where(e => e.IsActive)
                .AsNoTracking();
            employees = await ConvertEmployeeToEmployeeDto(queryEmp);
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

            var employeeQuery = _context.Employees
                .Include(e => e.Manager)
                .Include(e => e.Department)
                .Include(e => e.Creator)
                .AsNoTracking();
            var employee = await ConvertEmployeeToEmployeeInfo(employeeQuery, id);
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
            if (!checkRole || employeeDto.Role == Role.SuperAdmin)
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
            Console.WriteLine("Url " + imageUrl);
            var employee = new Employee
            {
                Username = employeeDto.Username,
                Password = employeeDto.Password,
                Name = employeeDto.Name,
                Role = employeeDto.Role,
                Salary = employeeDto.Salary,
                Email = employeeDto.Email,
                Address = employeeDto.Address,
                Phone = employeeDto.Phone,
                ManagerID = employeeDto.ManagerID,
                DepartmentID = employeeDto.DepartmentID,
                CreatedBy = userId,
                ImageUrl = imageUrl,
                CreatedOn = DateTime.Now,
            };
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
                if (!checkRole || employeeDto.Role == Role.SuperAdmin)
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

                var employee = new Employee
                {
                    Username = employeeDto.Username,
                    Password = employeeDto.Password,
                    Name = employeeDto.Name,
                    Role = employeeDto.Role,
                    Salary = employeeDto.Salary,
                    Email = employeeDto.Email,
                    Address = employeeDto.Address,
                    Phone = employeeDto.Phone,
                    ManagerID = employeeDto.ManagerID,
                    DepartmentID = employeeDto.DepartmentID,
                    CreatedBy = userId,
                    ImageUrl = imageUrl,
                    CreatedOn = DateTime.Now,
                };

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
            if (!checkRole || employeeDto.Role == Role.SuperAdmin)
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
                .FirstOrDefaultAsync(e => e.Id == id);


            // updating the details if employee exist
            if (employeeToUpdate == null)
            {
                return false;

            }
            var firstLetter = employeeDto.Name.ElementAt(0);
            var lastLetter = employeeDto.Name.ElementAt(1);

            var imageUrl = $"https://ui-avatars.com/api/?name={firstLetter}+{lastLetter}";


            _mapper.Map(employeeDto, employeeToUpdate);
            
            employeeToUpdate.Name = employeeDto.Name;
            employeeToUpdate.Role = employeeDto.Role;
            employeeToUpdate.Salary = employeeDto.Salary;
            employeeToUpdate.Email = employeeDto.Email;
            employeeToUpdate.Address = employeeDto.Address;
            employeeToUpdate.Phone = employeeDto.Phone;
            employeeToUpdate.ManagerID = employeeDto.ManagerID;
            employeeToUpdate.DepartmentID = employeeDto.DepartmentID;
            employeeToUpdate.CreatedBy = userId;
            employeeToUpdate.ImageUrl = imageUrl;
            employeeToUpdate.CreatedOn = DateTime.Now;
            employeeToUpdate.ImageUrl = imageUrl;
            employeeToUpdate.UpdatedBy = userId;

            int rowAffected = await _context.SaveChangesAsync();
            return rowAffected > 0;

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
                ManagerName = e.Manager!.Name,
                DepartmentName = e.Department.Name,
                DepartmentId = e.DepartmentID,
                ManagerId = e.ManagerID
            })
            .AsNoTracking()
            .ToListAsync();

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
            if (res == null)
            {
                return null;
            }
            /*var employees = await _context.Departments.Include(d => d.Employees).ThenInclude(e => e.Manager).Select(e => new EmployeeDto
            {
                Id = e.Id,
                DepartmentName = departmentName,

            })*/

            // fetching the employees of the department with id-[x]
            var employeeInfo = _context.Employees
                .Where(e => e.DepartmentID == res.Id & e.IsActive)
                .AsNoTracking();
            var employees = await ConvertEmployeeToEmployeeIdAndName(employeeInfo);
            return employees ?? new List<EmployeeIdAndName>();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<EmployeeIdAndName>> GetEmployeeIdAndName()
    {
        try
        {
            var employeeInfo = _context.Employees
                .Include(e => e.Department);
            var employees = await ConvertEmployeeToEmployeeIdAndName(employeeInfo);
            return employees??new List<EmployeeIdAndName>();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UpdateEmployeeDto> GetEmployeeToUpdate(int id)
    {
        try
        {
            var employee = await _context.Employees.
                Where(e => e.Id == id).
                Select(e => new UpdateEmployeeDto
                {
                    Name = e.Name,
                    Email = e.Email,
                    Address = e.Address,
                    DepartmentID = e.DepartmentID,
                    ManagerID = e.ManagerID,
                    Phone = e.Phone,
                    Role = e.Role,
                    Salary = e.Salary,
                }).FirstAsync();

            return employee;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> UpdateAvatar(int id, IFormFile file)
    {
        try
        {
            var employee = await _context.Employees.FirstAsync(e => e.Id == id);
            var imageUrl = await _cloudinaryService.UploadFile(file);
            employee.ImageUrl = imageUrl;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}


