namespace EmployeeSystem.Contract.Dtos
{
    public class TaskReviewDto : BaseEntityDto
    {
        public int ?Id { get; set; }
        public int TaskID { get; set; }
        public string Content { get; set; }
        
        public string ?ReviewedBy { get; set; }
        public int ReviewerId { get; set; }

        public EmployeeDto? ReviewerDetails { get; set; }
    }
}
