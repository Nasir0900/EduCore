using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorDepartmentRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicPrograms_Faculties_FacultyId",
                table: "AcademicPrograms");

            migrationBuilder.RenameColumn(
                name: "FacultyId",
                table: "AcademicPrograms",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_AcademicPrograms_FacultyId",
                table: "AcademicPrograms",
                newName: "IX_AcademicPrograms_DepartmentId");

            migrationBuilder.AddColumn<int>(
                name: "FacultyId",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_FacultyId",
                table: "Departments",
                column: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicPrograms_Departments_DepartmentId",
                table: "AcademicPrograms",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Faculties_FacultyId",
                table: "Departments",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicPrograms_Departments_DepartmentId",
                table: "AcademicPrograms");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Faculties_FacultyId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_FacultyId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "Departments");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "AcademicPrograms",
                newName: "FacultyId");

            migrationBuilder.RenameIndex(
                name: "IX_AcademicPrograms_DepartmentId",
                table: "AcademicPrograms",
                newName: "IX_AcademicPrograms_FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicPrograms_Faculties_FacultyId",
                table: "AcademicPrograms",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
