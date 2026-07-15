using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LeaveNumber",
                table: "EmployeeLeaves",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveNumber",
                table: "EmployeeLeaves");
        }
    }
}
