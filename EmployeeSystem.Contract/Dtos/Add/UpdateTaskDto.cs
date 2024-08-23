﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos.Add
{
    public class UpdateTaskDto
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? AssignedTo { get; set; }

        public TaskType? TaskType { get; set; }

        public int? ParentId { get; set; }

        public int? SprintId { get; set; } = null;
        [Required]
        public TasksStatus? Status { get; set; } = TasksStatus.Pending;
    }
}
