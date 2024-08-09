using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Provider.Services;

namespace EmployeeSystemWebApi.Extention
{
    public static class AddServices
    {
        public static IServiceCollection AddCustomerServices (this IServiceCollection services)
        {
            // adding other services
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITaskReviewService, TaskReviewService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IAuthService, Authservice>();
            services.AddScoped<IPaginatedService, PaginatedService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
