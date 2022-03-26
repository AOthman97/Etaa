using Etaa.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Etaa.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        // Each individual db set represents a table in our DB
        public DbSet<AccommodationType> AccommodationTypes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Clearance> Clearances { get; set; }
        public DbSet<Contributor> Contributors { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<EducationalStatus> EducationalStatuses { get; set; }
        public DbSet<EventTypes> EventTypes { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }
        public DbSet<FinancialStatement> FinancialStatements { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<HealthStatus> HealthStatuses { get; set; }
        public DbSet<Installments> Installments { get; set; }
        public DbSet<InvestmentType> InvestmentTypes { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Kinship> Kinships { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<MartialStatus> MartialStatuses { get; set; }
        public DbSet<Modules> Modules { get; set; }
        public DbSet<NumberOfFunds> NumberOfFunds { get; set; }
        public DbSet<PaymentVoucher> PaymentVouchers { get; set; }
        public DbSet<ProjectDomainTypes> ProjectDomainTypes { get; set; }
        public DbSet<ProjectGroup> ProjectGroups { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<ProjectsAssets> ProjectsAssets { get; set; }
        public DbSet<ProjectSelectionReasons> ProjectSelectionReasons { get; set; }
        public DbSet<ProjectSocialBenefits> ProjectSocialBenefits { get; set; }
        public DbSet<ProjectsSelectionReasons> ProjectsSelectionReasons { get; set; }
        public DbSet<ProjectsSocialBenefits> ProjectsSocialBenefits { get; set; }
        public DbSet<ProjectTypes> ProjectTypes { get; set; }
        public DbSet<ProjectTypesAssets> ProjectTypesAssets { get; set; }
        public DbSet<Religion> Religions { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}