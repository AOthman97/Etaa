using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string? Mobile { get; set; }
        [Display(Name = "Whatsapp Mobile")]
        public string? WhatsappMobile { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public string? Address { get; set; }
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Monthly Share Amount")]
        public decimal? MonthlyShareAmount { get; set; }
        [Display(Name = "Number of Shares")]
        public int? NumberOfShares { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Canceled")]
        public bool IsCanceled { get; set; }

        // Relationship between the contributor and district
        [Display(Name = "District")]
        public int DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public District? District { get; set; }
    }
}
