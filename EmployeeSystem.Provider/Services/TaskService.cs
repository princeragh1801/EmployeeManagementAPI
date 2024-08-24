using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.IdAndName;
using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Provider.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITaskReviewService _reviewService;

        public TaskService(ApplicationDbContext context, ITaskReviewService taskReviewService)
        {
            _context = context;
            _reviewService = taskReviewService;
        }

        private bool CheckValidParent(TaskType parent, TaskType child)
        {
            if (parent == TaskType.Epic && child != TaskType.Feature)
            {
                return false;
            }
            else if (parent == TaskType.Feature && child != TaskType.Userstory)
            {
                return false;
            }
            else if (parent == TaskType.Userstory && (child != TaskType.Task && child != TaskType.Bug))
            {
                return false;
            }
            else if (parent == TaskType.Task || parent == TaskType.Bug)
            {
                return false;
            }
            return true;
        }

        public IQueryable<Tasks> GetTasksInfo(int userId)
        {
            // fetching the user details
            var user = _context.Employees.FirstOrDefault(e => e.Id == userId);

            // checking whether the user is superadmin or not and creating the query according to the role
            if (user != null && user.Role != Role.SuperAdmin)
            {
                var query = _context.Tasks
                    .Include(t => t.Employee)
                    .Include(t => t.Creator)
                    .Where(t => t.IsActive & (t.Employee.Id == user.Id || t.Employee.ManagerID == user.Id));
                return query;
            }
            return _context.Tasks.Where(t => t.IsActive);
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
            catch (Exception ex)
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
                    Status = t.Status,
                    ProjectId = t.ProjectId,
                    Description = t.Description,
                    AssigneeName = t.Employee.Name,
                    AssignerName = t.Creator.Name,
                    CreatedOn = t.CreatedOn,
                    TaskType = t.TaskType
                }).ToListAsync();

                return tasks;
            }
            catch (Exception ex)
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
                        Status = t.Status,
                        Description = t.Description,
                        ProjectId = t.ProjectId,
                        AssigneeName = t.Employee.Name,
                        AssignerName = t.Creator.Name,
                        CreatedOn = t.CreatedOn,
                        TaskType = t.TaskType
                    }).ToListAsync();

                return tasks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TasksDto>> GetSprintTask(int sprintId)
        {
            try
            {
                // creating the task dto list
                var tasks = await _context.Tasks
                    .Where(t => t.IsActive & t.SprintId == sprintId)
                    .Select(t => new TasksDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Status = t.Status,
                        Description = t.Description,
                        ProjectId = t.ProjectId,
                        AssigneeName = t.Employee.Name,
                        AssignerName = t.Creator.Name,
                        CreatedOn = t.CreatedOn,
                        TaskType = t.TaskType
                    }).ToListAsync();

                return tasks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TaskInfo?> GetById(int userId, int id)
        {
            try
            {
                // filter according to the role
                var query = GetTasksInfo(userId);

                // check whether the task belongs to the list, then convert the task into task dto form
                var task = await query
                    .Select(t => new TasksDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Status = t.Status,
                        Description = t.Description,
                        AssigneeName = t.Employee.Name,
                        ProjectId = t.ProjectId,
                        AssignerName = t.Creator.Name,
                        CreatedOn = t.CreatedOn,
                        TaskType = t.TaskType

                    }).FirstOrDefaultAsync(t => t.Id == id);

                var reviews = await _reviewService.Get(id);

                var subTasks = await _context.Tasks
                    .Where(t => t.IsActive & t.ParentId == id)
                    .Select(t => new TaskIdAndName
                    {
                        Id = t.Id,
                        Name = t.Name,
                        TaskType = t.TaskType,
                    }).ToListAsync();

                var taskInfo = new TaskInfo
                {
                    Task = task,
                    Reviews = reviews,
                    SubTasks = subTasks
                };
                return taskInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TasksDto?> Update(int userId, int id, UpdateTaskDto taskDto)
        {
            try
            {
                // we can apply check first

                // fetching the task details
                var task = await _context.Tasks
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (task == null)
                {
                    return null;
                }
                var user = await _context.Employees.FirstAsync(e => e.Id == userId);
                int assignedToId = taskDto.AssignedTo ?? 0;
                
                if (assignedToId != 0)
                {
                    var assignedUser = await _context.Employees.FirstOrDefaultAsync(e => e.Id == assignedToId);
                    if(assignedUser != null)
                    {
                        task.AssignedTo = assignedToId;
                        var assinedTolog = new TaskLog
                        {
                            Message = $"Task Assigned to {assignedUser.Name} by {user.Name}, on {task.UpdatedOn}",
                            Id = task.Id,
                        };

                        _context.TaskLogs.Add(assinedTolog);
                    }
                }
                // updating the status

                if (!string.IsNullOrEmpty(taskDto.Description))
                {
                    task.Description = taskDto.Description;
                    var log = new TaskLog
                    {
                        Message = $"Task description changed by {user.Name} at {DateTime.Now}",
                        TaskId = id,
                    };
                    _context.TaskLogs.Add(log);
                }

                if (!string.IsNullOrEmpty(taskDto.Name))
                {
                    var log = new TaskLog
                    {
                        Message = $"Task Name changed by {user.Name} - {task.Name} to {taskDto.Name} at {DateTime.Now}",
                        TaskId = id,
                    };
                    _context.TaskLogs.Add(log);
                    task.Name = taskDto.Name;
                }

                if (taskDto.Status != null)
                {
                    var status = taskDto.Status??TasksStatus.Pending;
                    var log = new TaskLog
                    {
                        Message = $"Task Status changed by {user.Name} - {task.Status} to {taskDto.Status} at {DateTime.Now}",
                        TaskId = id,
                    };
                    _context.TaskLogs.Add(log);
                    task.Status = status;
                }

                if(taskDto.TaskType != null)
                {
                    var log = new TaskLog
                    {
                        Message = $"Task Type changed by {user.Name} - {task.TaskType} to {taskDto.TaskType} at {DateTime.Now}",
                        TaskId = id,
                    };
                    _context.TaskLogs.Add(log);
                    task.TaskType = taskDto.TaskType??TaskType.Epic;
                }

                if(taskDto.ParentId != null)
                {
                    var parent = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskDto.ParentId);
                    
                    var checkValidParent = CheckValidParent(parent.TaskType, task.TaskType);

                    if (!checkValidParent)
                    {
                        return null;
                    }
                    
                    var log = new TaskLog
                    {
                        Message = $"Task Parent changed by {user.Name} New parent - {parent.Name} at {DateTime.Now}",
                        TaskId = id,
                    };
                    _context.TaskLogs.Add(log);
                    task.ParentId = taskDto.ParentId;
                }

                if(taskDto.OriginalEstimateHours != null)
                {
                    var log = new TaskLog
                    {
                        Message = $"Original Estimate hours set to {taskDto.OriginalEstimateHours} by {user.Name} at {DateTime.Now}",
                        TaskId = id,
                    };
                    _context.TaskLogs.Add(log);
                    task.OriginalEstimateHours = taskDto.OriginalEstimateHours;
                }
                if(taskDto.RemainingEstimateHours != null)
                {
                    var log = new TaskLog
                    {
                        Message = $"Remaining Estimate hours changed - {task.OriginalEstimateHours} to {taskDto.RemainingEstimateHours} by {user.Name} at {DateTime.Now}",
                        TaskId = id,
                    };
                    _context.TaskLogs.Add(log);
                    task.RemainingEstimateHours = taskDto.RemainingEstimateHours;
                }

                task.UpdatedOn = DateTime.Now;
                task.UpdatedBy = userId;
                await _context.SaveChangesAsync();

                var taskDetails = new TasksDto
                {
                    Id = task.Id,
                    Name = task.Name,
                    Status = task.Status,
                    ProjectId = task.ProjectId,
                    Description = task.Description,
                    AssigneeName = task.Employee.Name,
                    AssignerName = task.Creator.Name,
                    CreatedOn = task.CreatedOn,
                };

                return taskDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Add(int adminId, AddTaskDto taskDto)
        {
            try
            {
                var parentId = taskDto.ParentId ?? 0;
                if (parentId != 0)
                {
                    var parent = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == parentId);
                    var checkValidParent = CheckValidParent(parent.TaskType, taskDto.TaskType);
                    if (!checkValidParent)
                    {
                        return -4;
                    }
                }
                int assignedById = adminId;
                int assignedToId = taskDto.AssignedTo ?? 0;
               
                if (assignedToId == 0)
                {
                    var taskToAdd = new Tasks
                    {
                        Name = taskDto.Name,
                        Description = taskDto.Description,
                        AssignedTo = assignedToId == 0 ? null : assignedToId,
                        Status = taskDto.Status,
                        TaskType = taskDto.TaskType,
                        SprintId = taskDto.SprintId == 0 ? null : taskDto.SprintId,
                        ParentId = taskDto.ParentId == 0 ? null : taskDto.ParentId,
                        OriginalEstimateHours = taskDto.OriginalEstimateHours == 0 ? null : taskDto.OriginalEstimateHours,
                        RemainingEstimateHours = taskDto.OriginalEstimateHours == 0 ? null : taskDto.OriginalEstimateHours,
                        ProjectId = taskDto.ProjectId,
                        CreatedBy = adminId,
                        CreatedOn = DateTime.Now,
                    };

                    _context.Add(taskToAdd);
                    await _context.SaveChangesAsync();
                    var log = new TaskLog
                    {
                        Message = $"A new task created by {taskToAdd.Creator.Name} at {taskToAdd.CreatedOn}",
                        TaskId = taskToAdd.Id
                    };
                    _context.TaskLogs.Add(log);
                    await _context.SaveChangesAsync();
                    return taskToAdd.Id;
                }

                // fetching the assigned user details
                var assignedUser = await _context.Employees.FirstOrDefaultAsync(e => e.Id == assignedToId);
                if (assignedUser == null)
                {
                    return -3;
                }

                // check for whether the task belongs to the project or not
                if (taskDto.ProjectId != 0)
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
                    AssignedTo = taskDto.AssignedTo,
                    Status = taskDto.Status,
                    TaskType = taskDto.TaskType,
                    ParentId = taskDto.ParentId == 0 ? null : taskDto.ParentId,
                    ProjectId = taskDto.ProjectId,
                    SprintId = taskDto.SprintId == 0 ? null : taskDto.SprintId,
                    OriginalEstimateHours = taskDto.OriginalEstimateHours == 0 ? null : taskDto.OriginalEstimateHours,
                    RemainingEstimateHours = taskDto.OriginalEstimateHours == 0 ? null : taskDto.OriginalEstimateHours,
                    CreatedBy = adminId,
                    CreatedOn = DateTime.Now,
                };

                // adding and updating the database
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                var taskLog = new TaskLog
                {
                    Message = $"A new task created by {task.Creator.Name} at {task.CreatedOn}",
                    TaskId = task.Id
                };
                _context.TaskLogs.Add(taskLog);
                await _context.SaveChangesAsync();
                return task.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddMany(int adminId, List<AddTaskDto> taskList)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await transaction.CreateSavepointAsync("Adding list");
                foreach (var taskDto in taskList)
                {
                    var parentId = taskDto.ParentId ?? 0;
                    if (parentId != 0)
                    {
                        var parent = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == parentId);
                        var checkValidParent = CheckValidParent(parent.TaskType, taskDto.TaskType);
                        if (!checkValidParent)
                        {
                            return false;
                        }
                    }

                    int assignedById = adminId;
                    int assignedToId = taskDto.AssignedTo ?? 0;

                    if (assignedToId == 0)
                    {
                        var taskToAdd = new Tasks
                        {
                            Name = taskDto.Name,
                            Description = taskDto.Description,
                            AssignedTo = assignedToId == 0 ? null : assignedToId,
                            Status = taskDto.Status,
                            TaskType = taskDto.TaskType,
                            ParentId = taskDto.ParentId == 0 ? null : taskDto.ParentId,
                            ProjectId = taskDto.ProjectId,
                            SprintId = taskDto.SprintId == 0 ? null : taskDto.SprintId,
                            OriginalEstimateHours = taskDto.OriginalEstimateHours == 0 ? null : taskDto.OriginalEstimateHours,
                            RemainingEstimateHours = taskDto.OriginalEstimateHours == 0 ? null : taskDto.OriginalEstimateHours,
                            CreatedBy = adminId,
                            CreatedOn = DateTime.Now,
                        };

                        _context.Add(taskToAdd);
                        await _context.SaveChangesAsync();
                        var taskLog = new TaskLog
                        {
                            Message = $"A new task created by {taskToAdd.Creator.Name} at {taskToAdd.CreatedOn} ",
                            TaskId = taskToAdd.Id
                        };
                        _context.TaskLogs.Add(taskLog);
                        //await _context.SaveChangesAsync();
                        continue;
                    }

                    // fetching the assigned user details
                    var assignedUser = await _context.Employees.FirstOrDefaultAsync(e => e.Id == assignedToId);
                    if (assignedUser == null)
                    {
                        return false;
                    }

                    // check for whether the task belongs to the project or not
                    if (taskDto.ProjectId != null && taskDto.ProjectId != 0)
                    {
                        var project = await _context.Projects.FirstOrDefaultAsync(p => p.IsActive & p.Id == taskDto.ProjectId);

                        if (project == null)
                        {
                            return false;
                        }

                        // check whether the task whom to assign is belongs to that project 
                        var checkProjectEmployee = await _context.ProjectEmployees
                        .FirstOrDefaultAsync(p => p.ProjectId == taskDto.ProjectId
                            && p.EmployeeId == assignedToId);
                        // employee with given id is not in the project so can't assign the task
                        if (checkProjectEmployee == null)
                        {
                            return false;
                        }
                    }


                    // check for valid task assin
                    var check = assignedById == assignedToId ? true : await CheckTaskValidAssign(assignedToId, assignedById);
                    if (!check)
                    {
                        return false;
                    }

                    // creating the new task model
                    var task = new Tasks
                    {
                        Name = taskDto.Name,
                        Description = taskDto.Description,
                        AssignedTo = taskDto.AssignedTo,
                        Status = taskDto.Status,
                        SprintId = taskDto.SprintId == 0 ? null : taskDto.SprintId,
                        ParentId = taskDto.ParentId == 0 ? null : taskDto.ParentId,
                        OriginalEstimateHours = taskDto.OriginalEstimateHours == 0 ? null : taskDto.OriginalEstimateHours,
                        RemainingEstimateHours = taskDto.OriginalEstimateHours == 0 ? null : taskDto.OriginalEstimateHours,
                        ProjectId = taskDto.ProjectId,
                        CreatedBy = adminId,
                        CreatedOn = DateTime.Now,
                    };

                    // adding and updating the database
                    _context.Tasks.Add(task);
                    var log = new TaskLog
                    {
                        Message = $"A new task created by {task.Creator.Name} at {task.CreatedOn}",
                        TaskId = task.Id
                    };
                    _context.TaskLogs.Add(log);

                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackToSavepointAsync("Adding list");
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
                int ?assignedById = task.CreatedBy;

                // fetching employee
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == userId & e.IsActive);

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

                var log = new TaskLog
                {
                    Message = $"Task Removed by {employee.Name} at {DateTime.Now}",
                    TaskId = id
                };
                _context.TaskLogs.Add(log);
                await _context.SaveChangesAsync();
                // _context.Tasks.Remove(task);    
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> TaskExist(int id)
        {
            try
            {
                // fetching the task
                var task = await _context.Tasks.Where(t => t.Id == id & t.IsActive).FirstOrDefaultAsync();
                return task != null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<EpicTaskDto>> GetEpics()
        {
            try
            {
                var epics = await _context.Tasks
                    .Include(t => t.Employee)
                    .Where(t => t.IsActive && t.TaskType == TaskType.Epic)
                    .ToListAsync();

                var result = new List<EpicTaskDto>();

                foreach (var epic in epics)
                {
                    var features = new List<EpicTaskDto> ();
                    var featureDto = await _context.Tasks
                        .Include(t => t.Employee)
                        .Where(t => t.IsActive & t.TaskType == TaskType.Feature & t.ParentId == epic.Id)
                            .Select(t => new EpicTaskDto
                            {
                                Id = t.Id,
                                Name = t.Name,
                                TaskType = t.TaskType,
                                Status = t.Status,
                                AssignedTo = t.Employee.Name,
                                CreatedOn = t.CreatedOn
                            }).ToListAsync();

                    foreach (var feature in featureDto)
                    {
                        var userStories = new List<EpicTaskDto> ();
                        var userStoryDto = await _context.Tasks
                            .Include(t => t.Employee)
                            .Where(t => t.IsActive & t.TaskType == TaskType.Userstory & t.ParentId == feature.Id)
                            .Select(t => new EpicTaskDto
                            {
                                Id = t.Id,
                                Name = t.Name,
                                TaskType = t.TaskType,
                                Status = t.Status,
                                AssignedTo = t.Employee.Name,
                                CreatedOn = t.CreatedOn
                            }).ToListAsync();
                        
                        foreach(var userStory in userStoryDto)
                        {
                            var taskAndBugsDto = await _context.Tasks
                                .Include(t => t.Employee)
                                .Where(t => t.IsActive && (t.TaskType == TaskType.Task || t.TaskType == TaskType.Bug) && t.ParentId == userStory.Id)
                                .Select(t => new EpicTaskDto
                                {
                                    Id = t.Id,
                                    Name = t.Name,
                                    TaskType = t.TaskType,
                                    Status = t.Status,
                                    AssignedTo = t.Employee.Name,
                                    CreatedOn = t.CreatedOn
                                }).ToListAsync();

                            var taskBugDto = new EpicTaskDto
                            {
                                Id = userStory.Id,
                                Name = userStory.Name,
                                TaskType = userStory.TaskType,
                                AssignedTo = userStory.AssignedTo,
                                CreatedOn = userStory.CreatedOn,
                                Status = userStory.Status,
                                SubItems = taskAndBugsDto
                            };

                            userStories.Add(taskBugDto);
                        }
                        var userStoryToAdd = new EpicTaskDto
                        {
                            Id = feature.Id,
                            Name = feature.Name,
                            TaskType = feature.TaskType,
                            AssignedTo = feature.AssignedTo,
                            Status = feature.Status,
                            CreatedOn = feature.CreatedOn,
                            SubItems = userStories
                        };

                        features.Add(userStoryToAdd);
                    }

                    var epicDto = new EpicTaskDto
                    {
                        Id = epic.Id,
                        Name = epic.Name,
                        TaskType = epic.TaskType,
                        Status = epic.Status,
                        AssignedTo = epic.Employee.Name??null,
                        CreatedOn = epic.CreatedOn,
                        SubItems = features
                    };

                    result.Add(epicDto);
                }


                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    
        public async Task<List<TasksDto>> GetChildren(int id)
        {
            try
            {
                var subTasks = await _context.Tasks
                    .Where(t => t.IsActive & t.ParentId == id)
                    .Select(e => new TasksDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        //Description = e.Description,
                        CreatedOn = e.CreatedOn,
                        //ProjectId = e.ProjectId,
                        TaskType = e.TaskType,
                        AssigneeName = e.Employee.Name,
                        //AssignerName = e.Admin.Name,
                        Status = e.Status,

                    }).ToListAsync();

                return subTasks;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<bool> UpdateTaskSprint(int sprintId, int taskId)
        {
            try
            {
                var sprint = await _context.Sprints.FirstOrDefaultAsync(s => s.Id == sprintId);
                if(sprint == null)
                {
                    return false;
                }
                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId & t.IsActive & sprint.projectId == t.ProjectId);

                if(task == null)
                {
                    return false;
                }

                task.SprintId = sprintId;

                var log = new TaskLog
                {
                    Message = $"Task added to sprint - {sprint.Name} at {DateTime.Now}",
                    TaskId = taskId,
                };
                _context.TaskLogs.Add(log);
                await _context.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateTaskStatus(int id, TasksStatus status)
        {
            try
            {
                // fetching the details of the task
                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id & t.IsActive);

                if (task == null)
                {
                    return false;
                }
                var log = new TaskLog
                {
                    Message = $"Task status changed - {task.Status} to {status} at {DateTime.Now}",
                    TaskId = id,
                };

                task.Status = status;
                
                _context.TaskLogs.Add(log);
                // _context.Tasks.Remove(task);    
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateTaskParent(int id, int parentId)
        {
            try
            {
                // fetching the details of the task
                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id & t.IsActive);
                var parent = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == parentId);

                if (task == null || parent == null)
                {
                    return false;
                }

                if((task.TaskType == TaskType.Feature && parent.TaskType == TaskType.Epic) || (task.TaskType == TaskType.Userstory && parent.TaskType == TaskType.Feature) || ((task.TaskType == TaskType.Task || task.TaskType == TaskType.Bug) && parent.TaskType == TaskType.Userstory))
                {
                    task.ParentId = parentId;
                    var log = new TaskLog
                    {
                        Message = $"Task parent updated, Parent Name - {parent.Name} at {DateTime.Now}",
                        TaskId = id,
                    };
                    _context.TaskLogs.Add(log);
                    await _context.SaveChangesAsync();
                    return true;
                }
                // _context.Tasks.Remove(task);    
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<LogDto>> GetLogs(int taskId)
        {
            try
            {
               var logs = await _context.TaskLogs.Where(t => t.TaskId == taskId)
                    .Select(t => new LogDto
                    {
                        Message = t.Message
                    })
                    .ToListAsync();
                return logs;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<Tasks?> GetById(int id)
        {
            try
            {
                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
                return task;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateTask(Tasks task)
        {
            try
            {
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
