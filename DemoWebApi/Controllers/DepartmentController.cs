using DemoWebApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public DepartmentController(AppDbContext context)
        {
            _appDbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            var departments = await _appDbContext.Departments.ToListAsync();

            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartmentWithId(int id)
        {
            var department = await _appDbContext.Departments.FindAsync(id);
            if(department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [HttpPost]
        public async Task<ActionResult<Department>> AddDepartment(Department department)
        {
           _appDbContext.Departments.Add(department);
            await _appDbContext.SaveChangesAsync();
            return Ok(department);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            var department = _appDbContext.Departments.Find(id);
            if(department == null)
            {
                return BadRequest();
            }

            _appDbContext.Departments.Remove(department);
            await _appDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDepartment(int id,  Department department)
        {
            if(id != department.Id)
            {
                return BadRequest();
            }

            _appDbContext.Entry(department).State = EntityState.Modified;

            await _appDbContext.SaveChangesAsync();
            return NoContent();
        }


    }
}
