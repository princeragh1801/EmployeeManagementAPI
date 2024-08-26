using EmployeeSystem.Contract.Dtos.Add;
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
                    .OrderByDescending(t => t.Id)
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

        public async Task Add(AddTaskLogDto log)
        {
            try
            {
                var logToAdd = new TaskLog
                {
                    Message = log.Message,
                    TaskId = log.TaskId,
                };
                _context.TaskLogs.Add(logToAdd);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddMany(List<AddTaskLogDto> logs)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await transaction.CreateSavepointAsync("Adding multiple logs");
                foreach(var log in logs)
                {
                    var logToAdd = new TaskLog
                    {
                        Message = log.Message,
                        TaskId = log.TaskId,
                    };
                    _context.TaskLogs.Add(logToAdd);
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackToSavepointAsync("Adding multiple logs");
                throw new Exception(ex.Message);
            }
        }
    }
}
