using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeSystem.Contract.Dtos.IdAndName;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;

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
                var employees = await _context.ProjectEmployees.Include(e => e.Employee).Where(e => e.ProjectId == projectId).ProjectTo<EmployeeIdAndName>(_mapper.ConfigurationProvider).ToListAsync();
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
