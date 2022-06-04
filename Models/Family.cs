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
        [Display(Name = "حقل العنوان")]
        public string? Address { get; set; }
        [Display(Name = "حقل رقم البيت")]
        public string? HouseNumber { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "حقل الحارة")]
        public string? Alleyway { get; set; }
        [Display(Name = "حقل المربع السكني")]
        [DataType(DataType.Text)]
        public string? ResidentialSquare { get; set; }
        [Display(Name = "حقل رقم الهاتف الأول")]
        public string? FirstPhoneNumber { get; set; }
        [Display(Name = "حقل رقم الهاتف الثاني")]
        public string? SecondPhoneNumber { get; set; }
        [Display(Name = "حقل الرقم الوطني")]
        public string? NationalNumber { get; set; }
        [Display(Name = "حقل رقم الجواز")]
        public string? PassportNumber { get; set; }
        [Display(Name = "حقل عدد أفراد الأسرة")]
        public int? NumberOfIndividuals { get; set; }
        [Display(Name = "حقل العمر")]
        public int? Age { get; set; }
        [Display(Name = "حقل الدخل الشهري")]
        public decimal? MonthlyIncome { get; set; }
        [Display(Name = "هل يوجد مشروع إستثماري حاليا")]
        public bool IsCurrentInvestmentProject { get; set; }
        [Display(Name = "هل تمت الموافقة من الإدارة")]
        public bool IsApprovedByManagement { get; set; }
        [Display(Name = "هل تم الحذف")]
        public bool IsCanceled { get; set; }
        [Display(Name = "حقل تاريخ الميلاد")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "حقل المحلية")]
        public int? DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public District? District { get; set; }
        [Display(Name = "حقل النوع")]
        public int GenderId { get; set; }
        [ForeignKey("GenderId")]
        public Gender? Gender { get; set; }
        [Display(Name = "حقل الديانة")]
        public int ReligionId { get; set; }
        [ForeignKey("ReligionId")]
        public Religion? Religion { get; set; }
        [Display(Name = "حقل الحالة الإجتماعية")]
        public int? MartialStatusId { get; set; }
        [ForeignKey("MartialStatusId")]
        public MartialStatus? MartialStatus { get; set; }
        [Display(Name = "حقل الوظيفة")]
        public int? JobId { get; set; }
        [ForeignKey("JobId")]
        public Job? Job { get; set; }
        [Display(Name = "حقل الحالة الصحية")]
        public int? HealthStatusId { get; set; }
        [ForeignKey("HealthStatusId")]
        public HealthStatus? HealthStatus { get; set; }
        [Display(Name = "حقل الحالة التعليمية")]
        public int? EducationalStatusId { get; set; }
        [ForeignKey("EducationalStatusId")]
        public EducationalStatus? EducationalStatus { get; set; }
        [Display(Name = "حقل نوع الإقامة")]
        public int? AccommodationTypeId { get; set; }
        [ForeignKey("AccommodationTypeId")]
        public AccommodationType? AccommodationType { get; set; }
        [Display(Name = "حقل نوع الإستثمار")]
        public int? InvestmentTypeId { get; set; }
        [ForeignKey("InvestmentTypeId")]
        public InvestmentType? InvestmentType { get; set; }
        [Display(Name = "حقل إسم المستخدم")]
        public string? UserId { get; set; }
        [Display(Name = "حقل إسم مستخدم الإدارة")]
        public string? ManagementUserId { get; set; }

        // New 1-Many relationship with the Projects model
        [ForeignKey("FamilyId")]
        public ICollection<Projects>? Projects { get; set; }

        // Each state contains a number of cities, This property defines the relationship between them
        public ICollection<FamilyMember>? FamilyMembers;
    }
}
