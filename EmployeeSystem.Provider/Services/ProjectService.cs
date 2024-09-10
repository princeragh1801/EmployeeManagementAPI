using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.Count;
using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

        public IQueryable<Project> GetProjectInfo(int userId, string role, int employeeId=0)
        {
            var query = _context.Projects.Where(p => p.IsActive);
            if(employeeId == 0)
            {
                employeeId = userId;
            }if (role != "SuperAdmin")
            {
                if(employeeId != userId)
                {
                    var userProjects = _context.ProjectEmployees.Where(pe => pe.EmployeeId == userId).Select(pe => pe.ProjectId).ToList();
                    var employeeProjects = _context.ProjectEmployees.Where(pe => pe.EmployeeId == employeeId).Select(pe => pe.ProjectId).ToList();
                    query = query.Where(p => (p.CreatedBy == userId || userProjects.Contains(p.Id)) & employeeProjects.Contains(p.Id));
                }
                else
                {
                    var userProjects = _context.ProjectEmployees.Where(pe => pe.EmployeeId == userId).Select(pe => pe.ProjectId).ToList();
                    query = query.Where(p => (p.CreatedBy == userId || userProjects.Contains(p.Id)));
                }
                
            }
            else
            {
                if(employeeId != userId)
                {
                    var employeeProjects = _context.ProjectEmployees.Where(pe => pe.EmployeeId == employeeId).Select(pe => pe.ProjectId).ToList();
                    query = query.Where(p => (p.CreatedBy == employeeId || employeeProjects.Contains(p.Id)));
                }
            }

            return query;
        }

        public async Task<PaginatedItemsDto<List<ProjectDto>>> Get(IEnumerable<Claim> claims, PaginatedDto<ProjectStatus?> paginatedDto)
        {
            try
            {
                var userId = Convert.ToInt32(claims.First(e => e.Type == "UserId")?.Value);
                var userRole = claims.First(e => e.Type == "Role")?.Value??"Employee";
                var query = GetProjectInfo(userId, userRole);
                // only selecting which is active
                query = query.Include(p => p.Creator);

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


        public async Task<List<ProjectDto>> GetAll(IEnumerable<Claim> claims)
        {
            try
            {
                var userId = Convert.ToInt32(claims.First(e => e.Type == "UserId")?.Value);
                var userRole = claims.First(e => e.Type == "Role")?.Value ?? "Employee";
                // extracting the user info

                var query = GetProjectInfo(userId, userRole);

                // only fetching project details
                var projects = await query
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

        public async Task<List<EmployeeProjectInfo>> GetProjectsByEmployee(IEnumerable<Claim> claims, int employeeId)
        {
            try
            {
                var userId = Convert.ToInt32(claims.First(e => e.Type == "UserId")?.Value);
                var userRole = claims.First(e => e.Type == "Role")?.Value??"Employee";
                var query = GetProjectInfo(userId, userRole, employeeId);
                

                // only fetching project details
                var projects = await query
                    .Select(p => new ProjectDto
                    {
                        Name = p.Name,
                        Description = p.Description,
                        Status = p.Status,
                        Id = p.Id
                    }).ToListAsync();

                var employeeProjects = new List<EmployeeProjectInfo>();
                foreach(var project in projects)
                {
                    var tasks = await _context.Tasks.Where(t => t.ProjectId == project.Id && t.AssignedTo == userId).CountAsync();
                    var employeeProjectInfo = new EmployeeProjectInfo
                    {
                        Project = project,
                        Tasks = tasks
                    };
                    employeeProjects.Add(employeeProjectInfo);
                }
                return employeeProjects;
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

        public async Task<ProjectDetailsDto?> GetById(IEnumerable<Claim> claims, int id)
        {
            try
            {
                var userId = Convert.ToInt32(claims.First(e => e.Type == "UserId")?.Value);
                var userRole = claims.First(e => e.Type == "Role")?.Value ?? "Employee";

                var query = GetProjectInfo(userId, userRole);
                
                // fetching the project with given id
                var project = await query
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
                var pe = project.ProjectEmployees.Where(e => e.Employee.IsActive);
                var projectEmployees = _mapper.Map<List<ProjectEmployeeDto>>(pe);


                var tasks = project.Tasks.Where(t => t.IsActive);
                var totalTasks = tasks.Count();
                var pendingTasks =tasks.Where(t => t.Status == TasksStatus.Pending).Count();
                var completedTasks = tasks.Where(t => t.Status == TasksStatus.Completed).Count();
                var activeTasks = tasks.Where(t => t.Status == TasksStatus.Active).Count();

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
                }if(project.CreatedBy != adminId && admin.Role != Role.SuperAdmin)
                {
                    return -2;
                }

                project.Name = projectDto.Name;
                project.Description = projectDto.Description;
                project.Status = projectDto.Status;
                project.UpdatedBy = admin.Id;
                project.UpdatedOn = DateTime.Now;

                await _context.SaveChangesAsync();

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
                

                

                return project.Id;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }

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

        public async Task<ProjectCount> GetCounts()
        {
            try
            {
                var query = _context.Projects.Where(p => p.IsActive);
                var totalProject = await query.CountAsync();
                var active = await query.Where(p => p.Status == ProjectStatus.Active).CountAsync();
                var pending = await query.Where(p => p.Status == ProjectStatus.Pending).CountAsync();
                var completed = await query.Where(p => p.Status == ProjectStatus.Completed).CountAsync();
                var count = new ProjectCount
                {
                    Total = totalProject,
                    Active = active,
                    Pending = pending,
                    Completed = completed
                };
                return count;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
