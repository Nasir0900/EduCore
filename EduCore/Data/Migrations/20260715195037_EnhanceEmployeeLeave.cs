using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceEmployeeLeave : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDays",
                table: "EmployeeLeaves",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "EmployeeLeaves",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualRejoiningDate",
                table: "EmployeeLeaves",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "EmployeeLeaves",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledDate",
                table: "EmployeeLeaves",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedRejoiningDate",
                table: "EmployeeLeaves",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "EmployeeLeaves",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHalfDay",
                table: "EmployeeLeaves",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualRejoiningDate",
                table: "EmployeeLeaves");

            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "EmployeeLeaves");

            migrationBuilder.DropColumn(
                name: "CancelledDate",
                table: "EmployeeLeaves");

            migrationBuilder.DropColumn(
                name: "ExpectedRejoiningDate",
                table: "EmployeeLeaves");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "EmployeeLeaves");

            migrationBuilder.DropColumn(
                name: "IsHalfDay",
                table: "EmployeeLeaves");

            migrationBuilder.AlterColumn<int>(
                name: "TotalDays",
                table: "EmployeeLeaves",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "EmployeeLeaves",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);
        }
    }
}
