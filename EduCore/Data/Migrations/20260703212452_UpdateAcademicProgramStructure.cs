using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAcademicProgramStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationYears",
                table: "AcademicPrograms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProgramType",
                table: "AcademicPrograms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalParts",
                table: "AcademicPrograms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalSemesters",
                table: "AcademicPrograms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationYears",
                table: "AcademicPrograms");

            migrationBuilder.DropColumn(
                name: "ProgramType",
                table: "AcademicPrograms");

            migrationBuilder.DropColumn(
                name: "TotalParts",
                table: "AcademicPrograms");

            migrationBuilder.DropColumn(
                name: "TotalSemesters",
                table: "AcademicPrograms");
        }
    }
}
