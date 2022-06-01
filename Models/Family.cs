using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class Family
    {
        [Key]
        public int FamilyId { get; set; }
        [Required(ErrorMessage = "حقل الإسم عربي مطلوب!")]
        [DataType(DataType.Text)]
        [Display(Name = "حقل الإسم عربي")]
        public string NameAr { get; set; }
        [Display(Name = "حقل الإسم إنجليزي")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "حقل الإسم إنجليزي مطلوب!")]
        public string NameEn { get; set; }
        [DataType(DataType.Text)]
        public string? Address { get; set; }
        [Display(Name = "House Number")]
        public string? HouseNumber { get; set; }
        [DataType(DataType.Text)]
        public string? Alleyway { get; set; }
        [Display(Name = "Residential Square")]
        [DataType(DataType.Text)]
        public string? ResidentialSquare { get; set; }
        [Display(Name = "First Phone Number")]
        public string? FirstPhoneNumber { get; set; }
        [Display(Name = "Second Phone Number")]
        public string? SecondPhoneNumber { get; set; }
        [Display(Name = "National Number")]
        public string? NationalNumber { get; set; }
        [Display(Name = "Passport Number")]
        public string? PassportNumber { get; set; }
        [Display(Name = "Number of Individuals")]
        public int? NumberOfIndividuals { get; set; }
        public int? Age { get; set; }
        [Display(Name = "Monthly Income")]
        public decimal? MonthlyIncome { get; set; }
        [Display(Name = "Have Current Investment Project")]
        public bool IsCurrentInvestmentProject { get; set; }
        [Display(Name = "Approved By Management")]
        public bool? IsApprovedByManagement { get; set; }
        [Display(Name = "Canceled")]
        public bool IsCanceled { get; set; }
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "District")]
        public int? DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public District? District { get; set; }
        [Display(Name = "Gender")]
        public int GenderId { get; set; }
        [ForeignKey("GenderId")]
        public Gender? Gender { get; set; }
        [Display(Name = "Religion")]
        public int ReligionId { get; set; }
        [ForeignKey("ReligionId")]
        public Religion? Religion { get; set; }
        [Display(Name = "Martial Status")]
        public int? MartialStatusId { get; set; }
        [ForeignKey("MartialStatusId")]
        public MartialStatus? MartialStatus { get; set; }
        [Display(Name = "Job")]
        public int? JobId { get; set; }
        [ForeignKey("JobId")]
        public Job? Job { get; set; }
        [Display(Name = "Health Status")]
        public int? HealthStatusId { get; set; }
        [ForeignKey("HealthStatusId")]
        public HealthStatus? HealthStatus { get; set; }
        [Display(Name = "Educational Status")]
        public int? EducationalStatusId { get; set; }
        [ForeignKey("EducationalStatusId")]
        public EducationalStatus? EducationalStatus { get; set; }
        [Display(Name = "Accommodation Type")]
        public int? AccommodationTypeId { get; set; }
        [ForeignKey("AccommodationTypeId")]
        public AccommodationType? AccommodationType { get; set; }
        [Display(Name = "Investment Type")]
        public int? InvestmentTypeId { get; set; }
        [ForeignKey("InvestmentTypeId")]
        public InvestmentType? InvestmentType { get; set; }
        public string? UserId { get; set; }
        public string? ManagementUserId { get; set; }

        // New 1-Many relationship with the Projects model
        [ForeignKey("FamilyId")]
        public ICollection<Projects>? Projects { get; set; }

        // Each state contains a number of cities, This property defines the relationship between them
        public ICollection<FamilyMember>? FamilyMembers;
    }
}
