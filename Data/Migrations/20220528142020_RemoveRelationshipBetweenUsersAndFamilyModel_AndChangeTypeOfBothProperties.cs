using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etaa.Data.Migrations
{
    public partial class RemoveRelationshipBetweenIdentityUserAndFamilyModel_AndChangeTypeOfBothProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Families_IdentityUser_UserId",
                table: "Families");

            migrationBuilder.DropIndex(
                name: "IX_Families_UserId",
                table: "Families");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Families",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ManagementUserId",
                table: "Families",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Families",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ManagementUserId",
                table: "Families",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Families_UserId",
                table: "Families",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_IdentityUser_UserId",
                table: "Families",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "UserId");
        }
    }
}
