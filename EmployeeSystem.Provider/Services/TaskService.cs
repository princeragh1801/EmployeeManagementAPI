﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.Count;
using EmployeeSystem.Contract.Dtos.IdAndName;
using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Dtos.Info.PaginationInfo;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Provider.Services
{
    public class TaskService : ITaskService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ITaskReviewService _reviewService;
        private readonly IUtilityService _utilityService;
        private readonly ITaskLogService _logService;
        public TaskService(ApplicationDbContext context, ITaskReviewService taskReviewService, IUtilityService utilityService, ITaskLogService logService, IMapper mapper)
        {
            _context = context;
            _reviewService = taskReviewService;
            _utilityService = utilityService;
            _logService = logService;
            _mapper = mapper;
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

        public async Task<PaginatedItemsDto<List<TasksDto>>> Get(int userId, int projectId, ProjectTasksDto paginatedDto)
        {
            try
            {
                bool filter = false;
                var tasksList = _context.Tasks.Where(t => t.ProjectId == projectId & t.IsActive);

                var range = paginatedDto.DateRange;

                // range filter
                if (range != null)
                {
                    var startDate = range.StartDate;
                    var endDate = range.EndDate;

                    tasksList = tasksList.Where(t => t.CreatedOn >= startDate && t.CreatedOn <= endDate);
                }

                var sprint = paginatedDto.SprintId;

                // sprint filter
                if (sprint != null && sprint > 0)
                {
                    tasksList = tasksList.Where(t => t.SprintId == sprint);
                }

                var types = paginatedDto.Types;

                // types filter
                if (types != null && types.Count() > 0)
                {
                    filter = true;
                    tasksList = tasksList.Where(t => types.Contains(t.TaskType));

                }

                var status = paginatedDto.Status;

                // status filter
                if (status != null && status.Count() > 0)
                {
                    tasksList = tasksList.Where(t => status.Contains(t.Status));
                }

                var assignedTo = paginatedDto.AssignedTo;

                // assigned to filter
                if (assignedTo != null && assignedTo.Count() > 0)
                {
                    tasksList = tasksList.Where(t => t.AssignedTo != null && assignedTo.Contains(t.AssignedTo ?? 0));
                }

                var assign = paginatedDto.Assign;

                // assigned filter
                if (assign != null)
                {
                    tasksList = tasksList.Where(t => ((assign == true) ? (t.AssignedTo != null) : (t.AssignedTo == null)));
                }
                if (filter == false)
                {
                    var epics = tasksList.Where(t => t.TaskType == TaskType.Epic);
                    var features = tasksList.Where(t => t.TaskType == TaskType.Feature & t.ParentId == null);
                    var userStories = tasksList.Where(t => t.TaskType == TaskType.Userstory & t.ParentId == null);
                    var tasks = tasksList.Where(t => t.TaskType == TaskType.Task & t.ParentId == null);
                    var bugs = tasksList.Where(t => t.TaskType == TaskType.Bug & t.ParentId == null);
                    epics.Concat(features).Concat(userStories).Concat(tasks).Concat(bugs);
                    tasksList = epics;
                }


                var orderKey = paginatedDto.OrderKey ?? "Id";
                var search = paginatedDto.Search;
                var orderBy = paginatedDto.SortedOrder;

                if (!string.IsNullOrEmpty(search))
                {
                    tasksList = tasksList.Where(t => t.Name.Contains(search));
                }

                tasksList = orderBy == SortedOrder.NoOrder ? tasksList :_utilityService.GetOrdered(tasksList, orderKey, orderBy == SortedOrder.Ascending ? true : false);

                // calculating the total count and pages
                var totalCount = tasksList.Count();
                var totalPages = totalCount / paginatedDto.PagedItemsCount;
                if (totalCount % paginatedDto.PagedItemsCount != 0)
                {
                    totalPages++;
                }

                // now extrating projects of the page-[x]
                var tasksData = await tasksList
                    .Skip((paginatedDto.PageIndex - 1) * paginatedDto.PagedItemsCount)
                    .Take(paginatedDto.PagedItemsCount)
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

                // creating new dto to send the info
                PaginatedItemsDto<List<TasksDto>> res = new PaginatedItemsDto<List<TasksDto>>();
                res.Data = tasksData;
                res.TotalPages = totalPages;
                res.TotalItems = totalCount;
                return res;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<TaskCount> GetCount()
        {
            try
            {
                var query = _context.Tasks.Where(t => t.IsActive);
                var total = await query.CountAsync();
                var epic = await query.Where(t => t.TaskType == TaskType.Epic).CountAsync();
                var feature = await query.Where(t => t.TaskType == TaskType.Feature).CountAsync();
                var userstory = await query.Where(t => t.TaskType == TaskType.Userstory).CountAsync();
                var task = await query.Where(t => t.TaskType == TaskType.Task).CountAsync();
                var bug = await query.Where(t => t.TaskType == TaskType.Bug).CountAsync();

                var type = new TaskTypeCount
                {
                    Epic = epic,
                    Feature = feature,
                    UserStory = userstory,
                    Task = task,
                    Bug = bug
                };

                var pending = await query.Where(t => t.Status == TasksStatus.Pending).CountAsync();
                var active = await query.Where(t => t.Status == TasksStatus.Active).CountAsync();
                var completed = await query.Where(t => t.Status == TasksStatus.Completed).CountAsync();

                var status = new TaskStatusCount
                {
                    Pending = pending,
                    Active = active,
                    Completed = completed
                };

                var assign = await query.Where(t => t.AssignedTo != null).CountAsync();
                var unAssign = await query.Where(t => t.AssignedTo == null).CountAsync();

                var assigned = new AssignCount
                {
                    Assigned = assign,
                    UnAssigned = unAssign
                };

                var count = new TaskCount
                {
                    Total = total,
                    TypeCount = type,
                    StatusCount = status,
                    AssignCount = assigned
                };
                return count;

            }catch(Exception ex)
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
                var tasks = await query
                    .Where(t => t.IsActive)
                    .ProjectTo<TasksDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

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
                    .ProjectTo<TasksDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

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
                    .ProjectTo<TasksDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

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
                var query = _context.Tasks.Where(t => t.IsActive);

                query.Include(t => t.Parent).Include(t => t.Employee).Include(t => t.Creator);
                // check whether the task belongs to the list, then convert the task into task dto form
                /*var task = await query
                    .Select(t => new TasksDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Status = t.Status,
                        Description = t.Description,
                        AssigneeName = t.Employee.Name,
                        AssignedTo = t.AssignedTo,
                        ProjectId = t.ProjectId,
                        SprintId = t.SprintId,
                        OriginalEstimateHours = t.OriginalEstimateHours,
                        RemainingEstimateHours = t.RemainingEstimateHours,
                        AssignerName = t.Creator.Name,
                        CreatedOn = t.CreatedOn,
                        TaskType = t.TaskType

                    }).FirstOrDefaultAsync(t => t.Id == id);*/

                var taskEntity = await query.Include(t => t.Employee).FirstOrDefaultAsync(t => t.Id == id);
                if(taskEntity == null)
                {
                    return null;
                }

                var task = _mapper.Map<TasksDto>(taskEntity);


                var reviews = await _reviewService.Get(id);

                var subTasks = await _context.Tasks
                    .Where(t => t.IsActive & t.ParentId == id)
                    .ProjectTo<TaskIdAndName>(_mapper.ConfigurationProvider).ToListAsync();

                var parentId = await query.Where(t => t.Id == id).Select(t => t.ParentId).FirstAsync();

                var taskInfo = new TaskInfo
                {
                    Task = task,
                    Reviews = reviews,
                    SubTasks = subTasks
                };
                if (parentId != null)
                {
                    var parent = await _context.Tasks
                        .ProjectTo<TaskIdAndName>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(t => t.Id == parentId);
                    taskInfo.Parent = parent;
                }

                
                return taskInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool?> Update(int userId, int id, UpdateTaskDto taskDto)
        {
            try
            {
                List<AddTaskLogDto> logs = new List<AddTaskLogDto>();
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
                    if(assignedUser != null && task.AssignedTo != assignedUser.Id)
                    {
                        task.AssignedTo = assignedToId;
                        var assinedTolog = new AddTaskLogDto
                        {
                            Message = $"Task Assigned to {assignedUser.Name} by {user.Name}, on {task.UpdatedOn}",
                            TaskId = task.Id,
                        };

                        logs.Add(assinedTolog);
                    }
                }

                int? originalEstimateHours = taskDto.OriginalEstimateHours;
                int? remainingEstimateHours = taskDto.RemainingEstimateHours;
                var type = taskDto.TaskType;
                if (type == TaskType.Epic || type == TaskType.Feature || type == TaskType.Userstory)
                {
                    originalEstimateHours = null;
                    remainingEstimateHours = null;
                }
                if(originalEstimateHours != null && remainingEstimateHours != null)
                {
                    if(remainingEstimateHours > originalEstimateHours)
                    {
                        remainingEstimateHours = originalEstimateHours;
                    }
                }

                // updating the status

                if (!string.IsNullOrEmpty(taskDto.Description) && task.Description != taskDto.Description)
                {
                    task.Description = taskDto.Description;
                    var log = new AddTaskLogDto
                    {
                        Message = $"Task description changed by {user.Name} at {DateTime.Now}",
                        TaskId = id,
                    };
                    logs.Add(log);
                }

                if (!string.IsNullOrEmpty(taskDto.Name) && task.Name != taskDto.Name)
                {
                    var log = new AddTaskLogDto
                    {
                        Message = $"Task Name changed by {user.Name} - {task.Name} to {taskDto.Name} at {DateTime.Now}",
                        TaskId = id,
                    };
                    logs.Add(log);
                    task.Name = taskDto.Name;
                }

                if (taskDto.Status != null && task.Status != taskDto.Status)
                {
                    var status = taskDto.Status??TasksStatus.Pending;
                    var log = new AddTaskLogDto
                    {
                        Message = $"Task Status changed by {user.Name} - {task.Status} to {taskDto.Status} at {DateTime.Now}",
                        TaskId = id,
                    };
                    logs.Add(log);
                    task.Status = status;
                }

                if(taskDto.TaskType != null && task.TaskType != taskDto.TaskType)
                {
                    var log = new AddTaskLogDto
                    {
                        Message = $"Task Type changed by {user.Name} - {task.TaskType} to {taskDto.TaskType} at {DateTime.Now}",
                        TaskId = id,
                    };
                    logs.Add(log);
                    task.TaskType = taskDto.TaskType??TaskType.Epic;
                }

                if(taskDto.ParentId != null && task.ParentId != taskDto.ParentId)
                {
                    var parent = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskDto.ParentId);
                    
                    var checkValidParent = CheckValidParent(parent.TaskType, task.TaskType);

                    if (!checkValidParent)
                    {
                        return null;
                    }
                    
                    var log = new AddTaskLogDto
                    {
                        Message = $"Task Parent changed by {user.Name} New parent - {parent.Name} at {DateTime.Now}",
                        TaskId = id,
                    };
                    logs.Add(log);
                    task.ParentId = taskDto.ParentId;
                }

                if(taskDto.OriginalEstimateHours != null && task.OriginalEstimateHours != taskDto.OriginalEstimateHours)
                {
                    var log = new AddTaskLogDto
                    {
                        Message = $"Original Estimate hours set to {taskDto.OriginalEstimateHours} by {user.Name} at {DateTime.Now}",
                        TaskId = id,
                    };
                    logs.Add(log);
                    task.OriginalEstimateHours = originalEstimateHours;
                }
                if(taskDto.RemainingEstimateHours != null && task.RemainingEstimateHours != taskDto.RemainingEstimateHours)
                {
                    var log = new AddTaskLogDto
                    {
                        Message = $"Remaining Estimate hours changed - {task.OriginalEstimateHours} to {taskDto.RemainingEstimateHours} by {user.Name} at {DateTime.Now}",
                        TaskId = id,
                    };
                    logs.Add(log);
                    task.RemainingEstimateHours = remainingEstimateHours;
                }
                if(taskDto.SprintId != null && task.SprintId != taskDto.SprintId)
                {
                    var sprint = await _context.Sprints.FirstOrDefaultAsync(s => s.Id == taskDto.SprintId && s.projectId == task.ProjectId);

                    if(sprint != null && task.SprintId != taskDto.SprintId)
                    {
                        var log = new AddTaskLogDto
                        {
                            Message = $"Remaining Estimate hours changed - {task.SprintId} to {taskDto.SprintId} by {user.Name} at {DateTime.Now}",
                            TaskId = id,
                        };
                        task.SprintId = sprint.Id;
                        logs.Add(log);
                    }
                }
                task.UpdatedOn = DateTime.Now;
                task.UpdatedBy = userId;
                await _context.SaveChangesAsync();
                await _logService.AddMany(logs);

                return true;
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
                int? originalEstimateHours = taskDto.OriginalEstimateHours;
                var type = taskDto.TaskType;
                if(type == TaskType.Epic || type == TaskType.Feature || type == TaskType.Userstory)
                {
                    originalEstimateHours = null;
                }
                
                if (assignedToId == 0)
                {
                    var taskToAdd = _mapper.Map<Tasks>(taskDto);

                    taskToAdd.OriginalEstimateHours = originalEstimateHours;
                    taskToAdd.RemainingEstimateHours = originalEstimateHours;
                    taskToAdd.CreatedBy = adminId;

                    _context.Tasks.Add(taskToAdd);
                    await _context.SaveChangesAsync();
                    var log = new AddTaskLogDto
                    {
                        Message = $"A new task created by {taskToAdd.Creator.Name} at {taskToAdd.CreatedOn}",
                        TaskId = taskToAdd.Id
                    };
                    
                    await _context.SaveChangesAsync();
                    await _logService.Add(log);
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
                var task = _mapper.Map<Tasks>(taskDto);

                task.OriginalEstimateHours = originalEstimateHours;
                task.RemainingEstimateHours = originalEstimateHours;
                task.CreatedBy = adminId;

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
                List<AddTaskLogDto> logs = new List<AddTaskLogDto>();
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
                    int? originalEstimateHours = taskDto.OriginalEstimateHours;
                    var type = taskDto.TaskType;
                    if (type == TaskType.Epic || type == TaskType.Feature || type == TaskType.Userstory)
                    {
                        originalEstimateHours = null;
                    }
                    if (assignedToId == 0)
                    {
                        var taskToAdd = _mapper.Map<Tasks>(taskDto);

                        taskToAdd.OriginalEstimateHours = originalEstimateHours;
                        taskToAdd.RemainingEstimateHours = originalEstimateHours;
                        taskToAdd.CreatedBy = adminId;

                        _context.Tasks.Add(taskToAdd);
                        await _context.SaveChangesAsync();
                        var taskLog = new AddTaskLogDto
                        {
                            Message = $"A new task created by {taskToAdd.Creator.Name} at {taskToAdd.CreatedOn} ",
                            TaskId = taskToAdd.Id
                        };
                        logs.Add(taskLog);
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
                    if (taskDto.ProjectId != 0)
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
                    var task = _mapper.Map<Tasks>(taskDto);

                    task.OriginalEstimateHours = originalEstimateHours;
                    task.RemainingEstimateHours = originalEstimateHours;
                    task.CreatedBy = adminId;

                    // adding and updating the database
                    _context.Tasks.Add(task);
                    var log = new AddTaskLogDto
                    {
                        Message = $"A new task created by {task.Creator.Name} at {task.CreatedOn}",
                        TaskId = task.Id
                    };
                    logs.Add(log);

                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                await _logService.AddMany(logs);
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

                var log = new AddTaskLogDto
                {
                    Message = $"Task Removed by {employee.Name} at {DateTime.Now}",
                    TaskId = id
                };
                await _context.SaveChangesAsync();
                await _logService.Add(log);

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

                var log = new AddTaskLogDto
                {
                    Message = $"Task added to sprint - {sprint.Name} at {DateTime.Now}",
                    TaskId = taskId,
                };
                await _context.SaveChangesAsync();
                await _logService.Add(log);
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
                var log = new AddTaskLogDto
                {
                    Message = $"Task status changed - {task.Status} to {status} at {DateTime.Now}",
                    TaskId = id,
                };

                task.Status = status;
                
                // _context.Tasks.Remove(task);    
                await _context.SaveChangesAsync();
                await _logService.Add(log);
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
                    var log = new AddTaskLogDto
                    {
                        Message = $"Task parent updated, Parent Name - {parent.Name} at {DateTime.Now}",
                        TaskId = id,
                    };
                    await _context.SaveChangesAsync();
                    await _logService.Add(log);
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
