using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSystem.Provider.Services
{
    public class SprintService : ISprintService
    {
        private readonly ApplicationDbContext _context;

        public SprintService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<int> Upsert(int id, AddSprintDto addSprintDto)
        {
            try
            {
                var sprintToUpdate = await _context.Sprints.FirstOrDefaultAsync(s => s.Id == id);
                if (sprintToUpdate == null)
                {
                    var sprintExist = await _context.Sprints.FirstOrDefaultAsync(s => s.Name == addSprintDto.Name & s.projectId == addSprintDto.ProjectId);
                    if (sprintExist != null)
                    {
                        return 0;
                    }
                    var sprint = new Sprint
                    {
                        Name = addSprintDto.Name,
                        StartDate = addSprintDto.StartDate,
                        EndDate = addSprintDto.EndDate,
                        projectId = addSprintDto.ProjectId,
                    };
                    _context.Sprints.Add(sprint);

                    await _context.SaveChangesAsync();
                    return sprint.Id;
                }
                sprintToUpdate.Name = addSprintDto.Name;
                sprintToUpdate.StartDate = addSprintDto.StartDate;
                sprintToUpdate.EndDate = addSprintDto.EndDate;
                sprintToUpdate.projectId = addSprintDto.ProjectId;
                await _context.SaveChangesAsync();
                return sprintToUpdate.Id;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SprintInfo>> GetAll()
        {
            try
            {
                var sprints = await _context.Sprints.Where(s => s.isActive)
                    .OrderByDescending(s => s.Id)
                    .Select(s => new SprintInfo
                    {
                        Id = s.Id,
                        Name = s.Name,
                        StartDate = s.StartDate,
                        EndDate = s.EndDate,
                    }).ToListAsync();
                return sprints;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SprintInfo?> GetById(int id)
        {
            try
            {
                var sprint = await _context.Sprints.FirstOrDefaultAsync(s => s.Id == id & s.isActive);
                if (sprint == null) { return null; }
                return new SprintInfo { Id = sprint.Id, Name = sprint.Name, StartDate = sprint.StartDate, EndDate = sprint.EndDate, };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<SprintInfo>> GetByProjectId(int projectId)
        {
            try
            {
                var sprints = await _context.Sprints
                    .Where(s => s.isActive & s.projectId == projectId)
                    .OrderByDescending(s => s.Id)
                    .Select(s => new SprintInfo
                    {
                        Id = s.Id,
                        Name = s.Name,
                        StartDate = s.StartDate,
                        EndDate = s.EndDate,
                    }).ToListAsync();
                return sprints;
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
                var sprint = await _context.Sprints.FirstOrDefaultAsync(s => s.Id == id);
                if (sprint == null) return false;
                sprint.isActive = false;
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
