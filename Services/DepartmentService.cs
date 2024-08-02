using EmployeeSystemWebApi.Contract;
using EmployeeSystemWebApi.Contract.Models;
using EmployeeSystemWebApi.Provider.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSystemWebApi.Provider.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            try
            {
                var departments = await _context.Departments.ToListAsync();
                return departments;
            }catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Department?> GetById(int id)
        {
            try
            {
                var department = await _context.Departments.Where(d => d.Id == id).FirstOrDefaultAsync();
                return department;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                
            }
        }
        public async Task DeleteById(int id)
        {
            try
            {
                var department = await _context.Departments.Where(d => d.Id == id).FirstOrDefaultAsync();
                if(department != null)
                {
                    _context.Departments.Remove(department);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Department not found");
                }
                return;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddNew(Department department)
        {
            try
            {
                var departmentExist = await _context.Departments.Where(d => d.Name == department.Name).FirstOrDefaultAsync();
                if (departmentExist != null)
                {
                    throw new Exception("Department with given name already exist");
                }
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
