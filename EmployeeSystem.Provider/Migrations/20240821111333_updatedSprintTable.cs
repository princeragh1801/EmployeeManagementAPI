using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeSystem.Provider.Migrations
{
    /// <inheritdoc />
    public partial class updatedSprintTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "projectId",
                table: "Sprints",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sprints_projectId",
                table: "Sprints",
                column: "projectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sprints_Projects_projectId",
                table: "Sprints",
                column: "projectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sprints_Projects_projectId",
                table: "Sprints");

            migrationBuilder.DropIndex(
                name: "IX_Sprints_projectId",
                table: "Sprints");

            migrationBuilder.DropColumn(
                name: "projectId",
                table: "Sprints");
        }
    }
}
