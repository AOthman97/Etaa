using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class Family
    {
        [Key]
        public int FamilyId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Address { get; set; }
        public string? HouseNumber { get; set; }
        public string? Alleyway { get; set; }
        public string? ResidentialSquare { get; set; }
        public string? FirstPhoneNumber { get; set; }
        public string? SecondPhoneNumber { get; set; }
        public string? NationalNumber { get; set; }
        public string? PassportNumber { get; set; }
        public int? NumberOfIndividuals { get; set; }
        public int? Age { get; set; }
        public decimal? MonthlyIncome { get; set; }
        public bool? IsCurrentInvestmentProject { get; set; }
        public bool? IsApprovedByManagement { get; set; }
        public bool? IsCanceled { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public int DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public District? District { get; set; }
        public int GenderId { get; set; }
        [ForeignKey("GenderId")]
        public Gender? Gender { get; set; }
        public int ReligionId { get; set; }
        [ForeignKey("ReligionId")]
        public Religion? Religion { get; set; }
        public int MartialStatusId { get; set; }
        [ForeignKey("MartialStatusId")]
        public MartialStatus? MartialStatus { get; set; }
        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public Job? Job { get; set; }
        public int HealthStatusId { get; set; }
        [ForeignKey("HealthStatusId")]
        public HealthStatus? HealthStatus { get; set; }
        public int EducationalStatusId { get; set; }
        [ForeignKey("EducationalStatusId")]
        public EducationalStatus? EducationalStatus { get; set; }
        public int AccommodationTypeId { get; set; }
        [ForeignKey("AccommodationTypeId")]
        public AccommodationType? AccommodationType { get; set; }
        public int InvestmentTypeId { get; set; }
        [ForeignKey("InvestmentTypeId")]
        public InvestmentType? InvestmentType { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public int ManagementUserId { get; set; }
        [ForeignKey("UserId")]
        public Users? Users { get; set; }
        
        // Each state contains a number of cities, This property defines the relationship between them
        public ICollection<FamilyMember>? FamilyMembers;
    }
}
