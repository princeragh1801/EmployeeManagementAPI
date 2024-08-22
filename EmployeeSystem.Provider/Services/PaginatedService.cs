using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Info.PaginationInfo;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
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

        public static IQueryable<T> GetOrdered<T>(IQueryable<T> query, string columnName, bool ascending = true)
        {

            // Find the matching property in a case-insensitive manner
            var property = typeof(T).GetProperties()
                .FirstOrDefault(p => p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));

            if (property == null)
            {
                return query; // Property not found, return the original query
            }

            // Get the actual property name
            var actualPropertyName = property.Name;

            var sortingOrder = ascending ? "ascending" : "descending";
            var orderByString = $"{actualPropertyName} {sortingOrder}";

            return query.OrderBy(orderByString);
        }

        /*public static IQueryable<T> GetOrdered<T>(IQueryable<T> query, string columnName, bool ascending = true)
        {

            var parameter = Expression.Parameter(typeof(T), "x");
            //Console.WriteLine("Parameter : " + parameter);
            
            var property = typeof(T).GetProperty(columnName, BindingFlags.IgnoreCase);
            

            if (property == null)
            {
                return query; // Property not found, return the original query
            }
            //Console.WriteLine("Property : " + property);

            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            //Console.WriteLine("propertyAccess : " + propertyAccess);

            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            //Console.WriteLine("orderByExp : " + orderByExp);

            var orderByMethod = ascending ? "OrderBy" : "OrderByDescending";


            var resultExp = Expression.Call(typeof(Queryable), orderByMethod, new Type[] { typeof(T), property.PropertyType }, query.Expression, Expression.Quote(orderByExp));


            //Console.WriteLine("resultExp : " + resultExp);
            return query.Provider.CreateQuery<T>(resultExp);
        }

        /*public IQueryable<Employee> GetOrdered(IQueryable<Employee> query, string columnName, bool ace = true)
        {
            if (columnName.ToLower() == "name")
            {
                query = ace ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
            else if (columnName.ToLower() == "managername")
            {
                query = query.Where(e => e.ManagerID != null);
                query = ace ? query.OrderBy(x => x.Manager.Name) : query.OrderByDescending(x => x.Manager.Name);
            }
            else if (columnName.ToLower() == "departmentname")
            {
                query = query.Where(e => e.Department.Name != null);

                query = ace ? query.OrderBy(x => x.Department.Name) : query.OrderByDescending(x => x.Department.Name);
            }
            else if (columnName.ToLower() == "salary")
            {
                query = ace ? query.OrderBy(x => x.Salary) : query.OrderByDescending(x => x.Salary);
            }
            else if (columnName.ToLower() == "createdon")
            {
                query = ace ? query.OrderBy(x => x.CreatedOn) : query.OrderByDescending(x => x.CreatedOn);
            }else if (columnName.ToLower() == "updatedon")
            {
                query = query.Where(e => e.UpdatedOn != null);
                query = ace ? query.OrderBy(x => x.UpdatedOn) : query.OrderByDescending(x => x.UpdatedOn);
            }else if(columnName.ToLower() == "role")
            {
                query = ace ? query.OrderBy(x => x.Role) : query.OrderByDescending(x => x.Role);
            }
            else
            {
                query = ace ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
            }
            return query;
        }

        public IQueryable<Department> GetOrdered(IQueryable<Department> query, string columnName, bool ace = true)
        {
            if (columnName.ToLower() == "name")
            {
                query = ace ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }else if (columnName.ToLower() == "createdon")
            {
                query = ace ? query.OrderBy(x => x.CreatedOn) : query.OrderByDescending(x => x.CreatedOn);
            }
            else if (columnName.ToLower() == "updatedon")
            {
                query = query.Where(e => e.UpdatedOn != null);
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
            if (columnName.ToLower() == "name")
            {
                query = ace ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
            else if (columnName.ToLower() == "description")
            {
                query = ace ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description);
            }
            else if (columnName.ToLower() == "createdon")
            {
                query = ace ? query.OrderBy(x => x.CreatedOn) : query.OrderByDescending(x => x.CreatedOn);
            }
            else if (columnName.ToLower() == "updatedon")
            {
                query = query.Where(e => e.UpdatedOn != null);
                query = ace ? query.OrderBy(x => x.UpdatedOn) : query.OrderByDescending(x => x.UpdatedOn);
            }
            else
            {
                query = ace ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
            }
            return query;
        }*/

        public IQueryable<Tasks> GetOrdered(IQueryable<Tasks> query, string columnName, bool ace = true)
        {
            if (columnName.ToLower() == "name")
            {
                query = ace ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
            else if (columnName.ToLower() == "description")
            {
                query = ace ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description);
            }
            else if (columnName.ToLower() == "createdon")
            {
                query = ace ? query.OrderBy(x => x.CreatedOn) : query.OrderByDescending(x => x.CreatedOn);
            }
            else if (columnName.ToLower() == "updatedon")
            {
                query = query.Where(e => e.UpdatedOn != null);
                query = ace ? query.OrderBy(x => x.UpdatedOn) : query.OrderByDescending(x => x.UpdatedOn);
            }
            else
            {
                query = ace ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
            }
            return query;
        }

        public IQueryable<ProjectEmployee> GetOrdered(IQueryable<ProjectEmployee> query, string columnName, bool ace = true)
        {
            if (columnName.ToLower() == "name")
            {
                query = ace ? query.OrderBy(x => x.Project.Name) : query.OrderByDescending(x => x.Project.Name);
            }
            else if (columnName.ToLower() == "description")
            {
                query = ace ? query.OrderBy(x => x.Project.Description) : query.OrderByDescending(x => x.Project.Description);
            }
            else if (columnName.ToLower() == "createdon")
            {
                query = ace ? query.OrderBy(x => x.Project.CreatedOn) : query.OrderByDescending(x => x.Project.CreatedOn);
            }
            else if (columnName.ToLower() == "updatedon")
            {
                query = query.Where(e => e.Project.UpdatedOn != null);
                query = ace ? query.OrderBy(x => x.Project.UpdatedOn) : query.OrderByDescending(x => x.Project.UpdatedOn);
            }
            else
            {
                query = ace ? query.OrderBy(x => x.Project.Id) : query.OrderByDescending(x => x.Project.Id);
            }
            return query;
        }

        public async Task<PaginatedItemsDto<List<EmployeePaginationInfo>>> GetEmployees(int userId, PaginatedDto paginatedDto)
        {
            try
            {
                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;

                var user = await _context.Employees.FirstAsync(e => e.Id == userId);

                // including the details of the employee in query
                var query = _context.Employees
                    .Include(e => e.Manager)
                    .Include(m => m.Department)
                    .Where(e => e.IsActive);

                if(user.Role != Role.SuperAdmin)
                {
                    query = query.Where(e => e.ManagerID != null && e.ManagerID == userId );
                }

                // applying search filter on that
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(e => e.Name.Contains(search) || e.Department.Name.Contains(search) || e.Manager.Name.Contains(search) || e.Phone.Contains(search));
                }

                query = orderBy == SortedOrder.NoOrder  ? query : GetOrdered<Employee>(query, orderKey, orderBy == SortedOrder.Ascending ? true : false);

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
                    .Select(e => new EmployeePaginationInfo
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Salary = e.Salary,
                        Role = e.Role,
                        ManagerName = e.Manager.Name,
                        DepartmentName = e.Department.Name,
                        Email = e.Email,
                        CreatedOn = e.CreatedOn,
                    }).ToListAsync();

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

        public async Task<PaginatedItemsDto<List<DepartmentPaginationInfo>>> GetDepartments(PaginatedDto paginatedDto)
        {
            try
            {
                // only selecting which is active
                var query = _context.Departments.Include(d => d.Creator).Where(d => d.IsActive);

                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;

                // applying search filter on that
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d => d.Name.Contains(search));
                }

                // now getting the order wise details
                query = orderBy == SortedOrder.NoOrder ? query : GetOrdered<Department>(query, orderKey, orderBy == SortedOrder.Ascending ? true : false);

                // calculating the total count and pages
                var totalCount = query.Count();
                var totalPages = totalCount / paginatedDto.PagedItemsCount;
                if (totalCount % paginatedDto.PagedItemsCount != 0)
                {
                    totalPages++;
                }

                // now extrating departments of the page-[x]
                var departments = await query
                    .Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                    .Take(paginatedDto.PagedItemsCount)
                    .Select(
                    d => new DepartmentPaginationInfo
                    {
                        Id = d.Id,
                        Name = d.Name,
                        CreatedOn = d.CreatedOn,
                        CreatedBy = d.Creator.Name,
                    }).ToListAsync();

                // creating new dto to send the info
                PaginatedItemsDto<List<DepartmentPaginationInfo>> res = new PaginatedItemsDto<List<DepartmentPaginationInfo>>();

                res.Data =departments;
                res.TotalPages = totalPages;
                res.TotalItems = totalCount;
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginatedItemsDto<List<ProjectDto>>> GetProjects(int userId, PaginatedDto paginatedDto)
        {
            try
            {
                
                // only selecting which is active
                var query = _context.Projects.Include(p => p.Creator).Where(d => d.IsActive);

                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;

                var filters = paginatedDto.Filters;
                if (filters != null)
                {
                    foreach (var filter in filters)
                    {
                        var key = filter.Item1.ToLower();
                        var value = filter.Item2;
                        if (key == "status")
                        {
                            var status = ProjectStatus.Pending;
                            if (value == 1)
                            {
                                status = ProjectStatus.Active;
                            }
                            if (value == 2)
                            {
                                status = ProjectStatus.Completed;
                            }
                            query = query.Where(t => t.Status == status);
                        }
                    }
                }

                var user = await _context.Employees.FirstAsync(e => e.Id == userId);

                // role specific
                if (user.Role != Role.SuperAdmin)
                {
                    var userQuery = _context.ProjectEmployees
                        .Include(p => p.Employee).Include(p => p.Project)
                        .Where(p => p.Project.IsActive && (p.EmployeeId == userId || p.Employee.ManagerID == userId))
                        .Distinct();

                    if (!string.IsNullOrEmpty(search))
                    {
                        userQuery = userQuery.Where(pe => pe.Project.Name.Contains(search));
                    }

                    userQuery = orderBy == SortedOrder.NoOrder ? userQuery : GetOrdered<ProjectEmployee>(userQuery, orderKey, (orderBy == SortedOrder.Ascending) ? true : false);

                    // calculating the total count and pages
                    var totalCnt = userQuery.Count();
                    var totalPage = totalCnt / paginatedDto.PagedItemsCount;
                    if (totalCnt % paginatedDto.PagedItemsCount != 0)
                    {
                        totalPage++;
                    }

                    // now extrating projects of the page-[x]
                    var userProjects = await userQuery
                        .Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                        .Take(paginatedDto.PagedItemsCount)
                        .Select(e => new ProjectDto
                        {
                            Id = e.Project.Id,
                            Name = e.Project.Name,
                            Description = e.Project.Description,
                            CreatedBy = e.Project.Creator.Name,
                            CreatedOn = e.Project.CreatedOn,
                        }).ToListAsync();

                    // creating new dto to send the info
                    PaginatedItemsDto<List<ProjectDto>> resp = new PaginatedItemsDto<List<ProjectDto>>();

                    resp.Data = userProjects;
                    resp.TotalPages = totalPage;
                    resp.TotalItems = totalCnt;
                    return resp;
                }
                // applying search filter on that
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d => d.Name.Contains(search));
                }

                // now getting the order wise details
                query = orderBy == SortedOrder.NoOrder ? query : GetOrdered<Project>(query, orderKey, orderBy == SortedOrder.Ascending ? true : false);

                // calculating the total count and pages
                var totalCount = query.Count();
                var totalPages = totalCount / paginatedDto.PagedItemsCount;
                if (totalCount % paginatedDto.PagedItemsCount != 0)
                {
                    totalPages++;
                }

                // now extrating projects of the page-[x]
                var projects = await query
                    .Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                    .Take(paginatedDto.PagedItemsCount)
                    .Select(e => new ProjectDto
                     {
                         Id = e.Id,
                         Name = e.Name,
                         Description = e.Description,
                         CreatedBy = e.Creator.Name,
                         CreatedOn = e.CreatedOn,
                     }).ToListAsync();

                // creating new dto to send the info
                PaginatedItemsDto<List<ProjectDto>> res = new PaginatedItemsDto<List<ProjectDto>>();

                res.Data = projects;
                res.TotalPages = totalPages;
                res.TotalItems = totalCount;
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginatedItemsDto<List<TasksDto>>> GetTasks(int userId, PaginatedDto paginatedDto)
        {
            try
            {
                // only selecting which is active
                var query = _context.Tasks.Where(d => d.IsActive);
                var user = await _context.Employees.FirstAsync(e => e.Id == userId);
                if (user.Role != Role.SuperAdmin)
                {
                    query = query.Include(t => t.Employee).Where(t => t.Employee.ManagerID == userId || t.AssignedTo == userId);
                }
                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;
                var filters = paginatedDto.Filters;
                if (filters != null)
                {
                    foreach (var filter in filters)
                    {
                        var key = filter.Item1.ToLower();
                        var value = filter.Item2;
                        if (key == "Type")
                        {
                            var type = TaskType.Epic;
                            if (value == 1)
                            {
                                type = TaskType.Feature;
                            }
                            if (value == 2)
                            {
                                type = TaskType.Userstory;
                            }
                            if (value == 3)
                            {
                                type = TaskType.Task;
                            }
                            if (value == 4)
                            {
                                type = TaskType.Bug;
                            }

                            query = query.Where(t => t.TaskType == type);

                        }
                        else if (key == "status")
                        {
                            var status = TasksStatus.Pending;
                            if (value == 1)
                            {
                                status = TasksStatus.Active;
                            }
                            if (value == 2)
                            {
                                status = TasksStatus.Completed;
                            }
                            query = query.Where(t => t.Status == status);
                        }
                    }
                }

                // applying search filter on that
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d => d.Name.Contains(search));
                }

                // now getting the order wise details
                query = orderBy == SortedOrder.NoOrder ? query : GetOrdered(query, orderKey, orderBy == SortedOrder.Ascending ? true : false);

                // calculating the total count and pages
                var totalCount = query.Count();
                var totalPages = totalCount / paginatedDto.PagedItemsCount;
                if (totalCount % paginatedDto.PagedItemsCount != 0)
                {
                    totalPages++;
                }

                // now extrating projects of the page-[x]
                var tasks = await query
                    .Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                    .Take(paginatedDto.PagedItemsCount)
                    .Select(e => new TasksDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Description = e.Description,
                        CreatedOn = e.CreatedOn,
                        ProjectId = e.ProjectId,
                        AssigneeName = e.Employee.Name,
                        AssignerName = e.Creator.Name,
                        Status = e.Status,
                        
                    }).ToListAsync();

                // creating new dto to send the info
                PaginatedItemsDto<List<TasksDto>> res = new PaginatedItemsDto<List<TasksDto>>();
                res.Data = tasks;
                res.TotalPages = totalPages;
                res.TotalItems = totalCount;
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginatedItemsDto<List<TasksDto>>> GetProjectTasks(int userId, int projectId, PaginatedDto paginatedDto)
        {
            try
            {
                var tasksList = _context.Tasks.Where(t => t.ProjectId == projectId & t.IsActive);
                
                var filters = paginatedDto.Filters;
                if (filters != null)
                {
                    foreach (var filter in filters)
                    {
                        var key = filter.Item1.ToLower();
                        var value = filter.Item2;
                        if (key == "type")
                        {
                            if(value <= 4)
                            {
                                var type = TaskType.Epic;

                                if (value == 1)
                                {
                                    type = TaskType.Feature;
                                }
                                else if (value == 2)
                                {
                                    type = TaskType.Userstory;
                                }
                                if (value == 3)
                                {
                                    type = TaskType.Task;
                                }
                                if (value == 4)
                                {
                                    type = TaskType.Bug;
                                }

                                tasksList = tasksList.Where(t => t.TaskType == type);

                            }

                        }
                        else if (key == "status")
                        {
                            if(value <= 2)
                            {
                                var status = TasksStatus.Pending;
                                if (value == 1)
                                {
                                    status = TasksStatus.Active;
                                }
                                else if (value == 2)
                                {
                                    status = TasksStatus.Completed;
                                }
                                tasksList = tasksList.Where(t => t.Status == status);
                            }
                        }
                        else if(key == "sprint")
                        {
                            tasksList = tasksList.Where(s => s.SprintId == value);
                        }
                    }
                }
                else
                {
                    var epics = tasksList.Where(t => t.ProjectId == projectId & t.TaskType == TaskType.Epic);
                    var features = tasksList.Where(t => t.ProjectId == projectId & t.TaskType == TaskType.Feature & t.ParentId == null);
                    var userStories = tasksList.Where(t => t.TaskType == TaskType.Userstory & t.ParentId == null);
                    var tasks = tasksList.Where(t => t.ProjectId == projectId & t.TaskType == TaskType.Task & t.ParentId == null);
                    var bugs = tasksList.Where(t => t.IsActive & t.ProjectId == projectId & t.TaskType == TaskType.Bug & t.ParentId == null);
                    epics.Concat(features).Concat(userStories).Concat(tasks).Concat(bugs);
                    tasksList = epics;
                }

                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;

                if (!string.IsNullOrEmpty(search))
                {
                    tasksList = tasksList.Where(t => t.Name.Contains(search));
                }

                tasksList = orderBy == SortedOrder.NoOrder ? tasksList : GetOrdered(tasksList, orderKey, orderBy == SortedOrder.Ascending ? true : false);

                // calculating the total count and pages
                var totalCount = tasksList.Count();
                var totalPages = totalCount / paginatedDto.PagedItemsCount;
                if (totalCount % paginatedDto.PagedItemsCount != 0)
                {
                    totalPages++;
                }

                // now extrating projects of the page-[x]
                var tasksData = await tasksList
                    .Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                    .Take(paginatedDto.PagedItemsCount)
                    .Select(e => new TasksDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        //Description = e.Description,
                        CreatedOn = e.CreatedOn,
                        //ProjectId = e.ProjectId,
                        TaskType = e.TaskType,
                        AssigneeName = e.Employee.Name,
                        //AssignerName = e.Admin.Name,
                        Status = e.Status,

                    }).ToListAsync();

                // creating new dto to send the info
                PaginatedItemsDto<List<TasksDto>> res = new PaginatedItemsDto<List<TasksDto>>();
                res.Data = tasksData;
                res.TotalPages = totalPages;
                res.TotalItems = totalCount;
                return res;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
