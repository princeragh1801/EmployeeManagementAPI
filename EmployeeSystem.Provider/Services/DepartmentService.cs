using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.Info.PaginationInfo;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using EmployeeSystem.Contract.Response;
using Microsoft.EntityFrameworkCore;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Provider.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IUtilityService _utilityService;
        public DepartmentService(ApplicationDbContext applicationDbContext, IUtilityService utilityService, IMapper mapper)
        {
            _context = applicationDbContext;
            _utilityService = utilityService;
            _mapper = mapper;
        }

        public async Task<PaginatedItemsDto<List<DepartmentPaginationInfo>>> Get(PaginatedDto<Role?> paginatedDto)
        {
            try
            {
                // only selecting which is active
                var query = _context.Departments.Include(d => d.Creator).Where(d => d.IsActive);

                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;

                // applying search filter on that
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d => d.Name.Contains(search));
                }

                var range = paginatedDto.DateRange;

                // range filter
                if (range != null)
                {
                    var startDate = range.StartDate;
                    var endDate = range.EndDate;

                    query = query.Where(t => t.CreatedOn >= startDate && t.CreatedOn <= endDate);
                }

                // now getting the order wise details
                query = orderBy == SortedOrder.NoOrder ? query : _utilityService.GetOrdered<Department>(query, orderKey, orderBy == SortedOrder.Ascending ? true : false);

                // calculating the total count and pages
                var totalCount = query.Count();
                var totalPages = totalCount / paginatedDto.PagedItemsCount;
                if (totalCount % paginatedDto.PagedItemsCount != 0)
                {
                    totalPages++;
                }

                // now extrating departments of the page-[x]
                var departments = await query
                    .Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                    .Take(paginatedDto.PagedItemsCount)
                    .ProjectTo<DepartmentPaginationInfo>(_mapper.ConfigurationProvider).ToListAsync();

                // creating new dto to send the info
                PaginatedItemsDto<List<DepartmentPaginationInfo>> res = new PaginatedItemsDto<List<DepartmentPaginationInfo>>();

                res.Data = departments;
                res.TotalPages = totalPages;
                res.TotalItems = totalCount;
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<ApiResponse<List<DepartmentDto>>> GetAll()
        {
            try
            {
                var response = new ApiResponse<List<DepartmentDto>>();
                // fetching departments and converting them into list of department dto
                var departments = await _context.Departments
                    .Where(d => d.IsActive)
                    .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

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

                var departmentDto = _mapper.Map<DepartmentDto>(department);
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
