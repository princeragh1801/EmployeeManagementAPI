using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Provider.Services
{
    public class PaginatedService : IPaginatedService
    {
        private readonly ApplicationDbContext _context;

        public PaginatedService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public IQueryable<Employee> GetOrdered(IQueryable<Employee> query, string columnName, bool ace = true)
        {
            if (columnName == "Name")
            {
                query = ace ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
            else if (columnName == "ManagerName")
            {
                query = ace ? query.OrderBy(x => x.Manager.Name) : query.OrderByDescending(x => x.Manager.Name);
            }
            else if (columnName == "DeparmentName")
            {
                query = ace ? query.OrderBy(x => x.Department.Name) : query.OrderByDescending(x => x.Department.Name);
            }
            else if (columnName == "Salary")
            {
                query = ace ? query.OrderBy(x => x.Salary) : query.OrderByDescending(x => x.Salary);
            }
            else if (columnName == "CreatedOn")
            {
                query = ace ? query.OrderBy(x => x.CreatedOn) : query.OrderByDescending(x => x.CreatedOn);
            }else if (columnName == "UpdatedOn")
            {
                query = ace ? query.OrderBy(x => x.UpdatedOn) : query.OrderByDescending(x => x.UpdatedOn);
            }else
            {
                query = ace ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
            }
            return query;
        }

        public IQueryable<Department> GetOrdered(IQueryable<Department> query, string columnName, bool ace = true)
        {
            if (columnName == "Name")
            {
                query = ace ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }else if (columnName == "CreatedOn")
            {
                query = ace ? query.OrderBy(x => x.CreatedOn) : query.OrderByDescending(x => x.CreatedOn);
            }
            else if (columnName == "UpdatedOn")
            {
                query = ace ? query.OrderBy(x => x.UpdatedOn) : query.OrderByDescending(x => x.UpdatedOn);
            }
            else
            {
                query = ace ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
            }
            return query;
        }

        public IQueryable<Project> GetOrdered(IQueryable<Project> query, string columnName, bool ace = true)
        {
            if (columnName == "Name")
            {
                query = ace ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
            else if (columnName == "Description")
            {
                query = ace ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description);
            }
            else if (columnName == "CreatedOn")
            {
                query = ace ? query.OrderBy(x => x.CreatedOn) : query.OrderByDescending(x => x.CreatedOn);
            }
            else if (columnName == "UpdatedOn")
            {
                query = ace ? query.OrderBy(x => x.UpdatedOn) : query.OrderByDescending(x => x.UpdatedOn);
            }
            else
            {
                query = ace ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
            }
            return query;
        }

        public IQueryable<Tasks> GetOrdered(IQueryable<Tasks> query, string columnName, bool ace = true)
        {
            if (columnName == "Name")
            {
                query = ace ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
            else if (columnName == "Description")
            {
                query = ace ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description);
            }
            else if (columnName == "CreatedOn")
            {
                query = ace ? query.OrderBy(x => x.CreatedOn) : query.OrderByDescending(x => x.CreatedOn);
            }
            else if (columnName == "UpdatedOn")
            {
                query = ace ? query.OrderBy(x => x.UpdatedOn) : query.OrderByDescending(x => x.UpdatedOn);
            }
            else
            {
                query = ace ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
            }
            return query;
        }

        public async Task<List<EmployeeDto>> GetEmployees(PaginatedDto paginatedDto)
        {
            try
            {
                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;
                var query = _context.Employees
                    .Include(e => e.Manager)
                    .Include(m => m.Department)
                    .Where(e => e.IsActive);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(e => e.Name.Contains(search));
                }

                query = orderBy == SortedOrder.NoOrder ? query : GetOrdered(query, orderKey, orderBy == SortedOrder.Ascending ? true : false);

                var employees = await query.
                    Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                    .Take(paginatedDto.PagedItemsCount)
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

                return employees;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<List<DepartmentDto>> GetDepartments(PaginatedDto paginatedDto)
        {
            try
            {
                var query = _context.Departments.Where(d => d.IsActive);

                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d => d.Name.Contains(search));
                }

                query = orderBy == SortedOrder.NoOrder ? query : GetOrdered(query, orderKey, orderBy == SortedOrder.Ascending ? true : false);

                var departments = await query
                    .Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                    .Take(paginatedDto.PagedItemsCount)
                    .Select(
                    d => new DepartmentDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        CreatedOn = d.CreatedOn,
                        UpdatedOn = d.UpdatedOn,
                        CreatedBy = d.CreatedBy,
                        UpdatedBy = d.UpdatedBy,
                    }).ToListAsync();

                return departments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ProjectDto>> GetProjects(PaginatedDto paginatedDto)
        {
            try
            {
                var query = _context.Projects.Where(d => d.IsActive);

                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d => d.Name.Contains(search));
                }

                query = orderBy == SortedOrder.NoOrder ? query : GetOrdered(query, orderKey, orderBy == SortedOrder.Ascending ? true : false);

                var projects = await query
                    .Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                    .Take(paginatedDto.PagedItemsCount)
                    .Select(e => new ProjectDto
                     {
                         Id = e.Id,
                         Name = e.Name,
                         Description = e.Description,
                         CreatedBy = e.CreatedBy,
                         UpdatedBy = e.UpdatedBy,
                         CreatedOn = e.CreatedOn,
                         UpdatedOn = e.UpdatedOn,
                     }).ToListAsync();

                return projects;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<TasksDto>> GetTasks(PaginatedDto paginatedDto)
        {
            try
            {
                var query = _context.Tasks.Where(d => d.IsActive);

                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d => d.Name.Contains(search));
                }

                query = orderBy == SortedOrder.NoOrder ? query : GetOrdered(query, orderKey, orderBy == SortedOrder.Ascending ? true : false);

                var tasks = await query
                    .Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                    .Take(paginatedDto.PagedItemsCount)
                    .Select(e => new TasksDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Description = e.Description,
                        CreatedBy = e.CreatedBy,
                        UpdatedBy = e.UpdatedBy,
                        CreatedOn = e.CreatedOn,
                        UpdatedOn = e.UpdatedOn,
                    }).ToListAsync();

                return tasks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
