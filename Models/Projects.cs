namespace Etaa.Models
{
    public class Projects
    {
        [Key]
        public int ProjectId { get; set; }
        // Make the name an auto-generated field, how?
        // When saving or editing the project data it should concatenate the ProjectTypeName with the FamilyName, One for
        // NameAr, the other for NameEn
        [Display(Name = "الإسم عربي")]
        public string? NameAr { get; set; }
        [Display(Name = "الإسم إنجليزي")]
        public string? NameEn { get; set; }
        [Display(Name = "مستند المشروع")]
        public string? SignatureofApplicantPath { get; set; }
        [Display(Name = "نشاط المشروع")]
        public string? ProjectActivity { get; set; }
        [Display(Name = "الغرض من المشروع")]
        public string? ProjectPurpose { get; set; }
        [Required(ErrorMessage = "!حقل رأس المال مطلوب")]
        [Display(Name = "رأس مال المشروع")]
        public decimal? Capital { get; set; }
        [Display(Name = "مبلغ القسط الشهري")]
        [Required(ErrorMessage = "!حقل القسط الشهري مطلوب")]
        public decimal? MonthlyInstallmentAmount { get; set; }
        [Display(Name = "عدد الأقساط")]
        [Required(ErrorMessage = "!حقل عدد الأقساط مطلوب")]
        public int? NumberOfInstallments { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "التاريخ")]
        public DateTime? Date { get; set; }
        [Display(Name = "فترة السماحية")]
        [DataType(DataType.Date)]
        public DateTime? WaiverPeriod { get; set; }
        [Display(Name = "هل تمت الموافقة من قبل الإدارة")]
        public bool IsApprovedByManagement { get; set; }
        [Display(Name = "هل تم الحذف")]
        public bool IsCanceled { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ إستحقاق أول قسط")]
        [Required(ErrorMessage = "!حقل تاريخ إستحقاق القسط الأول مطلوب")]
        public DateTime? FirstInstallmentDueDate { get; set; }

        // Relationship between the projects and families
        [Display(Name = "الأسرة")]
        public int FamilyId { get; set; }
        [Display(Name = "رقم التمويل")]
        public int NumberOfFundsId { get; set; }
        [NotMapped]
        public virtual ICollection<NumberOfFunds>? NumberOfFunds { get; set; }
        // Relationship between the projects and families
        [Display(Name = "نوع المشروع")]
        public int ProjectTypeId { get; set; }
        [NotMapped]
        public virtual ICollection<ProjectTypes>? ProjectTypes { get; set; }
        [Display(Name = "إسم المستخدم")]
        public string? UserId { get; set; }
        [Display(Name = "إسم مستخدم الإدارة")]
        public string? ManagementUserId { get; set; }
        [Display(Name = "رقم المشروع")]
        public int? ProjectNumber { get; set; } = 0;

        [ForeignKey("ProjectId")]
        [NotMapped]
        public virtual ICollection<ProjectsAssets>? ProjectsAssets { get; set; }
        [ForeignKey("ProjectId")]
        [NotMapped]
        public virtual ICollection<ProjectsSelectionReasons>? ProjectsSelectionReasons { get; set; }
        [NotMapped]
        public virtual ICollection<MultiSelectList>? ProjectsSelectionReasonsList { get; set; }
        [ForeignKey("ProjectId")]
        [NotMapped]
        public virtual ICollection<ProjectsSocialBenefits>? ProjectsSocialBenefits { get; set; }
        [NotMapped]
        public virtual ICollection<MultiSelectList>? ProjectsSocialBenefitsList { get; set; }

        [NotMapped]
        public int ProjectSelectionReasonsId { get; set; }
        [NotMapped]
        public virtual ICollection<MultiSelectList>? ProjectSelectionReasons { get; set; }
        [NotMapped]
        public int ProjectSocialBenefitsId { get; set; }
        [NotMapped]
        public virtual ICollection<MultiSelectList>? ProjectSocialBenefits { get; set; }
    }
}
