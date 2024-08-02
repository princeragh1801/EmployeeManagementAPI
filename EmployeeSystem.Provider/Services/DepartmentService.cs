using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using EmployeeSystem.Contract.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace EmployeeSystem.Provider.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<ApiResponse<List<DepartmentDto>>> GetAll()
        {
            try
            {
                var response = new ApiResponse<List<DepartmentDto>>();
                // fetching departments and converting them into list of department dto
                var departments = await _context.Departments
                    .Where(d => d.IsActive)
                    .Select(
                    d => new DepartmentDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        CreatedOn = d.CreatedOn,
                        UpdatedOn = d.UpdatedOn,
                        CreatedBy = d.CreatedBy,
                        UpdatedBy = d.UpdatedBy,
                    }).ToListAsync();

                response.Data = departments;
                response.Message = "Departments fetched";

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<ApiResponse<DepartmentDto>> GetById(int id)
        {
            try
            {
                var response = new ApiResponse<DepartmentDto>();
                // fetching department details
                var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
                
                if(department  == null)
                {
                    response.Status = 404;
                    return response;
                }

                var departmentDto = new DepartmentDto
                {
                    Id = department.Id,
                    Name = department.Name,
                    CreatedOn = department.CreatedOn,
                    UpdatedOn = department.UpdatedOn,
                    CreatedBy = department.CreatedBy,
                    UpdatedBy = department.UpdatedBy,
                };
                response.Data = departmentDto;
                response.Message = "Department fetched";

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public async Task<ApiResponse<bool>> DeleteById(int id)
        {
            try
            {
                var response = new ApiResponse<bool>();
                // fetching the department 
                var department = await _context.Departments
                    .FirstOrDefaultAsync(d => d.Id == id & d.IsActive);

                // checking if department exist or not
                if (department != null)
                {
                    // soft delete setting the department isActive status
                    department.IsActive = false;

                    // optional thing
                    /*var employees = await _context.Employees.Where(e=> e.DepartmentID == id).ToListAsync();
                    foreach ( var employee in employees)
                    {
                        employee.DepartmentID = null;
                    }*/

                    // hard delete
                    //_context.Departments.Remove(department);

                    await _context.SaveChangesAsync();
                    response.Message = "Department deleted";
                    return response;
                }
                response.Message = "Department not found";
                response.Status = 404;
                return response;
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<int>> Add(int userId, AddDepartmentDto department)
        {
            try
            {
                var response = new ApiResponse<int>();
                // checking whether department with given name already exist or not
                var departmentExist = await _context.Departments.FirstOrDefaultAsync(d => d.Name == department.Name);
               
                if (departmentExist != null)
                {
                    if (departmentExist.IsActive)
                    {
                        response.Message = "Department already exist";
                        response.Status = 409;
                        response.Data = departmentExist.Id;
                        return response;
                    }
                    response.Message = "Department added";
                    response.Data = departmentExist.Id;
                    departmentExist.CreatedOn = DateTime.Now;
                    departmentExist.CreatedBy = userId;
                    await _context.SaveChangesAsync();
                    return response;
                }

                // creating a department model to add into the database
                var departmentModel = new Department
                {
                    Name = department.Name,
                    CreatedOn = DateTime.Now,
                    CreatedBy = userId,
                };
                _context.Departments.Add(departmentModel);
                await _context.SaveChangesAsync();
                response.Message = "Department added";
                response.Data = departmentModel.Id;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }


}
