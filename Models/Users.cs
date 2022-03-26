using System.ComponentModel.DataAnnotations;

namespace Etaa.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
        public string Role { get; set; }
        public string? Signature { get; set; }
        public string? ProfileImagePath { get; set; }

        public ICollection<Family>? Families;
        public ICollection<Projects>? Projects;
        public ICollection<FinancialStatement>? FinancialStatements;
        public ICollection<PaymentVoucher>? PaymentVouchers;
        public ICollection<Clearance>? Clearances;
    }
}
