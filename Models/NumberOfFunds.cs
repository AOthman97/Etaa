using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class NumberOfFunds
    {
        [Key]
        public int NumberOfFundsId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Description { get; set; }
        public int? Order { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public bool? IsCanceled { get; set; }

        [ForeignKey("NumberOfFundsId")]
        public ICollection<Projects>? Projects { get; set; }
    }
}
