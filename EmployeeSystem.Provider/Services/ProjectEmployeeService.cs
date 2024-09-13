using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeSystem.Contract.Dtos.IdAndName;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeSystem.Provider.Services
{
    public class ProjectEmployeeService : IProjectEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProjectEmployeeService(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _context = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<List<EmployeeIdAndName>> GetAll(int projectId)
        {
            try
            {
                var employees = await _context.ProjectEmployees.Include(e => e.Employee).Where(e => e.ProjectId == projectId & e.Employee.IsActive).ProjectTo<EmployeeIdAndName>(_mapper.ConfigurationProvider).ToListAsync();
                return employees;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddMembers(int projectId, IEnumerable<Claim> claims, List<int> employeesToAdd)
        {
            try
            {
                var userId = Convert.ToInt32(claims.First(e => e.Type == "UserId")?.Value);
                var role = claims.First(e => e.Type == "Role")?.Value;
                var project = await _context.Projects.FirstAsync(p => p.Id == projectId);
                if (project == null)
                {
                    return false;
                }
                if (role == "Admin" && project.CreatedBy != userId)
                {
                    return false;
                }
                var projectEmployees = employeesToAdd.Select(e => new ProjectEmployee
                {
                    EmployeeId = e,
                    ProjectId = projectId,
                });
                _context.ProjectEmployees.AddRange(projectEmployees);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteMembers(int projectId, IEnumerable<Claim> claims, List<int> employeesToAdd)
        {
            try
            {
                var userId = Convert.ToInt32(claims.First(e => e.Type == "UserId")?.Value);
                var role = claims.First(e => e.Type == "Role")?.Value;
                var project = await _context.Projects.FirstAsync(p => p.Id == projectId);
                if (project == null)
                {
                    return false;
                }
                if (role == "Admin" && project.CreatedBy != userId)
                {
                    return false;
                }
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
    }
}
