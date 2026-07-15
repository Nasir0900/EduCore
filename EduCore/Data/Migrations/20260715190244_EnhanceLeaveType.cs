using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceLeaveType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowBackDateApplication",
                table: "LeaveTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowCarryForward",
                table: "LeaveTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AvailableAfterMonths",
                table: "LeaveTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsYearlyLimit",
                table: "LeaveTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresApproval",
                table: "LeaveTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresDocuments",
                table: "LeaveTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowBackDateApplication",
                table: "LeaveTypes");

            migrationBuilder.DropColumn(
                name: "AllowCarryForward",
                table: "LeaveTypes");

            migrationBuilder.DropColumn(
                name: "AvailableAfterMonths",
                table: "LeaveTypes");

            migrationBuilder.DropColumn(
                name: "IsYearlyLimit",
                table: "LeaveTypes");

            migrationBuilder.DropColumn(
                name: "RequiresApproval",
                table: "LeaveTypes");

            migrationBuilder.DropColumn(
                name: "RequiresDocuments",
                table: "LeaveTypes");
        }
    }
}
