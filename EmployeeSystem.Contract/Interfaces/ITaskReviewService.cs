using EmployeeSystem.Contract.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface ITaskReviewService
    {
        public Task<List<TaskReviewDto>?> Get(int taskId);
        public Task<int> Add(int userId, AddTaskReviewDto taskReview);
    }
}
 