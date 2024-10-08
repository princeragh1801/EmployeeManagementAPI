﻿using EmployeeSystem.Provider.Services;

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
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISprintService, SprintService>();
            services.AddScoped<ITaskLogService, TaskLogsService>();
            services.AddScoped<IProjectEmployeeService, ProjectEmployeeService>();
            services.AddSingleton<IUtilityService, UtilityService>();
            services.AddSingleton<ICloudinaryService, CloudinaryService>();
            services.AddScoped<ITestService, TestService>();
            services.AddSingleton<IEmailService, EmailService>();
            return services;
        }
    }
}
