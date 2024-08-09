namespace EmployeeSystem.Contract.Dtos
{
    public class TaskReviewDto
    {
        public int ?Id { get; set; }
        public string Content { get; set; }
        
        public string ?ReviewedBy { get; set; }
        public string ReviewerAvatarUrl { get; set; }
        public DateTime CreatedOn { get; set; }

        //public EmployeeDto? ReviewerDetails { get; set; }
    }
}
