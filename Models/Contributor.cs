using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class Contributor
    {
        [Key]
        public int ContributorId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        [Display(Name = "Name (En)")]
        public string? NameEn { get; set; }
        public string? Mobile { get; set; }
        [Display(Name = "Whatsapp Mobile")]
        public string? WhatsappMobile { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Monthly Share Amount")]
        public decimal? MonthlyShareAmount { get; set; }
        [Display(Name = "Number of Shares")]
        public int? NumberOfShares { get; set; }
        [Display(Name = "Active")]
        public bool? IsActive { get; set; }
        [Display(Name = "Canceled")]
        public bool? IsCanceled { get; set; }

        // Relationship between the contributor and district
        [Display(Name = "District")]
        public int DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public District? District { get; set; }
    }
}
