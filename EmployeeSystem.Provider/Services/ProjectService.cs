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

        public ProjectService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
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
                        .Where(p => p.Project.IsActive && (p.EmployeeId == id || p.Employee.ManagerID == id))
                        .Distinct();

                    // fetching project based on the query
                    var res = await query
                        .Select(e => new ProjectDto
                        {
                            Id = e.ProjectId,
                            Name = e.Project.Name,
                            Description = e.Project.Description,
                            CreatedOn = e.Project.CreatedOn,
                            CreatedBy = e.Project.CreatedByName,
                            Status = e.Project.Status,
                        }).Distinct().ToListAsync();
                    return res;
                }
                
                // only fetching project details
                var projects = await _context.Projects
                    .Where(p => p.IsActive)
                    .Select(e => new ProjectDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Description = e.Description,
                        CreatedOn = e.CreatedOn,
                        CreatedBy = e.CreatedByName,
                        Status = e.Status,
                    }).ToListAsync();
                
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
                    .Where(p => p.EmployeeId == employeeId)
                    .Distinct();



                // only fetching project details
                var projects = await query
                    .Where(p => p.Project.IsActive)
                    .Select(e => new ProjectDto
                    {
                        Id = e.Project.Id,
                        Name = e.Project.Name,
                        Description = e.Project.Description,
                        CreatedBy = e.Project.CreatedByName,
                        CreatedOn = e.Project.CreatedOn,
                        Status = e.Project.Status,
                    }).ToListAsync();

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
                    .Select(e => new ProjectDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Description = e.Description,
                        CreatedOn = e.CreatedOn,
                        CreatedBy = e.CreatedByName,
                        Status = e.Status,
                    }).ToListAsync();

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
                     .Include(p => p.Tasks)
                     .Include(p => p.ProjectEmployees)
                         .ThenInclude(pe => pe.Employee)
                     .FirstOrDefaultAsync(e => e.Id == id);

                if (project == null)
                {
                    return null;
                }

                // fetching the employees of the project
                var projectEmployees = project.ProjectEmployees
                    .Select(p => new ProjectEmployeeDto
                    {
                        EmployeeId = p.EmployeeId,
                        EmployeeName = p.Employee.Name,
                    }).ToList();

                // fetching the tasks of the project
                var tasksDto = project.Tasks.Select(task => new TaskBasicDto
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    Status = task.Status,
                }).ToList();

                // assemble all the details in project details dto to send
                var projectDetails = new ProjectDetailsDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    Members = projectEmployees,
                    Tasks = tasksDto,
                    Status = project.Status,
                    CreatedBy = project.CreatedByName,
                    CreatedOn = project.CreatedOn,
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
                var project = new Project
                {
                    Name = projectDto.Name,
                    Description = projectDto.Description,
                    AdminId = adminId,
                    IsActive = true,
                    Status = projectDto.Status,
                    CreatedBy = adminId,
                    CreatedByName = admin.Name,
                    CreatedOn = DateTime.Now
                };

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
            using var transaction = await _context.Database.BeginTransactionAsync();
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
                project.UpdatedByName = admin.Name;

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

        public async Task<bool> AddMembers(int projectId, List<int> employeesToAdd)
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
    }
}
