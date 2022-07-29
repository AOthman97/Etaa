namespace Etaa.Models
{
    public class FamilyMember
    {
        [Key]
        public int FamilyMemberId { get; set; }
        [Required(ErrorMessage = "!حقل الإسم عربي مطلوب")]
        [Display(Name = "الإسم عربي")]
        public string NameAr { get; set; }
        [Required(ErrorMessage = "!حقل الإسم إنجليزي مطلوب")]
        [Display(Name = "الإسم إنجليزي")]
        public string NameEn { get; set; }
        [Display(Name = "العمر")]
        public int? Age { get; set; }
        [Display(Name = "الملاحظات")]
        public string? Note { get; set; }
        [Display(Name = "هل تم الحذف")]
        public bool IsCanceled { get; set; }

        // Relationship between the family member and kinship
        [Display(Name = "صلة القرابة")]
        public int KinshipId { get; set; }
        [ForeignKey("KinshipId")]
        public Kinship? Kinship { get; set; }

        // Relationship between the family member and gender
        [Display(Name = "النوع")]
        public int GenderId { get; set; }
        [ForeignKey("GenderId")]
        public Gender? Gender { get; set; }

        // Relationship between the family member and educational status
        [Display(Name = "الحالة التعليمية")]
        public int EducationalStatusId { get; set; }
        [ForeignKey("EducationalStatusId")]
        public EducationalStatus? EducationalStatus { get; set; }

        // Relationship between the family member and job
        [Display(Name = "الوظيفة")]
        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public Job? Job { get; set; }
        // Relationship between the family member and family
        [Display(Name = "الأسرة")]
        public int? FamilyId { get; set; }
        [ForeignKey("FamilyId")]
        public Family? Family { get; set; }
    }
}
