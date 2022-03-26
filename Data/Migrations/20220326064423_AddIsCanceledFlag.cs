using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etaa.Data.Migrations
{
    public partial class AddIsCanceledFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMembers_Families_FamilyId",
                table: "FamilyMembers");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "States",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Religions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "ProjectTypesAssets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "ProjectTypes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "ProjectSocialBenefits",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "ProjectSelectionReasons",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "ProjectsAssets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Projects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "ProjectGroups",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "ProjectDomainTypes",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsApprovedByManagement",
                table: "PaymentVouchers",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "PaymentVouchers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "NumberOfFunds",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "MartialStatuses",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Kinships",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Jobs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "InvestmentTypes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Installments",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "HealthStatuses",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Genders",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "FinancialStatements",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FamilyId",
                table: "FamilyMembers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "FamilyMembers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Families",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "EducationalStatuses",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Districts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Contributors",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Clearances",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Cities",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "AccommodationTypes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMembers_Families_FamilyId",
                table: "FamilyMembers",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "FamilyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMembers_Families_FamilyId",
                table: "FamilyMembers");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "States");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Religions");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "ProjectTypesAssets");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "ProjectTypes");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "ProjectSocialBenefits");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "ProjectSelectionReasons");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "ProjectsAssets");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "ProjectGroups");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "ProjectDomainTypes");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "PaymentVouchers");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "NumberOfFunds");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "MartialStatuses");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Kinships");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "InvestmentTypes");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Installments");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "HealthStatuses");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Genders");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "FinancialStatements");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "FamilyMembers");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "EducationalStatuses");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Contributors");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Clearances");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "AccommodationTypes");

            migrationBuilder.AlterColumn<bool>(
                name: "IsApprovedByManagement",
                table: "PaymentVouchers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FamilyId",
                table: "FamilyMembers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMembers_Families_FamilyId",
                table: "FamilyMembers",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "FamilyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
