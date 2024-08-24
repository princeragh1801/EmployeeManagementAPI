using EmployeeSystem.Contract.Dtos.IdAndName;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSystem.Provider.Services
{
    public class ProjectEmployeeService : IProjectEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public ProjectEmployeeService(ApplicationDbContext applicationDbContext) 
        {
            _context = applicationDbContext;
        }

        public async Task<List<EmployeeIdAndName>> GetAll(int projectId)
        {
            try
            {
                var employees = await _context.ProjectEmployees.Include(e => e.Employee).Where(e => e.ProjectId == projectId).Select(pe => new EmployeeIdAndName
                {
                    Id = pe.EmployeeId,
                    Name = pe.Employee.Name,
                }).ToListAsync();
                return employees;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
            }
            catch (Exception ex)
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
    }
}
