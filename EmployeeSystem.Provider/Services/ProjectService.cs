﻿using EmployeeSystem.Contract.Dtos;
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
                var user = await _context.Employees.FirstAsync(e => e.Id == id);
                if(user.Role != Role.SuperAdmin)
                {
                    var query = _context.ProjectEmployees.Include(p => p.Employee).Include(p => p.Project).Where(p => p.Project.IsActive && (p.EmployeeId == id || p.Employee.ManagerID == id)).Distinct();

                    var res = await query
                        .Select(e => new ProjectDto
                        {
                            Id = e.ProjectId,
                            Name = e.Project.Name,
                            Description = e.Project.Description,
                            CreatedBy = e.Project.CreatedBy,
                            UpdatedBy = e.Project.UpdatedBy,
                            CreatedOn = e.Project.CreatedOn,
                            UpdatedOn = e.Project.UpdatedOn,
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
                        CreatedBy = e.CreatedBy,
                        UpdatedBy = e.UpdatedBy,
                        CreatedOn = e.CreatedOn,
                        UpdatedOn = e.UpdatedOn,
                        Status = e.Status,
                    }).ToListAsync();
                
                return projects;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);    
            }
        }

        public async Task<ProjectDetailsDto?> GetById(int id)
        {
            try
            {
                var project = await _context.Projects
                     .Include(p => p.Tasks)
                     .Include(p => p.ProjectEmployees)
                         .ThenInclude(pe => pe.Employee)
                     .FirstOrDefaultAsync(e => e.Id == id);

                if (project == null)
                {
                    return null;
                }

                var projectEmployees = project.ProjectEmployees.Select(p => new ProjectEmployeeDto
                {
                    EmployeeId = p.EmployeeId,
                    EmployeeeName = p.Employee.Name,
                }).ToList();

                var tasksDto = project.Tasks.Select(task => new TaskBasicDto
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                }).ToList();

                var projectDetails = new ProjectDetailsDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    Members = projectEmployees,
                    Tasks = tasksDto,
                    Status = project.Status,
                    CreatedBy = project.CreatedBy,
                    UpdatedBy = project.UpdatedBy,
                    CreatedOn = project.CreatedOn,
                    UpdatedOn = project.UpdatedOn,
                };

                return projectDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Add(int userId, int adminId, AddProjectDto projectDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if(projectDto.Members.Count <= 2)
                {
                    return 0;
                }

                await transaction.CreateSavepointAsync("Adding New Project");
                var project = new Project
                {
                    Name = projectDto.Name,
                    Description = projectDto.Description,
                    AdminId = adminId,
                    IsActive = true,
                    Status = projectDto.Status,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now
                };

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                foreach (var p in projectDto.Members)
                {
                    var projectEmployee = new ProjectEmployee
                    {
                        EmployeeId = p.EmployeeId,
                        ProjectId = project.Id
                    };
                    _context.ProjectEmployees.Add(projectEmployee);
                    
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return project.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackToSavepointAsync("Adding New Project");
                //Console.WriteLine(ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);

                if(project == null)
                {
                    return false;
                }
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
