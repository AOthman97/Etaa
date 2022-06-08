using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etaa.Data.Migrations
{
    public partial class SetSomeColumnsAsNullableInFamily : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Families_AccommodationTypes_AccommodationTypeId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_EducationalStatuses_EducationalStatusId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_HealthStatuses_HealthStatusId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_InvestmentTypes_InvestmentTypeId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_Jobs_JobId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_MartialStatuses_MartialStatusId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_IdentityUser_UserId",
                table: "Families");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Families",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MartialStatusId",
                table: "Families",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ManagementUserId",
                table: "Families",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "Families",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "InvestmentTypeId",
                table: "Families",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "HealthStatusId",
                table: "Families",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EducationalStatusId",
                table: "Families",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AccommodationTypeId",
                table: "Families",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_AccommodationTypes_AccommodationTypeId",
                table: "Families",
                column: "AccommodationTypeId",
                principalTable: "AccommodationTypes",
                principalColumn: "AccommodationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_EducationalStatuses_EducationalStatusId",
                table: "Families",
                column: "EducationalStatusId",
                principalTable: "EducationalStatuses",
                principalColumn: "EducationalStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_HealthStatuses_HealthStatusId",
                table: "Families",
                column: "HealthStatusId",
                principalTable: "HealthStatuses",
                principalColumn: "HealthStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_InvestmentTypes_InvestmentTypeId",
                table: "Families",
                column: "InvestmentTypeId",
                principalTable: "InvestmentTypes",
                principalColumn: "InvestmentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Jobs_JobId",
                table: "Families",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_MartialStatuses_MartialStatusId",
                table: "Families",
                column: "MartialStatusId",
                principalTable: "MartialStatuses",
                principalColumn: "MartialStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_IdentityUser_UserId",
                table: "Families",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Families_AccommodationTypes_AccommodationTypeId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_EducationalStatuses_EducationalStatusId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_HealthStatuses_HealthStatusId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_InvestmentTypes_InvestmentTypeId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_Jobs_JobId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_MartialStatuses_MartialStatusId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_IdentityUser_UserId",
                table: "Families");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Families",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MartialStatusId",
                table: "Families",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ManagementUserId",
                table: "Families",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "Families",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InvestmentTypeId",
                table: "Families",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HealthStatusId",
                table: "Families",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EducationalStatusId",
                table: "Families",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccommodationTypeId",
                table: "Families",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Families_AccommodationTypes_AccommodationTypeId",
                table: "Families",
                column: "AccommodationTypeId",
                principalTable: "AccommodationTypes",
                principalColumn: "AccommodationTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Families_EducationalStatuses_EducationalStatusId",
                table: "Families",
                column: "EducationalStatusId",
                principalTable: "EducationalStatuses",
                principalColumn: "EducationalStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Families_HealthStatuses_HealthStatusId",
                table: "Families",
                column: "HealthStatusId",
                principalTable: "HealthStatuses",
                principalColumn: "HealthStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Families_InvestmentTypes_InvestmentTypeId",
                table: "Families",
                column: "InvestmentTypeId",
                principalTable: "InvestmentTypes",
                principalColumn: "InvestmentTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Jobs_JobId",
                table: "Families",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Families_MartialStatuses_MartialStatusId",
                table: "Families",
                column: "MartialStatusId",
                principalTable: "MartialStatuses",
                principalColumn: "MartialStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Families_IdentityUser_UserId",
                table: "Families",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
