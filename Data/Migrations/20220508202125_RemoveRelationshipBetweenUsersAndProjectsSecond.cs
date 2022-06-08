using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etaa.Data.Migrations
{
    public partial class RemoveRelationshipBetweenIdentityUserAndProjectsSecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_IdentityUser_IdentityUserUserId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_IdentityUserUserId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IdentityUserUserId",
                table: "Projects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdentityUserUserId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_IdentityUserUserId",
                table: "Projects",
                column: "IdentityUserUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_IdentityUser_IdentityUserUserId",
                table: "Projects",
                column: "IdentityUserUserId",
                principalTable: "IdentityUser",
                principalColumn: "UserId");
        }
    }
}
