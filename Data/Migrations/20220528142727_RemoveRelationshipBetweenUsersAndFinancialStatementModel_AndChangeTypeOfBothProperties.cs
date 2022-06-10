using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etaa.Data.Migrations
{
    public partial class RemoveRelationshipBetweenIdentityUserAndFinancialStatementModel_AndChangeTypeOfBothProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<string>(
                name: "DocumentPath",
                table: "FinancialStatements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DocumentPath",
                table: "FinancialStatements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
