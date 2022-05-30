using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etaa.Data.Migrations
{
    public partial class EditTheLogTableToMatchWithSerilog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_EventTypes_EventTypeId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Modules_ModuleId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "PageName",
                table: "Logs");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Logs",
                newName: "TimeStamp");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Logs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EventTypeId",
                table: "Logs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Exception",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageTemplate",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_EventTypes_EventTypeId",
                table: "Logs",
                column: "EventTypeId",
                principalTable: "EventTypes",
                principalColumn: "EventTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Modules_ModuleId",
                table: "Logs",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_EventTypes_EventTypeId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Modules_ModuleId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Exception",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "MessageTemplate",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "Logs");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "Logs",
                newName: "Date");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventTypeId",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PageName",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_EventTypes_EventTypeId",
                table: "Logs",
                column: "EventTypeId",
                principalTable: "EventTypes",
                principalColumn: "EventTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Modules_ModuleId",
                table: "Logs",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
