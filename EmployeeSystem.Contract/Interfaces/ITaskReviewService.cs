using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface ITaskReviewService
    {
        /// <summary>
        /// Retrieves the list of reviews for the task with the specified ID.
        /// </summary>
        /// <param name="taskId">The unique identifier of the task for which reviews are being retrieved.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of TaskReviewDto with the reviews for the specified task, or null if no reviews are found.
        /// </returns>
        public Task<List<TaskReviewDto>?> Get(int taskId);


        /// <summary>
        /// Adds a new review for the specified task and returns the ID of the created review.
        /// </summary>
        /// <param name="userId">The unique identifier of the user adding the review.</param>
        /// <param name="adminId">The unique identifier of the admin authorizing the review addition.</param>
        /// <param name="taskReview">An object containing the details of the task review to be added.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the ID of the newly created task review.
        /// </returns>
        public Task<int> Add(int taskId, int adminId, AddTaskReviewDto taskReview);

        /// <summary>
        /// Updates the review of a task based on the provided task ID, admin ID, and review details.
        /// </summary>
        /// <param name="id">The unique identifier of the task to be reviewed.</param>
        /// <param name="adminId">The unique identifier of the admin performing the review.</param>
        /// <param name="taskReviewDto">An <see cref="AddTaskReviewDto"/> object containing the details of the review to be updated.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a nullable boolean value indicating whether the update was successful (<c>true</c>), failed (<c>false</c>), or if the result is unknown (<c>null</c>).
        /// </returns>
        public Task<bool?> UpdateReview(int id, int adminId, AddTaskReviewDto taskReviewDto);
    }
}
 