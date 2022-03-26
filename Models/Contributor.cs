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
        public string? NameEn { get; set; }
        public string? Mobile { get; set; }
        public string? WhatsappMobile { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MonthlyShareAmount { get; set; }
        public int? NumberOfShares { get; set; }
        public bool? IsActive { get; set; }

        // Relationship between the contributor and district
        public int DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public District? District { get; set; }
    }
}
