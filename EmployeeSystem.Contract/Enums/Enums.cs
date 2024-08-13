namespace EmployeeSystem.Contract.Enums
{
    public class Enums
    {
        public enum Role
        {
            Employee,
            Admin,
            SuperAdmin
        }

        public enum TasksStatus
        {
            Pending,
            Active,
            Completed
        }

        public enum AttendanceStatus
        {
            Present,
            Absent,
            OnLeave
        }

        public enum SalaryStatus
        {
            Paid,
            AdvancePaid
        }

        public enum SortedOrder
        {
            Descending,
            Ascending,
            NoOrder
        }

        public enum ProjectStatus
        {
            Pending,
            Active,
            Completed
        }

        public enum RequestType
        {
            AdvanceSalary,

        }
        public enum RequestStatus
        {
            Requested,
            Approved,
            Rejected
        }

        public enum TaskType
        {
            Epic,
            Feature,
            Userstory,
            Task,
            Bug
        }
    }
}
