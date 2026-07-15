using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EduCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedLeaveTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "LeaveTypes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "LeaveTypeId", "AllowBackDateApplication", "AllowCarryForward", "AvailableAfterMonths", "Description", "DisplayOrder", "IsActive", "IsPaidLeave", "IsYearlyLimit", "LeaveCode", "LeaveTypeName", "MaximumDays", "RequiresApproval", "RequiresDocuments" },
                values: new object[,]
                {
                    { 1, false, false, 0, null, 1, true, true, true, "CL", "Casual Leave", 10, true, false },
                    { 2, false, true, 12, null, 2, true, true, true, "AL", "Annual Leave", 30, true, false },
                    { 3, true, false, 0, null, 3, true, true, true, "ML", "Medical Leave", 20, true, true },
                    { 4, false, false, 24, null, 4, true, true, false, "SL", "Study Leave", 365, true, true },
                    { 5, false, false, 0, null, 5, true, true, false, "MAT", "Maternity Leave", 180, true, true },
                    { 6, false, false, 0, null, 6, true, true, false, "PAT", "Paternity Leave", 15, true, false },
                    { 7, false, false, 0, null, 7, true, false, false, "EOL", "Extraordinary Leave", 365, true, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "LeaveTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "LeaveTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "LeaveTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "LeaveTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "LeaveTypeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "LeaveTypeId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "LeaveTypeId",
                keyValue: 7);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "LeaveTypes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");
        }
    }
}
