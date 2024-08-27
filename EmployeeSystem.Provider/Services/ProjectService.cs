using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Provider.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtilityService _utilityService;
        private readonly IMapper _mapper;
        public ProjectService(ApplicationDbContext applicationDbContext, IUtilityService utilityService, IMapper mapper)
        {
            _context = applicationDbContext;
            _utilityService = utilityService;
            _mapper = mapper;
        }

        public async Task<PaginatedItemsDto<List<ProjectDto>>> Get(int userId, PaginatedDto<ProjectStatus?> paginatedDto)
        {
            try
            {

                // only selecting which is active
                var query = _context.Projects.Include(p => p.Creator).Where(d => d.IsActive);

                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;
                var range = paginatedDto.DateRange;

                // range filter
                if (range != null)
                {
                    var startDate = range.StartDate;
                    var endDate = range.EndDate;

                    query = query.Where(t => t.CreatedOn >= startDate && t.CreatedOn <= endDate);
                }

                var status = paginatedDto.Status;
                if (status != null)
                {
                    query = query.Where(t => t.Status == status);
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

                    userQuery = orderBy == SortedOrder.NoOrder ? userQuery : _utilityService.GetOrdered<ProjectEmployee>(userQuery, orderKey, (orderBy == SortedOrder.Ascending) ? true : false);

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
                query = orderBy == SortedOrder.NoOrder ? query : _utilityService.GetOrdered<Project>(query, orderKey, orderBy == SortedOrder.Ascending ? true : false);

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
                    .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider).ToListAsync();

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


        public async Task<List<ProjectDto>> GetAll(int id)
        {
            try
            {
                // extracting the user info
                var user = await _context.Employees.FirstAsync(e => e.Id == id);

                // checking the role
                if(user.Role != Role.SuperAdmin)
                {
                    // creating the query if user is not super admin
                    var query = _context.ProjectEmployees
                        .Include(p => p.Employee).Include(p => p.Project)
                        .ThenInclude(p => p.Creator)
                        .Where(p => p.Project.IsActive && (p.EmployeeId == id || p.Employee.ManagerID == id))
                        .Distinct();

                    // fetching project based on the query
                    var res = await query
                        .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
                        .Distinct().ToListAsync();
                    return res;
                }
                
                // only fetching project details
                var projects = await _context.Projects
                    .Include(p => p.Creator)
                    .Where(p => p.IsActive)
                    .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                
                return projects;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);    
            }
        }

        public async Task<List<ProjectDto>> GetProjectsByEmployee(int employeeId)
        {
            try
            {
                var query = _context.ProjectEmployees
                    .Include(p => p.Employee)
                    .Include(p => p.Project)
                    .ThenInclude(p => p.Creator)
                    .Where(p => p.EmployeeId == employeeId)
                    .Distinct();



                // only fetching project details
                var projects = await query
                    .Where(p => p.Project.IsActive)
                    .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider).ToListAsync();

                return projects;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProjectDto>> GetProjectsStatusWise(ProjectStatus status)
        {
            try
            {
                var projects = await _context.Projects
                    .Where(p => p.IsActive && p.Status == status)
                    .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider).ToListAsync();

                return projects;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProjectDetailsDto?> GetById(int id)
        {
            try
            {
                // fetching the project with given id
                var project = await _context.Projects
                    .Include(p => p.Creator)
                     .Include(p => p.Tasks)
                     .Include(p => p.ProjectEmployees)
                         .ThenInclude(pe => pe.Employee)
                     .FirstOrDefaultAsync(e => e.Id == id & e.IsActive);

                if (project == null)
                {
                    return null;
                }

                // fetching the employees of the project
                var projectEmployees = _mapper.Map<List<ProjectEmployeeDto>>(project.ProjectEmployees);


                var tasks = project.Tasks.Where(t => t.IsActive);
                var totalTasks = tasks.Count();
                var pendingTasks =tasks.Where(t => t.Status == TasksStatus.Pending).Count();
                var completedTasks = tasks.Where(t => t.Status == TasksStatus.Completed).Count();
                var activeTasks = tasks.Where(t => t.Status == TasksStatus.Active).Count();

                /*// fetching the tasks of the project
                var tasksDto = tasks
                    .Select(task => new TaskBasicDto
                    {
                        Id = task.Id,
                        Name = task.Name,
                        Description = task.Description,
                        Status = task.Status,
                        TaskType = task.TaskType,
                    }).ToList();*/

                


                // assemble all the details in project details dto to send
                var projectDetails = new ProjectDetailsDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    Members = projectEmployees,
                    Status = project.Status,
                    CreatedBy = project.Creator.Name,
                    CreatedOn = project.CreatedOn,
                    ActiveTasks = activeTasks,
                    CompletedTasks = completedTasks,
                    PendingTasks = pendingTasks,
                    TotalTasks = totalTasks,
                };

                return projectDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Add(int adminId, AddProjectDto projectDto)
        {
            try
            {

                var admin = await _context.Employees.FirstAsync(e => e.Id == adminId);
                // creating a new project model
                var project = _mapper.Map<Project>(projectDto);
                project.CreatedBy = adminId;

                // added the new project in db
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                if(projectDto.Members != null && projectDto.Members.Any())
                {
                    var projectEmployeesToAdd = projectDto.Members.Select(pe => new ProjectEmployee
                    {
                        EmployeeId = pe.EmployeeId,
                        ProjectId = project.Id
                    });

                    _context.ProjectEmployees.AddRange(projectEmployeesToAdd);
                    await _context.SaveChangesAsync();
                }

                return project.Id;
            }
            catch (Exception ex)
            {
                
                //Console.WriteLine(ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> Update(int id, int adminId, AddProjectDto projectDto)
        {
            try
            {
                var admin = await _context.Employees.FirstAsync(e => e.Id == adminId);
                // creating a new project model
                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id & p.IsActive);
                if(project == null)
                {
                    return -1;
                }

                project.Name = projectDto.Name;
                project.Description = projectDto.Description;
                project.Status = projectDto.Status;
                project.UpdatedBy = admin.Id;
                project.UpdatedOn = DateTime.Now;

                await _context.SaveChangesAsync();

                if(projectDto.Members != null && projectDto.Members.Any())
                {
                    var existingEmp = _context.ProjectEmployees
                        .Where(pe => pe.ProjectId == id)
                        .Select(pe => pe.EmployeeId)
                        .ToList();

                    var empToRemove = existingEmp.Except(projectDto.Members.Select(m => m.EmployeeId)).ToList();
                    var empToAdd = projectDto.Members.Select(e => e.EmployeeId).Except(existingEmp).ToList();
                    
                    if(empToRemove.Any()) _context.ProjectEmployees.RemoveRange(_context.ProjectEmployees.Where(pe=> pe.ProjectId == id && empToRemove.Contains(pe.EmployeeId)));

                    if (empToAdd.Any())
                    {
                        var projectEmployeesToAdd = empToAdd.Select(e => new ProjectEmployee
                        {
                            EmployeeId = e,
                            ProjectId = id,
                        });
                        _context.ProjectEmployees.AddRange(projectEmployeesToAdd);
                    }

                    await _context.SaveChangesAsync();
                }

                

                return project.Id;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }

        /*public async Task<bool> AddMembers(int projectId, List<int> employeesToAdd)
        {
            try
            {
                var projectEmployees = employeesToAdd.Select(e => new ProjectEmployee
                {
                    EmployeeId = e,
                    ProjectId = projectId,
                });
                _context.ProjectEmployees.AddRange(projectEmployees);
                await _context.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteMembers(int projectId, List<int> employeesToAdd)
        {
            try
            {
                var projectEmployees = employeesToAdd.Select(e => new ProjectEmployee
                {
                    EmployeeId = e,
                    ProjectId = projectId,
                });
                _context.ProjectEmployees.RemoveRange(projectEmployees);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
*/
        
        public async Task<bool> DeleteById(int id)
        {
            try
            {
                // fetching the project details
                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);

                if(project == null)
                {
                    return false;
                }

                // soft delete
                project.IsActive = false;

                //_context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*public async Task<List<EmployeeIdAndName>> GetProjectEmployees(int projectId)
        {
            try
            {
                // fetching the project details
                var projectEmployees = await _context.ProjectEmployees
                    .Include(pe => pe.Employee)
                    .Where(pe => pe.ProjectId == projectId)
                    .Select(pe => new EmployeeIdAndName
                    {
                        Id = pe.Employee.Id,
                        Name = pe.Employee.Name,
                        DepartmentName = pe.Employee.Department.Name
                    }).ToListAsync();
                return projectEmployees;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    */
    }
}
