using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etaa.Data.Migrations
{
    public partial class RemoveRelationshipBetweenIdentityUserAndPaymentVoucherModel_AndChangeTypeOfBothProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentVouchers_IdentityUser_UserId",
                table: "PaymentVouchers");

            migrationBuilder.DropIndex(
                name: "IX_PaymentVouchers_UserId",
                table: "PaymentVouchers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PaymentVouchers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ManagementUserId",
                table: "PaymentVouchers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PaymentVouchers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ManagementUserId",
                table: "PaymentVouchers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_UserId",
                table: "PaymentVouchers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentVouchers_IdentityUser_UserId",
                table: "PaymentVouchers",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
