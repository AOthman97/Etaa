using System.ComponentModel.DataAnnotations;

namespace Etaa.Models
{
    public class Installments
    {
        [Key]
        public int InstallmentsId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? InstallmentNumber { get; set; }
    }
}
