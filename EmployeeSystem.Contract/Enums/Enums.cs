using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Complete
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
    }
}
