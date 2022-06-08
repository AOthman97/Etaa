using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etaa.Data.Migrations
{
    public partial class RemoveRelationshipBetweenIdentityUserAndClearanceModel_AndChangeTypeOfBothProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clearances_IdentityUser_UserId",
                table: "Clearances");

            migrationBuilder.DropIndex(
                name: "IX_Clearances_UserId",
                table: "Clearances");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Clearances",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ManagementUserId",
                table: "Clearances",
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
                table: "Clearances",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ManagementUserId",
                table: "Clearances",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clearances_UserId",
                table: "Clearances",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clearances_IdentityUser_UserId",
                table: "Clearances",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "UserId");
        }
    }
}
