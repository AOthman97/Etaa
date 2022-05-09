using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etaa.Data.Migrations
{
    public partial class RemoveRelationshipBetweenUsersAndProjectsSecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_UsersUserId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_UsersUserId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UsersUserId",
                table: "Projects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsersUserId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UsersUserId",
                table: "Projects",
                column: "UsersUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_UsersUserId",
                table: "Projects",
                column: "UsersUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
