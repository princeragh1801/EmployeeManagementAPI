using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Contract.Models
{
    public class TaskLog
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public int TaskId { get; set; }

        [ForeignKey(nameof(TaskId))]
        public Tasks Task {  get; set; }
    }
}
