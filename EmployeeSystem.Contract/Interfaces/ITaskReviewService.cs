using EmployeeSystem.Contract.Dtos;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface ITaskReviewService
    {
        public Task<List<TaskReviewDto>?> Get(int taskId);
        public Task<int> Add(int userId, AddTaskReviewDto taskReview);
    }
}
 