using EmployeeSystem.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Contract.Dtos
{
    public class ProjectDto : BaseEntityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public List<AddProjectEmployeeDto> ProjectEmployees { get; set; }

        //public List<TasksDto> Tasks { get; set; }

    }
}
