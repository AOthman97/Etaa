using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etaa.Data.Migrations
{
    public partial class EditFirstInstallmentDateToProjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstInstallmentDate",
                table: "Projects",
                newName: "FirstInstallmentDueDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstInstallmentDueDate",
                table: "Projects",
                newName: "FirstInstallmentDate");
        }
    }
}
