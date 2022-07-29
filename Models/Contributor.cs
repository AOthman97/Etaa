namespace Etaa.Models
{
    public class Contributor
    {
        [Key]
        public int ContributorId { get; set; }
        [Required(ErrorMessage = "!حقل الإسم عربي مطلوب")]
        [Display(Name = "الإسم عربي")]
        public string NameAr { get; set; }
        [Required(ErrorMessage = "!حقل الإسم إنجليزي مطلوب")]
        [Display(Name = "الإسم إنجليزي")]
        public string? NameEn { get; set; }
        [Display(Name = "رقم التلفون")]
        public string? Mobile { get; set; }
        [Display(Name = "رقم الواتساب")]
        public string? WhatsappMobile { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "البريد الإلكتروني")]
        public string? Email { get; set; }
        [Display(Name = "العنوان")]
        public string? Address { get; set; }
        [Display(Name = "تاريخ البداية")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "تاريخ الإنتهاء")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        [Display(Name = "مبلغ القسط الشهري")]
        public decimal? MonthlyShareAmount { get; set; }
        [Display(Name = "عدد الأسهم")]
        public int? NumberOfShares { get; set; }
        [Display(Name = "نشط حاليا")]
        public bool IsActive { get; set; }
        [Display(Name = "هل تم الحذف")]
        public bool IsCanceled { get; set; }

        // Relationship between the contributor and district
        [Display(Name = "المحلية")]
        public int DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public District? District { get; set; }
    }
}
