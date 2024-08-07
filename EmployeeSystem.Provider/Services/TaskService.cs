
using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Provider.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Tasks> GetTasksInfo(int userId)
        {
            // fetching the user details
            var user = _context.Employees.FirstOrDefault(e => e.UserId == userId);

            // checking whether the user is superadmin or not and creating the query according to the role
            if (user != null && user.Role != Role.SuperAdmin)
            {
                var query = _context.Tasks.Include(t => t.Employee).Include(t => t.Admin).Where(t => t.IsActive & (t.Employee.Id == user.Id || t.Employee.ManagerID == user.Id));
                return query;
            }
            return _context.Tasks;
        }

        public async Task<bool> CheckTaskValidAssign(int assignedTo, int assignedBy)
        {
            try
            {
                // extracting the assignee details
                var assignee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == assignedTo);

                // extracting the assigner details
                var assinger = await _context.Employees.FirstOrDefaultAsync(e => e.Id == assignedBy);

                return ((assinger != null && assignee != null) && (assinger.Role == Role.SuperAdmin || assignee.ManagerID == assignedBy));
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TasksDto>> GetAllTasks(int userId)
        {
            try
            {
                // filter according to the role
                var query = GetTasksInfo(userId);

                // creating the task dto list
                var tasks = await query.Where(t => t.IsActive).Select(t => new TasksDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    AssignedBy = t.AssignedBy,
                    AssignedTo = t.AssignedTo,
                    Status = t.Status,
                    Description = t.Description,
                    AssigneeName = t.Employee.Name,
                    AssignerName = t.Admin.Name,
                    CreatedBy = t.CreatedBy,
                    UpdatedBy = t.UpdatedBy,
                    CreatedOn = t.CreatedOn,
                    UpdatedOn = t.UpdatedOn,

                }).ToListAsync();

                return tasks;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TasksDto>> GetEmployeeTask(int employeeId)
        {
            try
            {
               

                // creating the task dto list
                var tasks = await _context.Tasks
                    .Where(t => t.IsActive & t.AssignedTo == employeeId)
                    .Select(t => new TasksDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        AssignedBy = t.AssignedBy,
                        AssignedTo = t.AssignedTo,
                        Status = t.Status,
                        Description = t.Description,
                        AssigneeName = t.Employee.Name,
                        AssignerName = t.Admin.Name,
                        CreatedBy = t.CreatedBy,
                        UpdatedBy = t.UpdatedBy,
                        CreatedOn = t.CreatedOn,
                        UpdatedOn = t.UpdatedOn,

                    }).ToListAsync();

                return tasks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<TasksDto?> GetById(int userId, int id)
        {
            try
            {
                // filter according to the role
                var query = GetTasksInfo(userId);

                // check whether the task belongs to the list, then convert the task into task dto form
                var task = await query.Select(t => new TasksDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    AssignedBy = t.AssignedBy,
                    AssignedTo = t.AssignedTo,
                    Status = t.Status,
                    Description = t.Description,
                    AssigneeName = t.Employee.Name,
                    AssignerName = t.Admin.Name,
                    CreatedBy = t.CreatedBy,
                    UpdatedBy = t.UpdatedBy,
                    CreatedOn = t.CreatedOn,
                    UpdatedOn = t.UpdatedOn,

                }).FirstOrDefaultAsync(t => t.Id == id);

                    
                return task;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TasksDto?> UpdateStatus(int userId, int id, TasksStatus taskStatus)
        {
            try
            {
                // we can apply check first

                // fetching the task details
                var task = await _context.Tasks
                    .Select(t => new TasksDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        AssignedBy = t.AssignedBy,
                        AssignedTo = t.AssignedTo,
                        Status = t.Status,
                        Description = t.Description,
                        AssigneeName = t.Employee.Name,
                        AssignerName = t.Admin.Name,
                        CreatedBy = t.CreatedBy,
                        UpdatedBy = t.UpdatedBy,
                        CreatedOn = t.CreatedOn,
                        UpdatedOn = t.UpdatedOn,

                    })
                    .FirstOrDefaultAsync(t => t.Id == id);
                    
                    
                if (task == null)
                {
                    return null;
                }

                // updating the status
                task.Status = taskStatus;
                task.UpdatedOn = DateTime.Now;
                task.UpdatedBy = userId;
                await _context.SaveChangesAsync();

                return task;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Add(int userId, int adminId, AddTaskDto taskDto)
        {
            try
            {
                int assignedById = adminId;
                int assignedToId = taskDto.AssignedTo;

                // fetching the assigned user details
                var assignedUser = await _context.Employees.FirstOrDefaultAsync(e => e.Id == assignedToId);
                if(assignedUser == null)
                {
                    return -3;
                }

                // check for whether the task belongs to the project or not
                if(taskDto.ProjectId != null && taskDto.ProjectId != 0)
                {
                    var project = await _context.Projects.FirstOrDefaultAsync(p => p.IsActive & p.Id == taskDto.ProjectId);

                    if (project == null)
                    {
                        return -2;
                    }

                    // check whether the task whom to assign is belongs to that project 
                    var checkProjectEmployee = await _context.ProjectEmployees
                    .FirstOrDefaultAsync(p => p.ProjectId == taskDto.ProjectId
                        && p.EmployeeId == assignedToId);
                    // employee with given id is not in the project so can't assign the task
                    if (checkProjectEmployee == null)
                    {
                        return -1;
                    }
                }
                

                // check for valid task assin
                var check = assignedById == assignedToId ? true : await CheckTaskValidAssign(assignedToId, assignedById);
                if (!check)
                {
                    return 0;
                }

                // creating the new task model
                var task = new Tasks
                {
                    Name = taskDto.Name,
                    Description = taskDto.Description,
                    AssignedBy = adminId,
                    AssignedTo = taskDto.AssignedTo,
                    Status = taskDto.Status,
                    ProjectId = taskDto.ProjectId == 0 ?null : taskDto.ProjectId,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                };

                // adding and updating the database
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                return task.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // check for assigner details
        public async Task<bool?> Delete(int userId, int id)
        {
            try
            {
                // fetching the details of the task
                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id & t.IsActive);

                if (task == null)
                {
                    return false;
                }
                int assignedById = task.AssignedBy;
                int assignedToId = task.AssignedTo;

                // fetching employee
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == userId & e.IsActive);

                var check = false;

                // check user who is deleting the task has authority to delete it or not
                if (employee != null && (employee.Role == Role.SuperAdmin || employee.Id == assignedById))
                {
                    check = true;
                }
                if (!check)
                {
                    return null;
                }
               
                // soft delete
                task.IsActive = false;
                // _context.Tasks.Remove(task);    
                await _context.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> TaskExist(int id)
        {
            try
            {
                // fetching the task
                var task = await _context.Tasks.Where(t => t.Id ==id & t.IsActive).FirstOrDefaultAsync();
                return task != null;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
