﻿using System.ComponentModel.DataAnnotations;

namespace EmployeeSystem.Contract.Dtos.Add
{
    public class AddDepartmentDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "Name must not exceed 50 characters")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name can only contain lowercase and uppercase letters and spaces")]
        public required string Name { get; set; }

    }
}
