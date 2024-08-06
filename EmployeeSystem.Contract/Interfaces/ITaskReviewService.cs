using EmployeeSystem.Contract.Dtos;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface ITaskReviewService
    {
        /// <summary>
        /// This is the get request it returns the list of reviews of the task with the given id 
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>List</returns>
        public Task<List<TaskReviewDto>?> Get(int taskId);

        /// <summary>
        /// This is the post request it basically adds new review of the task and it returns the id of the created review
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="adminId"></param>
        /// <param name="taskReview"></param>
        /// <returns>Integer</returns>
        public Task<int> Add(int userId, int adminId, AddTaskReviewDto taskReview);
    }
}
 