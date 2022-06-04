using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etaa.Data.Migrations
{
    public partial class AddViewToIncludeTheProjectAssetsAndProjectTypeAssets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                create view ProjectAssetesProjectTypeAssets as
                SELECT dbo.ProjectTypesAssets.NameAr, dbo.ProjectTypesAssets.NameEn, dbo.ProjectTypesAssets.ProjectTypeId, dbo.ProjectsAssets.Quantity, dbo.ProjectsAssets.Amount, dbo.ProjectsAssets.ProjectTypesAssetsId, dbo.Projects.ProjectId
                FROM dbo.ProjectTypesAssets INNER JOIN
                dbo.ProjectsAssets ON dbo.ProjectTypesAssets.ProjectTypesAssetsId = dbo.ProjectsAssets.ProjectTypesAssetsId INNER JOIN
                dbo.Projects ON dbo.ProjectsAssets.ProjectId = dbo.Projects.ProjectId
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                drop view ProjectAssetesProjectTypeAssets;
                ");
        }
    }
}
