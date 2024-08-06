﻿using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Provider.Services
{
    public class PaginatedService : IPaginatedService
    {
        private readonly ApplicationDbContext _context;

        public PaginatedService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public IQueryable<Employee> GetOrdered(IQueryable<Employee> query, string columnName, bool ace = true)
        {
            if (columnName.ToLower() == "name")
            {
                query = ace ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
            else if (columnName.ToLower() == "managername")
            {
                query = query.Where(e => e.ManagerID != null);
                query = ace ? query.OrderBy(x => x.Manager.Name) : query.OrderByDescending(x => x.Manager.Name);
            }
            else if (columnName.ToLower() == "departmentname")
            {
                query = query.Where(e => e.Department.Name != null);

                query = ace ? query.OrderBy(x => x.Department.Name) : query.OrderByDescending(x => x.Department.Name);
            }
            else if (columnName.ToLower() == "salary")
            {
                query = ace ? query.OrderBy(x => x.Salary) : query.OrderByDescending(x => x.Salary);
            }
            else if (columnName.ToLower() == "createdon")
            {
                query = ace ? query.OrderBy(x => x.CreatedOn) : query.OrderByDescending(x => x.CreatedOn);
            }else if (columnName.ToLower() == "updatedon")
            {
                query = query.Where(e => e.UpdatedOn != null);
                query = ace ? query.OrderBy(x => x.UpdatedOn) : query.OrderByDescending(x => x.UpdatedOn);
            }else if(columnName.ToLower() == "role")
            {
                query = ace ? query.OrderBy(x => x.Role) : query.OrderByDescending(x => x.Role);
            }
            else
            {
                query = ace ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
            }
            return query;
        }

        public IQueryable<Department> GetOrdered(IQueryable<Department> query, string columnName, bool ace = true)
        {
            if (columnName.ToLower() == "name")
            {
                query = ace ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }else if (columnName.ToLower() == "createdon")
            {
                query = ace ? query.OrderBy(x => x.CreatedOn) : query.OrderByDescending(x => x.CreatedOn);
            }
            else if (columnName.ToLower() == "updatedon")
            {
                query = query.Where(e => e.UpdatedOn != null);
                query = ace ? query.OrderBy(x => x.UpdatedOn) : query.OrderByDescending(x => x.UpdatedOn);
            }
            else
            {
                query = ace ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
            }
            return query;
        }

        public IQueryable<Project> GetOrdered(IQueryable<Project> query, string columnName, bool ace = true)
        {
            if (columnName.ToLower() == "name")
            {
                query = ace ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
            else if (columnName.ToLower() == "description")
            {
                query = ace ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description);
            }
            else if (columnName.ToLower() == "createdon")
            {
                query = ace ? query.OrderBy(x => x.CreatedOn) : query.OrderByDescending(x => x.CreatedOn);
            }
            else if (columnName.ToLower() == "updatedon")
            {
                query = query.Where(e => e.UpdatedOn != null);
                query = ace ? query.OrderBy(x => x.UpdatedOn) : query.OrderByDescending(x => x.UpdatedOn);
            }
            else
            {
                query = ace ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
            }
            return query;
        }

        public IQueryable<Tasks> GetOrdered(IQueryable<Tasks> query, string columnName, bool ace = true)
        {
            if (columnName.ToLower() == "name")
            {
                query = ace ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
            else if (columnName.ToLower() == "description")
            {
                query = ace ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description);
            }
            else if (columnName.ToLower() == "createdon")
            {
                query = ace ? query.OrderBy(x => x.CreatedOn) : query.OrderByDescending(x => x.CreatedOn);
            }
            else if (columnName.ToLower() == "updatedon")
            {
                query = query.Where(e => e.UpdatedOn != null);
                query = ace ? query.OrderBy(x => x.UpdatedOn) : query.OrderByDescending(x => x.UpdatedOn);
            }
            else
            {
                query = ace ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
            }
            return query;
        }

        public async Task<PaginatedItemsDto<List<EmployeeDto>>> GetEmployees(PaginatedDto paginatedDto)
        {
            try
            {
                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;
                var query = _context.Employees
                    .Include(e => e.Manager)
                    .Include(m => m.Department)
                    .Where(e => e.IsActive);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(e => e.Name.Contains(search));
                }

                query = orderBy == SortedOrder.NoOrder ? query : GetOrdered(query, orderKey, orderBy == SortedOrder.Ascending ? true : false);

                var totalCount = query.Count(); 
                var totalPages = query.Count() / paginatedDto.PagedItemsCount;
                if (query.Count() % paginatedDto.PagedItemsCount != 0)
                {
                    totalPages++;
                }

                var employees = await query.
                    Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                    .Take(paginatedDto.PagedItemsCount)
                    .Select(e => new EmployeeDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Salary = e.Salary,
                        Role = e.Role,
                        ManagerName = e.Manager.Name,
                        DepartmentName = e.Department.Name,
                        DepartmentId = e.DepartmentID,
                        ManagerId = e.ManagerID,
                        CreatedBy = e.CreatedBy,
                        UpdatedBy = e.UpdatedBy,
                        CreatedOn = e.CreatedOn,
                        UpdatedOn = e.UpdatedOn,
                    }).ToListAsync();
                PaginatedItemsDto<List<EmployeeDto>> res = new PaginatedItemsDto<List<EmployeeDto>>();

                res.Data = employees;
                res.TotalPages = totalPages;
                res.TotalItems = totalCount;
                return res;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<PaginatedItemsDto<List<DepartmentDto>>> GetDepartments(PaginatedDto paginatedDto)
        {
            try
            {
                var query = _context.Departments.Where(d => d.IsActive);

                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d => d.Name.Contains(search));
                }

                query = orderBy == SortedOrder.NoOrder ? query : GetOrdered(query, orderKey, orderBy == SortedOrder.Ascending ? true : false);
                var totalCount = query.Count();
                var totalPages = query.Count() / paginatedDto.PagedItemsCount;
                if (query.Count() % paginatedDto.PagedItemsCount != 0)
                {
                    totalPages++;
                }
                var departments = await query
                    .Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                    .Take(paginatedDto.PagedItemsCount)
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

                PaginatedItemsDto<List<DepartmentDto>> res = new PaginatedItemsDto<List<DepartmentDto>>();

                res.Data =departments;
                res.TotalPages = totalPages;
                res.TotalItems = totalCount;
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PaginatedItemsDto<List<ProjectDto>>> GetProjects(PaginatedDto paginatedDto)
        {
            try
            {
                var query = _context.Projects.Where(d => d.IsActive);

                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d => d.Name.Contains(search));
                }

                query = orderBy == SortedOrder.NoOrder ? query : GetOrdered(query, orderKey, orderBy == SortedOrder.Ascending ? true : false);

                var totalPages = query.Count() / paginatedDto.PagedItemsCount;
                var totalCount = query.Count();
                if (query.Count() % paginatedDto.PagedItemsCount != 0)
                {
                    totalPages++;
                }
                var projects = await query
                    .Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                    .Take(paginatedDto.PagedItemsCount)
                    .Select(e => new ProjectDto
                     {
                         Id = e.Id,
                         Name = e.Name,
                         Description = e.Description,
                         CreatedBy = e.CreatedBy,
                         UpdatedBy = e.UpdatedBy,
                         CreatedOn = e.CreatedOn,
                         UpdatedOn = e.UpdatedOn,
                     }).ToListAsync();
                PaginatedItemsDto<List<ProjectDto>> res = new PaginatedItemsDto<List<ProjectDto>>();

                res.Data = projects;
                res.TotalPages = totalPages;
                res.TotalItems = totalCount;
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PaginatedItemsDto<List<TasksDto>>> GetTasks(PaginatedDto paginatedDto)
        {
            try
            {
                var query = _context.Tasks.Where(d => d.IsActive);

                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d => d.Name.Contains(search));
                }

                query = orderBy == SortedOrder.NoOrder ? query : GetOrdered(query, orderKey, orderBy == SortedOrder.Ascending ? true : false);
                var totalPages = query.Count() / paginatedDto.PagedItemsCount;
                var totalCount = query.Count();
                if (query.Count() % paginatedDto.PagedItemsCount != 0)
                {
                    totalPages++;
                }
                var tasks = await query
                    .Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                    .Take(paginatedDto.PagedItemsCount)
                    .Select(e => new TasksDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Description = e.Description,
                        CreatedBy = e.CreatedBy,
                        UpdatedBy = e.UpdatedBy,
                        CreatedOn = e.CreatedOn,
                        UpdatedOn = e.UpdatedOn,
                    }).ToListAsync();
                PaginatedItemsDto<List<TasksDto>> res = new PaginatedItemsDto<List<TasksDto>>();
                res.Data = tasks;
                res.TotalPages = totalPages;
                res.TotalItems = totalCount;
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
