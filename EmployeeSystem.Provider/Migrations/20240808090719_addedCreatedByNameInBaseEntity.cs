using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeSystem.Provider.Migrations
{
    /// <inheritdoc />
    public partial class addedCreatedByNameInBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "TaskReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByName",
                table: "TaskReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByName",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedByName",
                table: "Users");

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
                name: "UpdatedByName",
                table: "TaskReviews");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "UpdatedByName",
                table: "Requests");

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
                name: "CreatedByName",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "UpdatedByName",
                table: "Departments");
        }
    }
}
