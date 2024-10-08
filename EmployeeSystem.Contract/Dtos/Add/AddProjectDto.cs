﻿using System.ComponentModel.DataAnnotations;
using EmployeeSystem.Contract.Enums;

namespace EmployeeSystem.Contract.Dtos.Add
{
    public class AddProjectDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "Name must not exceed 50 characters")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name can only contain lowercase and uppercase letters and spaces")]
        public required string Name { get; set; }


        [Required(ErrorMessage = "Description is required")]
        [MinLength(10, ErrorMessage = "Name must be at least 10 characters long")]
        public required string Description { get; set; }

        public ProjectStatus Status { get; set; } = ProjectStatus.Pending;
        /*[Required]
        [Range(1, int.MaxValue)]
        public int AdminId { get; set; }*/

        public List<AddProjectEmployeeDto>? Members { get; set; }
    }
}
