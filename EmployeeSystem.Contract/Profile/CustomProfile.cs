using AutoMapper;

namespace EmployeeSystem.Contract.Prof
{

    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            // map for project
            CreateMap<ProjectEmployee, ProjectEmployeeDto>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.Name))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Employee.ImageUrl));

            CreateMap<ProjectEmployee, EmployeeIdAndName>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Employee.Name))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Employee.Department.Name));



            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Creator.Name))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn));

            CreateMap<AddProjectDto, Project>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
               .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
               .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
               .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.Now));

            // map for employee
            CreateMap<Employee, EmployeePaginationInfo>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager.Name))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));

            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager.Name))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentID)) // Assuming `DepartmentId` is a direct property
                .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.ManagerID)) // Assuming `ManagerId` is a direct property
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Creator != null ? src.Creator.Name : ""))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(src => src.UpdatedOn));

            CreateMap<Employee, EmployeeInfo>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? src.Manager.Name : null))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Creator != null ? src.Creator.Name : ""));


            CreateMap<AddEmployeeDto, Employee>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.DepartmentID, opt => opt.MapFrom(src => src.DepartmentID == 0 ? (int?)null : src.DepartmentID))
                .ForMember(dest => dest.ManagerID, opt => opt.MapFrom(src => src.ManagerID == 0 ? (int?)null : src.ManagerID))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.Now)) // Consider handling this manually
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())// Handle manually if needed
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());// Handle manually if needed

            CreateMap<UpdateEmployeeDto, Employee>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.DepartmentID, opt => opt.MapFrom(src => src.DepartmentID == 0 ? (int?)null : src.DepartmentID))
                .ForMember(dest => dest.ManagerID, opt => opt.MapFrom(src => src.ManagerID == 0 ? (int?)null : src.ManagerID))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())// Handle manually if needed
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())// Handle manually if needed
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore()) // Assuming UpdatedBy is set separately
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(src => DateTime.Now)); // Sets UpdatedOn to the current date and time


            CreateMap<Employee, EmployeeIdAndName>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));
            // map for department
            CreateMap<Department, DepartmentPaginationInfo>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Creator.Name));

            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Creator.Name));
            

            // map for tasks
            CreateMap<Tasks, TasksDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.TaskType, opt => opt.MapFrom(src => src.TaskType))
                .ForMember(dest => dest.AssigneeName, opt => opt.MapFrom(src => src.Employee.Name))
                .ForMember(dest => dest.AssignerName, opt => opt.MapFrom(src => src.Creator.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => TasksStatus.Pending))
                .ForMember(dest => dest.SprintId, opt => opt.MapFrom(src => src.SprintId == 0 ? (int?)null : src.SprintId))
                .ForMember(dest => dest.OriginalEstimateHours, opt => opt.Ignore())
                .ForMember(dest => dest.RemainingEstimateHours, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<Tasks, TaskIdAndName>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.TaskType, opt => opt.MapFrom(src => src.TaskType));


            CreateMap<AddTaskDto, Tasks>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.TaskType, opt => opt.MapFrom(src => src.TaskType))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedTo == 0 ? (int?)null : src.AssignedTo))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => TasksStatus.Pending))
                .ForMember(dest => dest.SprintId, opt => opt.MapFrom(src => src.SprintId == 0 ? (int?)null : src.SprintId))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId == 0 ? (int?)null : src.ParentId))
                .ForMember(dest => dest.OriginalEstimateHours, opt => opt.Ignore())
                .ForMember(dest => dest.RemainingEstimateHours, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<Tasks, UpdateTaskDto>();

            CreateMap<UpdateTaskDto, Tasks>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.TaskType, opt => opt.MapFrom(src => src.TaskType))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedTo == 0 ? (int?)null : src.AssignedTo))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => TasksStatus.Pending))
                .ForMember(dest => dest.SprintId, opt => opt.MapFrom(src => src.SprintId == 0 ? (int?)null : src.SprintId))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId == 0 ? (int?)null : src.ParentId))
                .ForMember(dest => dest.OriginalEstimateHours, opt => opt.Ignore())
                .ForMember(dest => dest.RemainingEstimateHours, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(_ => DateTime.Now));
        }
    }
}
