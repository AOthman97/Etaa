using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etaa.Data.Migrations
{
    public partial class Etaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccommodationTypes",
                columns: table => new
                {
                    AccommodationTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationTypes", x => x.AccommodationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "EducationalStatuses",
                columns: table => new
                {
                    EducationalStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationalStatuses", x => x.EducationalStatusId);
                });

            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    EventTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsEvent = table.Column<bool>(type: "bit", nullable: false),
                    IsException = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.EventTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    GenderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMale = table.Column<bool>(type: "bit", nullable: false),
                    IsFemale = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.GenderId);
                });

            migrationBuilder.CreateTable(
                name: "HealthStatuses",
                columns: table => new
                {
                    HealthStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthStatuses", x => x.HealthStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Installments",
                columns: table => new
                {
                    InstallmentsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstallmentNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Installments", x => x.InstallmentsId);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentTypes",
                columns: table => new
                {
                    InvestmentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentTypes", x => x.InvestmentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.JobId);
                });

            migrationBuilder.CreateTable(
                name: "Kinships",
                columns: table => new
                {
                    KinshipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kinships", x => x.KinshipId);
                });

            migrationBuilder.CreateTable(
                name: "MartialStatuses",
                columns: table => new
                {
                    MartialStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MartialStatuses", x => x.MartialStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    ModuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.ModuleId);
                });

            migrationBuilder.CreateTable(
                name: "NumberOfFunds",
                columns: table => new
                {
                    NumberOfFundsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    MinAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberOfFunds", x => x.NumberOfFundsId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDomainTypes",
                columns: table => new
                {
                    ProjectDomainTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDomainTypes", x => x.ProjectDomainTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectGroups",
                columns: table => new
                {
                    ProjectGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectGroups", x => x.ProjectGroupId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectSelectionReasons",
                columns: table => new
                {
                    ProjectSelectionReasonsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSelectionReasons", x => x.ProjectSelectionReasonsId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectSocialBenefits",
                columns: table => new
                {
                    ProjectSocialBenefitsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSocialBenefits", x => x.ProjectSocialBenefitsId);
                });

            migrationBuilder.CreateTable(
                name: "Religions",
                columns: table => new
                {
                    ReligionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Religions", x => x.ReligionId);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    StateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.StateId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTypes",
                columns: table => new
                {
                    ProjectTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectDomainTypeId = table.Column<int>(type: "int", nullable: false),
                    ProjectGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTypes", x => x.ProjectTypeId);
                    table.ForeignKey(
                        name: "FK_ProjectTypes_ProjectDomainTypes_ProjectDomainTypeId",
                        column: x => x.ProjectDomainTypeId,
                        principalTable: "ProjectDomainTypes",
                        principalColumn: "ProjectDomainTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTypes_ProjectGroups_ProjectGroupId",
                        column: x => x.ProjectGroupId,
                        principalTable: "ProjectGroups",
                        principalColumn: "ProjectGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    EventTypeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_Logs_EventTypes_EventTypeId",
                        column: x => x.EventTypeId,
                        principalTable: "EventTypes",
                        principalColumn: "EventTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Logs_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Logs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTypesAssets",
                columns: table => new
                {
                    ProjectTypesAssetsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTypesAssets", x => x.ProjectTypesAssetsId);
                    table.ForeignKey(
                        name: "FK_ProjectTypesAssets_ProjectTypes_ProjectTypeId",
                        column: x => x.ProjectTypeId,
                        principalTable: "ProjectTypes",
                        principalColumn: "ProjectTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.DistrictId);
                    table.ForeignKey(
                        name: "FK_Districts_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contributors",
                columns: table => new
                {
                    ContributorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatsappMobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MonthlyShareAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NumberOfShares = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributors", x => x.ContributorId);
                    table.ForeignKey(
                        name: "FK_Contributors_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clearances",
                columns: table => new
                {
                    ClearanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClearanceDocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClearanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApprovedByManagement = table.Column<bool>(type: "bit", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ManagementUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clearances", x => x.ClearanceId);
                    table.ForeignKey(
                        name: "FK_Clearances_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    FamilyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alleyway = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResidentialSquare = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfIndividuals = table.Column<int>(type: "int", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    MonthlyIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsCurrentInvestmentProject = table.Column<bool>(type: "bit", nullable: true),
                    IsApprovedByManagement = table.Column<bool>(type: "bit", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: false),
                    ReligionId = table.Column<int>(type: "int", nullable: false),
                    MartialStatusId = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    HealthStatusId = table.Column<int>(type: "int", nullable: false),
                    EducationalStatusId = table.Column<int>(type: "int", nullable: false),
                    AccommodationTypeId = table.Column<int>(type: "int", nullable: false),
                    InvestmentTypeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ManagementUserId = table.Column<int>(type: "int", nullable: false),
                    ProjectsProjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.FamilyId);
                    table.ForeignKey(
                        name: "FK_Families_AccommodationTypes_AccommodationTypeId",
                        column: x => x.AccommodationTypeId,
                        principalTable: "AccommodationTypes",
                        principalColumn: "AccommodationTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Families_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Families_EducationalStatuses_EducationalStatusId",
                        column: x => x.EducationalStatusId,
                        principalTable: "EducationalStatuses",
                        principalColumn: "EducationalStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Families_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "GenderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Families_HealthStatuses_HealthStatusId",
                        column: x => x.HealthStatusId,
                        principalTable: "HealthStatuses",
                        principalColumn: "HealthStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Families_InvestmentTypes_InvestmentTypeId",
                        column: x => x.InvestmentTypeId,
                        principalTable: "InvestmentTypes",
                        principalColumn: "InvestmentTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Families_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Families_MartialStatuses_MartialStatusId",
                        column: x => x.MartialStatusId,
                        principalTable: "MartialStatuses",
                        principalColumn: "MartialStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Families_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "ReligionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Families_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilyMembers",
                columns: table => new
                {
                    FamilyMemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KinshipId = table.Column<int>(type: "int", nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: false),
                    EducationalStatusId = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    FamilyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyMembers", x => x.FamilyMemberId);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_EducationalStatuses_EducationalStatusId",
                        column: x => x.EducationalStatusId,
                        principalTable: "EducationalStatuses",
                        principalColumn: "EducationalStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "FamilyId",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "GenderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_Kinships_KinshipId",
                        column: x => x.KinshipId,
                        principalTable: "Kinships",
                        principalColumn: "KinshipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignatureofApplicantPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectActivity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectPurpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capital = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MonthlyInstallmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NumberOfInstallments = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WaiverPeriod = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsApprovedByManagement = table.Column<bool>(type: "bit", nullable: true),
                    FamilyId = table.Column<int>(type: "int", nullable: false),
                    NumberOfFundsId = table.Column<int>(type: "int", nullable: false),
                    ProjectTypeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ManagementUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Projects_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "FamilyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_NumberOfFunds_NumberOfFundsId",
                        column: x => x.NumberOfFundsId,
                        principalTable: "NumberOfFunds",
                        principalColumn: "NumberOfFundsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectTypes_ProjectTypeId",
                        column: x => x.ProjectTypeId,
                        principalTable: "ProjectTypes",
                        principalColumn: "ProjectTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FinancialStatements",
                columns: table => new
                {
                    FinancialStatementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsApprovedByManagement = table.Column<bool>(type: "bit", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ManagementUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialStatements", x => x.FinancialStatementId);
                    table.ForeignKey(
                        name: "FK_FinancialStatements_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialStatements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PaymentVouchers",
                columns: table => new
                {
                    PaymentVoucherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentDocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsApprovedByManagement = table.Column<bool>(type: "bit", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    InstallmentsId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ManagementUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentVouchers", x => x.PaymentVoucherId);
                    table.ForeignKey(
                        name: "FK_PaymentVouchers_Installments_InstallmentsId",
                        column: x => x.InstallmentsId,
                        principalTable: "Installments",
                        principalColumn: "InstallmentsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentVouchers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentVouchers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProjectsAssets",
                columns: table => new
                {
                    ProjectsAssetsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ProjectTypesAssetsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsAssets", x => x.ProjectsAssetsId);
                    table.ForeignKey(
                        name: "FK_ProjectsAssets_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectsAssets_ProjectTypesAssets_ProjectTypesAssetsId",
                        column: x => x.ProjectTypesAssetsId,
                        principalTable: "ProjectTypesAssets",
                        principalColumn: "ProjectTypesAssetsId",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProjectsSelectionReasons",
                columns: table => new
                {
                    ProjectsSelectionReasonsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ProjectSelectionReasonsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsSelectionReasons", x => x.ProjectsSelectionReasonsId);
                    table.ForeignKey(
                        name: "FK_ProjectsSelectionReasons_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectsSelectionReasons_ProjectSelectionReasons_ProjectSelectionReasonsId",
                        column: x => x.ProjectSelectionReasonsId,
                        principalTable: "ProjectSelectionReasons",
                        principalColumn: "ProjectSelectionReasonsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectsSocialBenefits",
                columns: table => new
                {
                    ProjectsSocialBenefitsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ProjectSocialBenefitsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsSocialBenefits", x => x.ProjectsSocialBenefitsId);
                    table.ForeignKey(
                        name: "FK_ProjectsSocialBenefits_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectsSocialBenefits_ProjectSocialBenefits_ProjectSocialBenefitsId",
                        column: x => x.ProjectSocialBenefitsId,
                        principalTable: "ProjectSocialBenefits",
                        principalColumn: "ProjectSocialBenefitsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Clearances_ProjectId",
                table: "Clearances",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Clearances_UserId",
                table: "Clearances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Contributors_DistrictId",
                table: "Contributors",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CityId",
                table: "Districts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_AccommodationTypeId",
                table: "Families",
                column: "AccommodationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_DistrictId",
                table: "Families",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_EducationalStatusId",
                table: "Families",
                column: "EducationalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_GenderId",
                table: "Families",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_HealthStatusId",
                table: "Families",
                column: "HealthStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_InvestmentTypeId",
                table: "Families",
                column: "InvestmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_JobId",
                table: "Families",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_MartialStatusId",
                table: "Families",
                column: "MartialStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_ProjectsProjectId",
                table: "Families",
                column: "ProjectsProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_ReligionId",
                table: "Families",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_UserId",
                table: "Families",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_EducationalStatusId",
                table: "FamilyMembers",
                column: "EducationalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_FamilyId",
                table: "FamilyMembers",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_GenderId",
                table: "FamilyMembers",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_JobId",
                table: "FamilyMembers",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_KinshipId",
                table: "FamilyMembers",
                column: "KinshipId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialStatements_ProjectId",
                table: "FinancialStatements",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialStatements_UserId",
                table: "FinancialStatements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_EventTypeId",
                table: "Logs",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_ModuleId",
                table: "Logs",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UserId",
                table: "Logs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_InstallmentsId",
                table: "PaymentVouchers",
                column: "InstallmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_ProjectId",
                table: "PaymentVouchers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_UserId",
                table: "PaymentVouchers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FamilyId",
                table: "Projects",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_NumberOfFundsId",
                table: "Projects",
                column: "NumberOfFundsId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectTypeId",
                table: "Projects",
                column: "ProjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserId",
                table: "Projects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsAssets_ProjectId",
                table: "ProjectsAssets",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsAssets_ProjectTypesAssetsId",
                table: "ProjectsAssets",
                column: "ProjectTypesAssetsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsSelectionReasons_ProjectId",
                table: "ProjectsSelectionReasons",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsSelectionReasons_ProjectSelectionReasonsId",
                table: "ProjectsSelectionReasons",
                column: "ProjectSelectionReasonsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsSocialBenefits_ProjectId",
                table: "ProjectsSocialBenefits",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsSocialBenefits_ProjectSocialBenefitsId",
                table: "ProjectsSocialBenefits",
                column: "ProjectSocialBenefitsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTypes_ProjectDomainTypeId",
                table: "ProjectTypes",
                column: "ProjectDomainTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTypes_ProjectGroupId",
                table: "ProjectTypes",
                column: "ProjectGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTypesAssets_ProjectTypeId",
                table: "ProjectTypesAssets",
                column: "ProjectTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clearances_Projects_ProjectId",
                table: "Clearances",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.NoAction,
                onUpdate: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Projects_ProjectsProjectId",
                table: "Families",
                column: "ProjectsProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_States_StateId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_Projects_ProjectsProjectId",
                table: "Families");

            migrationBuilder.DropTable(
                name: "Clearances");

            migrationBuilder.DropTable(
                name: "Contributors");

            migrationBuilder.DropTable(
                name: "FamilyMembers");

            migrationBuilder.DropTable(
                name: "FinancialStatements");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "PaymentVouchers");

            migrationBuilder.DropTable(
                name: "ProjectsAssets");

            migrationBuilder.DropTable(
                name: "ProjectsSelectionReasons");

            migrationBuilder.DropTable(
                name: "ProjectsSocialBenefits");

            migrationBuilder.DropTable(
                name: "Kinships");

            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Installments");

            migrationBuilder.DropTable(
                name: "ProjectTypesAssets");

            migrationBuilder.DropTable(
                name: "ProjectSelectionReasons");

            migrationBuilder.DropTable(
                name: "ProjectSocialBenefits");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropTable(
                name: "NumberOfFunds");

            migrationBuilder.DropTable(
                name: "ProjectTypes");

            migrationBuilder.DropTable(
                name: "AccommodationTypes");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "EducationalStatuses");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "HealthStatuses");

            migrationBuilder.DropTable(
                name: "InvestmentTypes");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "MartialStatuses");

            migrationBuilder.DropTable(
                name: "Religions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ProjectDomainTypes");

            migrationBuilder.DropTable(
                name: "ProjectGroups");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
