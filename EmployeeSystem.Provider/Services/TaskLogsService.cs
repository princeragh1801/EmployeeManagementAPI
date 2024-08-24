using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSystem.Provider.Services
{
    public class TaskLogsService : ITaskLogService
    {
        private readonly ApplicationDbContext _context;
        public TaskLogsService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<List<LogDto>> GetLogs(int taskId)
        {
            try
            {
                var logs = await _context.TaskLogs
                    .Where(t => t.TaskId == taskId)
                    .Select(t => new LogDto
                    {
                        Message = t.Message,
                    }).ToListAsync();

                return logs;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task Add(TaskLog log)
        {
            try
            {
                _context.TaskLogs.Add(log);
                Console.WriteLine("Log id : " + log.Id);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
