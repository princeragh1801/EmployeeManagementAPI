using EmployeeSystem.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Contract.Dtos
{
    public class ProjectDetailsDto : BaseEntityDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<TaskBasicDto> Tasks { get; set; }

        public List<ProjectEmployeeDto> Members { get; set; }
    }
}
