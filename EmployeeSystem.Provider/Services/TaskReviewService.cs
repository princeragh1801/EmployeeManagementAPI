using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSystem.Provider.Services
{
    public class TaskReviewService : ITaskReviewService
    {
        private readonly ApplicationDbContext _context;

        public TaskReviewService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        
        public async Task<List<TaskReviewDto>?> Get(int taskId)
        {
            try
            {
                // fetching task details
                var task = await _context.Tasks
                    .FirstOrDefaultAsync(t => t.Id == taskId);

                // task doesn't exist
                if(task == null)
                {
                    return null;
                }

                // fetching the reviews and converting to taskreview dto list
                var reviews = await _context.TaskReviews
                    .Include(tr => tr.Reviewer)
                    .Where(tr => tr.TaskID == taskId)
                    .OrderByDescending(tr => tr.Id)
                    .Select(tr => new TaskReviewDto
                    {
                        Id = tr.Id,
                        ReviewedBy = tr.Reviewer.Name,
                        Content = tr.Content,
                        CreatedOn = tr.CreatedOn,
                        ReviewerAvatarUrl = tr.Reviewer.ImageUrl,

                    }).ToListAsync();

                return reviews;
                


            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<int> Add(int userId, int adminId, AddTaskReviewDto taskReviewDto)
        {
            try
            {
                // creating new task review model
                var taskReview = new TaskReview
                {
                    Content = taskReviewDto.Content,
                    ReviewedBy = adminId,
                    TaskID = taskReviewDto.TaskID,
                    CreatedOn = DateTime.Now,
                    CreatedBy = userId
                };

                // adding and updating the database info
                _context.TaskReviews.Add(taskReview);
                await _context.SaveChangesAsync();
                return taskReview.TaskID;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
