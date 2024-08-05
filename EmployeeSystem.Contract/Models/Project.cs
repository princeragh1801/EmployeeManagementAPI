﻿using System.ComponentModel.DataAnnotations.Schema;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Models
{
    public class Project : BaseEntity
    {
        public int Id { get; set; }
        
        public int AdminId { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public ProjectStatus Status { get; set; } = ProjectStatus.Pending;

        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(AdminId))]
        public Employee Admin { get; set; }

        public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();

        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
