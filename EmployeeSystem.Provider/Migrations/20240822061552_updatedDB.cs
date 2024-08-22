using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeSystem.Provider.Migrations
{
    /// <inheritdoc />
    public partial class updatedDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Users_UserId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Employees_AdminId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Employees_RequestedBy",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskReviews_Employees_ReviewedBy",
                table: "TaskReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Employees_AssignedBy",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AssignedBy",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_TaskReviews_ReviewedBy",
                table: "TaskReviews");

            migrationBuilder.DropIndex(
                name: "IX_Requests_RequestedBy",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Projects_AdminId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AssignedBy",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "UpdatedByName",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "TaskReviews");

            migrationBuilder.DropColumn(
                name: "ReviewedBy",
                table: "TaskReviews");

            migrationBuilder.DropColumn(
                name: "UpdatedByName",
                table: "TaskReviews");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestedBy",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "UpdatedByName",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UpdatedByName",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UpdatedByName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "UpdatedByName",
                table: "Departments");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Tasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "TaskReviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Sprints",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Sprints",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Sprints",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Sprints",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Requests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Projects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Departments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatedBy",
                table: "Tasks",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UpdatedBy",
                table: "Tasks",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskReviews_CreatedBy",
                table: "TaskReviews",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskReviews_UpdatedBy",
                table: "TaskReviews",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Sprints_CreatedBy",
                table: "Sprints",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Sprints_UpdatedBy",
                table: "Sprints",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CreatedBy",
                table: "Requests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UpdatedBy",
                table: "Requests",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatedBy",
                table: "Projects",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UpdatedBy",
                table: "Projects",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatedBy",
                table: "Employees",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UpdatedBy",
                table: "Employees",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CreatedBy",
                table: "Departments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_UpdatedBy",
                table: "Departments",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_CreatedBy",
                table: "Departments",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_UpdatedBy",
                table: "Departments",
                column: "UpdatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_CreatedBy",
                table: "Employees",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_UpdatedBy",
                table: "Employees",
                column: "UpdatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Employees_CreatedBy",
                table: "Projects",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Employees_UpdatedBy",
                table: "Projects",
                column: "UpdatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Employees_CreatedBy",
                table: "Requests",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Employees_UpdatedBy",
                table: "Requests",
                column: "UpdatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sprints_Employees_CreatedBy",
                table: "Sprints",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sprints_Employees_UpdatedBy",
                table: "Sprints",
                column: "UpdatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskReviews_Employees_CreatedBy",
                table: "TaskReviews",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskReviews_Employees_UpdatedBy",
                table: "TaskReviews",
                column: "UpdatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Employees_CreatedBy",
                table: "Tasks",
                column: "CreatedBy",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Employees_UpdatedBy",
                table: "Tasks",
                column: "UpdatedBy",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_CreatedBy",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_UpdatedBy",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_CreatedBy",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_UpdatedBy",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Employees_CreatedBy",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Employees_UpdatedBy",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Employees_CreatedBy",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Employees_UpdatedBy",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Sprints_Employees_CreatedBy",
                table: "Sprints");

            migrationBuilder.DropForeignKey(
                name: "FK_Sprints_Employees_UpdatedBy",
                table: "Sprints");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskReviews_Employees_CreatedBy",
                table: "TaskReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskReviews_Employees_UpdatedBy",
                table: "TaskReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Employees_CreatedBy",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Employees_UpdatedBy",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_CreatedBy",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_UpdatedBy",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_TaskReviews_CreatedBy",
                table: "TaskReviews");

            migrationBuilder.DropIndex(
                name: "IX_TaskReviews_UpdatedBy",
                table: "TaskReviews");

            migrationBuilder.DropIndex(
                name: "IX_Sprints_CreatedBy",
                table: "Sprints");

            migrationBuilder.DropIndex(
                name: "IX_Sprints_UpdatedBy",
                table: "Sprints");

            migrationBuilder.DropIndex(
                name: "IX_Requests_CreatedBy",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_UpdatedBy",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CreatedBy",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_UpdatedBy",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CreatedBy",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UpdatedBy",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Departments_CreatedBy",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_UpdatedBy",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Sprints");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Sprints");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Sprints");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Sprints");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedBy",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByName",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "TaskReviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "TaskReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReviewedBy",
                table: "TaskReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByName",
                table: "TaskReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestedBy",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByName",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByName",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByName",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedBy",
                table: "Tasks",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TaskReviews_ReviewedBy",
                table: "TaskReviews",
                column: "ReviewedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestedBy",
                table: "Requests",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AdminId",
                table: "Projects",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Users_UserId",
                table: "Employees",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Employees_AdminId",
                table: "Projects",
                column: "AdminId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Employees_RequestedBy",
                table: "Requests",
                column: "RequestedBy",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskReviews_Employees_ReviewedBy",
                table: "TaskReviews",
                column: "ReviewedBy",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Employees_AssignedBy",
                table: "Tasks",
                column: "AssignedBy",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
