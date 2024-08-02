using EmployeeSystem.Contract.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Provider
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<TaskReview> TaskReviews { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }

        /*public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        entry.Entity.CreatedBy = GetCurrentUserId();
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedOn = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = GetCurrentUserId();
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        private int GetCurrentUserId()
        {
            // Implement logic to retrieve the current user's ID
            // This is just a placeholder example
            var id = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);
            return 1;  // Replace with actual user ID
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Department and Employee relationship
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee and Manager relationship
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany()
                .HasForeignKey(e => e.ManagerID)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-many relationship between Department and Employee
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentID);

            // Tasks and Employee relationships
            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.Employee)
                .WithMany()
                .HasForeignKey(t => t.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.Admin)
                .WithMany()
                .HasForeignKey(t => t.AssignedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // TaskReview and Tasks relationship
            modelBuilder.Entity<TaskReview>()
                .HasOne(tr => tr.Task)
                .WithMany()
                .HasForeignKey(tr => tr.TaskID)
                .OnDelete(DeleteBehavior.Cascade);

            // TaskReview and Employee relationship
            modelBuilder.Entity<TaskReview>()
                .HasOne(tr => tr.Reviewer)
                .WithMany()
                .HasForeignKey(tr => tr.ReviewedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Set precision and scale for the Salary property
            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<ProjectEmployee>()
                .HasKey(pe => new { pe.ProjectId, pe.EmployeeId });

            modelBuilder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Project)
                .WithMany(p => p.ProjectEmployees)
                .HasForeignKey(pe => pe.ProjectId);

            modelBuilder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Employee)
                .WithMany(e => e.ProjectEmployees)
                .HasForeignKey(pe => pe.EmployeeId);

            modelBuilder.Entity<Employee>()
            .HasMany(e => e.Attendances)
            .WithOne(a => a.Employee)
            .HasForeignKey(a => a.EmployeeId);

            // Define one-to-many relationship between Project and Employee (Admin)
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Admin)
                .WithMany() // No navigation property on the Employee side
                .HasForeignKey(p => p.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }
    }
}
