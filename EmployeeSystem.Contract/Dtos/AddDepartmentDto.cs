using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Contract.Dtos
{
    public class AddDepartmentDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2)]
        public string Name { get; set; }
    }
}
